namespace DebantErp.DAL.Models
{
    public class AuthSessionModel
    {
        public Guid SessionId { get; set; }
        public int UserId { get; set; }
        public string SessionToken { get; set; } = string.Empty;
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastAccessedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; }
    }
}
