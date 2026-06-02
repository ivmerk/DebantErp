using DebantErp.DAL.Models;

namespace DebantErp.DAL;
public class AuthDAL : IAuthDAL
{
  public Task<List<UserModel>> Get()
  {
      throw new NotImplementedException();
  }

  public async Task<UserModel> Get(string email)
  {
      var result = await DbHelper.QueryAsync<UserModel>(
          "SELECT * FROM users WHERE email = @email",
          new { email }
      );
      return result.FirstOrDefault() ?? new UserModel();
  }

  public async Task<UserModel> Get(int id)
  {
      var result = await DbHelper.QueryAsync<UserModel>(
          "SELECT id, first_name AS FirstName, last_name AS LastName, phone, role, email, status FROM users WHERE id = @id",
          new { id }
      );
      return result.FirstOrDefault() ?? new UserModel();
  }

  public async Task<int> Create(UserModel model)
  {
      string sql =
          "INSERT INTO users ( first_name, last_name, phone, role, email, password, salt, status) VALUES (@firstName, @lastName, @phone, @role, @email, @password,  @salt, @status)";
      return await DbHelper.ExecuteScalarAsync<int>(sql, model);
  }

  public async Task<bool> IsExist(int id)
  {
      string sql = "SELECT EXISTS (SELECT 1 FROM users WHERE id = @Id)";
      return await DbHelper.ExecuteScalarAsync<bool>(sql, new { id });
  }
  public async Task<int> Update(UserModel model)
  {
      string sql =
          "UPDATE users SET first_name = @firstName, last_name = @LastName, phone = @phone, role = @role, email = @email, status = @status, updated_at = CAST(@UpdatedAt AS timestamp with time zone) WHERE id = @id";
      return await DbHelper.ExecuteAsync(sql, model);
  }

  public Task<int> Delete(int id)
  {
      throw new NotImplementedException();
  }
}
