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
          var userId = _httpContextAccessor.HttpContext?.Session.GetInt32("userid");
          return userId != null;
        }
    }
