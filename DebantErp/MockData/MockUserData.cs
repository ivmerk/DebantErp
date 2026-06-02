using Dapper;
using DebantErp.BL.Auth;
using DebantErp.DAL.Models;
using Npgsql;

public class MockUserData
{
    private readonly string _connectionString;

    private readonly IEncrypt _encrypt;

    public MockUserData(string connectionString, IEncrypt encrypt)
    {
        _encrypt = encrypt;
        _connectionString = connectionString;
    }

    public async Task InsertAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var userCount = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM users");
        if (userCount > 0)
            return;

        var salt = Guid.NewGuid().ToString();
        var mockUsers = new List<UserModel>
        {
            new UserModel
            {
                FirstName = "Ivan",
                LastName = "Merkulov",
                Phone = "1234567890",
                Role = UserRoleEnum.Admin,
                Email = "s2P7D@example.com",
                Password = _encrypt.HashPassword("123456", salt),
                Salt = salt,
                Status = UserStatusEnum.Active,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            },
            new UserModel
            {
                FirstName = "Sergei",
                LastName = "Fedotov",
                Phone = "45461234590",
                Role = UserRoleEnum.Admin,
                Email = "adssjs2jlD@example.com",
                Password = _encrypt.HashPassword("123456", salt),
                Salt = salt,
                Status = UserStatusEnum.Active,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            },
        };

        try
        {
            foreach (var user in mockUsers)
                if (user != null)
                {
                    string sql =
                        "INSERT INTO users (first_name, last_name, phone, role, email, password, salt, status) VALUES (@firstName, @lastName, @phone, @role, @email, @password,  @salt, @status)";
                    await connection.ExecuteAsync(sql, user);
                }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
