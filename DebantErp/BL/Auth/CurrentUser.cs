namespace DebantErp.BL.Auth;

public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
          _httpContextAccessor = httpContextAccessor;
        }

        public bool IsLoggedIn()
        {
          return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;
        }
    }
