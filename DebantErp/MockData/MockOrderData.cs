using Dapper;
using Npgsql;

public class MockOrderData
{
    private readonly string _connectionString;

    public MockOrderData(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InsertAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var orderCount = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM orders");
        if (orderCount > 0)
            return;

        var orders = new List<string>
        {
            "R0001",
            "R0002",
            "R0003",
            "R0004",
            "R0005",
            "R0006",
            "R0007",
            "R0008",
            "R0009",
            "R0010",
            "R0011",
            "R0012",
            "R0013",
            "R0014",
            "R0015",
            "R0016",
            "R0017",
            "R0018",
            "R0019",
            "R0020",
            "R0021",
            "R0022",
        };

        try
        {
            foreach (var order in orders)
            {
                string insertOrderSql = @"INSERT INTO orders (number) VALUES (@Number)";

                await connection.ExecuteAsync(insertOrderSql, new { Number = order });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
