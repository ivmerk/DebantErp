using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public class AuthSessionDAL : IAuthSessionDAL
    {
        public async Task<int> Create(AuthSessionModel model)
        {
            const string sql = @"
                INSERT INTO AuthSessions (SessionId, UserId, SessionToken, IpAddress, UserAgent, CreatedAt, LastAccessedAt, ExpiresAt, IsActive)
                VALUES (@SessionId, @UserId, @SessionToken, @IpAddress, @UserAgent, @CreatedAt, @LastAccessedAt, @ExpiresAt, @IsActive)";
            return await DbHelper.ExecuteAsync(sql, model);
        }

        public async Task<int> DeactivateByUserId(int userId)
        {
            const string sql = "UPDATE AuthSessions SET IsActive = false WHERE UserId = @userId AND IsActive = true";
            return await DbHelper.ExecuteAsync(sql, new { userId });
        }
    }
}
