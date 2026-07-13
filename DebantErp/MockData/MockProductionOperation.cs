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

        // Разряд у каждой операции ссылается на номер разряда из таблицы grades (1..6).
        var operations = new List<object>
        {
            new { Name = "Порізка дна", Code = "CUT-BOTTOM", Grade = 1 },
            new { Name = "Порізка петлі", Code = "CUT-LOOP", Grade = 2 },
            new { Name = "Порізка манжети", Code = "CUT-CUFF", Grade = 3 },
            new { Name = "Друк тіла", Code = "PRINT-BODY", Grade = 4 },
        };
        try
        {
            foreach (var operation in operations)
            {
                string insertOperationSql =
                    @"INSERT INTO production_operations (name, code, grade) VALUES (@Name, @Code, @Grade)";
                await connection.ExecuteAsync(insertOperationSql, operation);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
