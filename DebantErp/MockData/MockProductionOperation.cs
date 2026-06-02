using Dapper;
using DebantErp.DAL.Models;
using Npgsql;

public class MockProductionOperation
{
    private readonly string _connectionString;

    public MockProductionOperation(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InsertAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var operationCount = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM production_operations"
        );
        if (operationCount > 0)
            return;

        var operations = new List<string>
        {
            "Порезка дна",
            "Порезка петли",
            "Порезка манжеты",
            "Печать тела",
        };
        try
        {
            foreach (var operation in operations)
            {
                string insertOperationSql =
                    @"INSERT INTO production_operations (name) VALUES (@Name)";
                await connection.ExecuteAsync(insertOperationSql, new { Name = operation });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
