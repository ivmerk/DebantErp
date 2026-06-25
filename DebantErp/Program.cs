using DebantErp.DAL;
using DebantErp.BL.Auth;
using DebantErp.BL.Employee;
using DebantErp.MockData;
using System.IO;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Serilog;

// Bootstrap-логгер: ловит ошибки ещё до построения хоста (DI, конфиг, сидер).
// После builder.Build() заменяется на полноценный из appsettings (секция "Serilog").
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
  var builder = WebApplication.CreateBuilder(args);
  builder.Services.AddControllersWithViews();
  builder.Services.AddHealthChecks();

  DebantErp.DAL.DbHelper.ConnectionString =
      builder.Configuration.GetConnectionString("DefaultConnection")
      ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is not configured.");

  builder.Services.AddHttpContextAccessor();
  // Configure Data Protection for persistent keys
  builder.Services.AddDataProtection()
      .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, ".data-protection")))
      .SetApplicationName("DebantErp")
      .SetDefaultKeyLifetime(TimeSpan.FromDays(365));
  // Add services to the container.
  builder.Services.AddSingleton<IAuthDAL, AuthDAL>();
  builder.Services.AddSingleton<IEmployeeDAL, EmployeeDAL>();
  builder.Services.AddSingleton<IEmployeeDetailsDAL, EmployeeDetailsDAL>();
  builder.Services.AddSingleton<ISpecialtyDAL, SpecialtyDAL>();
  builder.Services.AddSingleton<IEmployeeSpecialtyAssignmentDAL, EmployeeSpecialtyAssignmentDAL>();
  builder.Services.AddSingleton<IOrderDAL, OrderDAL>();
  builder.Services.AddSingleton<IOrderLaborCostDAL, OrderLaborCostDAL>();
  builder.Services.AddSingleton<IProductionOperationDAL, ProductionOperationDAL>();
  builder.Services.AddSingleton<IProductionRateDAL, ProductionRateDAL>();

  builder.Services.AddSingleton<IEncrypt, Encrypt>();
  builder.Services.AddScoped<IAuth, Auth>();
  builder.Services.AddScoped<IEmployee, Employee>();
  builder.Services.AddScoped<IEmployeeDetails, EmployeeDetails>();
  builder.Services.AddScoped<IEmployeeSpecialtyAssignment, EmployeeSpecialtyAssignment>();
  builder.Services.AddScoped<DebantErp.BL.Specialty.ISpecialty, DebantErp.BL.Specialty.Specialty>();
  builder.Services.AddScoped<DebantErp.BL.Order.IOrder, DebantErp.BL.Order.Order>();
  builder.Services.AddScoped<DebantErp.BL.Auth.ICurrentUser, DebantErp.BL.Auth.CurrentUser>();
  builder.Services.AddScoped<DebantErp.BL.OrderLaborCost.IOrderLaborCost, DebantErp.BL.OrderLaborCost.OrderLostCost>();
  builder.Services.AddScoped<DebantErp.BL.ProductionRate.IProductionOperation, DebantErp.BL.ProductionRate.ProductionOperation>();
  builder.Services.AddScoped<DebantErp.BL.ProductionRate.IProductionRate, DebantErp.BL.ProductionRate.ProductionRate>();

  // Serilog: конфиг читается из appsettings (секция "Serilog") — уровни, sinks
  // (Console + rolling File в /app/.logs, примонтированный том) можно менять без пересборки.
  builder.Host.UseSerilog((context, configuration) => configuration
      .ReadFrom.Configuration(context.Configuration)
      .Enrich.FromLogContext());


  builder.Services.AddSingleton(provider =>
  {
    var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger("DbHelper");
    DebantErp.DAL.DbHelper.SetLogger(logger);
    return new object();
  });


  builder.Services.AddCors(options =>
  {
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
  });

  // Cookie-аутентификация: identity (userid + role) лежит в зашифрованной cookie.
  // Сервер stateless — рестарт/деплой не разлогинивает (ключи Data Protection
  // персистятся в .data-protection/, см. ниже).
  builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
      options.LoginPath = "/login";
      options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
      options.SlidingExpiration = true;
      options.Cookie.HttpOnly = true;
      options.Cookie.IsEssential = true;
    });

  // Deny-by-default: каждый endpoint без явной authorization-метаданности требует
  // аутентификации. Публичные точки помечены [AllowAnonymous] (Home/Login/Register),
  // /health — .AllowAnonymous() ниже (иначе деплойный healthcheck словит 302 на /login).
  builder.Services.AddAuthorization(options =>
  {
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
      .RequireAuthenticatedUser()
      .Build();
  });

  var app = builder.Build();

  // Configure the HTTP request pipeline.
  if (!app.Environment.IsDevelopment())
  {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
  }

  // HTTPS-редирект отключён: в контейнере нет TLS-сертификата, редирект http→https ломает health-check.
  // Включить обратно, когда поставим reverse-proxy с TLS перед приложением.
  // app.UseHttpsRedirection();
  app.UseStaticFiles();

  // Одна строка на HTTP-запрос (метод, путь, статус, длительность). После UseStaticFiles —
  // чтобы не спамить запросами статики.
  app.UseSerilogRequestLogging();

  app.UseRouting();

  app.UseCors("AllowAll");

  app.UseAuthentication();
  app.UseAuthorization();
  var encryptService = app.Services.GetRequiredService<DebantErp.BL.Auth.IEncrypt>();
  var seeder = new MockDataSeeder(
      DebantErp.DAL.DbHelper.ConnectionString,
      encryptService
  );
  await seeder.SeedAsync();

  app.MapHealthChecks("/health").AllowAnonymous();

  app.MapControllerRoute(
      name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}");

  app.Run();
}
catch (Exception ex)
{
  Log.Fatal(ex, "Приложение аварийно завершилось при старте");
}
finally
{
  Log.CloseAndFlush();
}
