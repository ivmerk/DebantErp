namespace DebantErp.DAL.Models
{
    public class UserModel
    {
        public int? Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public UserRoleEnum Role { get; set; } = UserRoleEnum.User;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Salt { get; set; } = null!;

        public GenderEnum Gender { get; set; }

        public UserStatusEnum Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
