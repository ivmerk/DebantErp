using DebantErp.DAL.Models;

namespace DebantErp.Dtos
{
    public class UpdateUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public UserRoleEnum? Role { get; set; }
        public string? Email { get; set; }
        public UserStatusEnum? Status { get; set; }
    }
}
