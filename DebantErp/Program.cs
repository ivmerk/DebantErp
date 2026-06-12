using DebantErp.DAL;
using DebantErp.BL.Auth;
using DebantErp.BL.Employee;
using DebantErp.MockData;
using System.IO;
using Microsoft.AspNetCore.DataProtection;

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
builder.Services.AddSingleton<ISpecialityDAL, SpecialityDAL>();
builder.Services.AddSingleton<IEmployeeSpecialityAssignmentDAL, EmployeeSpecialityAssignmentDAL>();
builder.Services.AddSingleton<IOrderDAL, OrderDAL>();
builder.Services.AddSingleton<IOrderLaborCostDAL, OrderLaborCostDAL>();

builder.Services.AddSingleton<IEncrypt, Encrypt>();
builder.Services.AddSingleton<IAuthSessionDAL, AuthSessionDAL>();
builder.Services.AddScoped<IAuth, Auth>();
builder.Services.AddScoped<IEmployee, Employee>();
builder.Services.AddScoped<IEmployeeDetails, EmployeeDetails>();
builder.Services.AddScoped<IEmployeeSpecialityAssignment, EmployeeSpecialityAssignment>();
builder.Services.AddScoped<DebantErp.BL.Speciality.ISpeciality, DebantErp.BL.Speciality.Speciality>();
builder.Services.AddScoped<DebantErp.BL.Order.IOrder, DebantErp.BL.Order.Order>();
builder.Services.AddScoped<DebantErp.BL.Auth.ICurrentUser, DebantErp.BL.Auth.CurrentUser>();
builder.Services.AddScoped<DebantErp.BL.OrderLaborCost.IOrderLaborCost, DebantErp.BL.OrderLaborCost.OrderLostCost>();

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
  options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
  options.UseUtcTimestamp = false;
});
builder.Logging.AddDebug();


builder.Services.AddSingleton(provider =>
{
  var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
  var logger = loggerFactory.CreateLogger("DbHelper");
  DebantErp.DAL.DbHelper.SetLogger(logger);
  return new object();
});


// Add session services
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
  options.IdleTimeout = TimeSpan.FromMinutes(30);
  options.Cookie.HttpOnly = true;
  options.Cookie.IsEssential = true;
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

app.UseRouting();

app.UseSession();

app.UseCors("AllowAll");

app.UseAuthorization();
var encryptService = app.Services.GetRequiredService<DebantErp.BL.Auth.IEncrypt>();
var seeder = new MockDataSeeder(
    DebantErp.DAL.DbHelper.ConnectionString,
    encryptService
);
await seeder.SeedAsync();

app.MapHealthChecks("/health");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

