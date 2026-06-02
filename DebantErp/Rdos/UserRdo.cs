using DebantErp.DAL.Models;

namespace DebantErp.Rdos
{
    public class UserRdo
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Phone { get; set; } = "";
        public UserRoleEnum Role { get; set; }
        public string Email { get; set; } = "";
        public UserStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
