using Dapper;
using Npgsql;

public class MockGrade
{
    private readonly string _connectionString;

    public MockGrade(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InsertAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var gradeCount = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM grades");
        if (gradeCount > 0)
            return;
        try
        {
            var effectiveDate = new DateTime(2025, 1, 1);
            // Разряды 1..6 с растущей дневной ставкой (оклад = ставка × 21 р.д.).
            for (int grade = 1; grade <= 6; grade++)
            {
                string insertGradeSql =
                    @"INSERT INTO grades (grade, daily_rate, effective_date) VALUES (@Grade, @DailyRate, @EffectiveDate)";
                await connection.ExecuteAsync(
                    insertGradeSql,
                    new
                    {
                        Grade = grade,
                        DailyRate = 800m + (grade - 1) * 200m,
                        EffectiveDate = effectiveDate,
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
