using Dapper;
using Npgsql;

public class MockProductionRate
{
    private readonly string _connectionString;

    public MockProductionRate(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InsertAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var productionRateCount = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM production_rates"
        );
        if (productionRateCount > 0)
            return;
        try
        {
            var productionOperations = new List<int>();
            string getProductionOperationsSql = "SELECT id FROM production_operations";
            productionOperations = (
                await connection.QueryAsync<int>(getProductionOperationsSql)
            ).ToList();

            foreach (var operation in productionOperations)
            {
                string insertProductionRateSql =
                    @"INSERT INTO production_rates (production_operation_id, operation_timeframe, rate) VALUES (@OperationId, @OperationTimeframe, @Rate)";
                await connection.ExecuteAsync(
                    insertProductionRateSql,
                    new
                    {
                        OperationId = operation,
                        OperationTimeframe = operation * 10,
                        Rate = operation * 5,
                    }
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
