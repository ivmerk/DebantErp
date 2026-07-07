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

        var operations = new List<object>
        {
            new { Name = "Порізка дна", Code = "CUT-BOTTOM" },
            new { Name = "Порізка петлі", Code = "CUT-LOOP" },
            new { Name = "Порізка манжети", Code = "CUT-CUFF" },
            new { Name = "Друк тіла", Code = "PRINT-BODY" },
        };
        try
        {
            foreach (var operation in operations)
            {
                string insertOperationSql =
                    @"INSERT INTO production_operations (name, code) VALUES (@Name, @Code)";
                await connection.ExecuteAsync(insertOperationSql, operation);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
