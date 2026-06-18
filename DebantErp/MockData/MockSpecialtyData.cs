using Dapper;
using Npgsql;

public class MockSpecialtyData
{
    private readonly string _connectionString;

    public MockSpecialtyData(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InsertAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();

        var specialtyCount = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM specialties"
        );
        if (specialtyCount > 0)
            return;

        var specialties = new List<object>
        {
            new { Name = "Швея", IsActual = true },
            new { Name = "Ткач", IsActual = true },
            new { Name = "Закройщик", IsActual = true },
            new { Name = "Технолог швейного производства", IsActual = true },
            new { Name = "Оператор швейного оборудования", IsActual = true },
        };

        try
        {
            string insertSpecialtiesSql =
                @"
                INSERT INTO specialties (name, is_actual) 
                VALUES (@Name, @IsActual) 
                ON CONFLICT (name) DO NOTHING 
                RETURNING id";
            var specialtyIds = new List<int>();

            foreach (var specialty in specialties)
            {
                var id = await connection.QuerySingleOrDefaultAsync<int>(
                    insertSpecialtiesSql,
                    specialty,
                    transaction
                );
                if (id != 0) // если вставка произошла
                    specialtyIds.Add(id);
            }

            if (!specialtyIds.Any())
            {
                string getExistingSpecialtiesSql = "SELECT id FROM specialties";
                specialtyIds = (
                    await connection.QueryAsync<int>(
                        getExistingSpecialtiesSql,
                        transaction: transaction
                    )
                ).ToList();
            }

            string getEmployeesSql = "SELECT id FROM employees ORDER BY RANDOM() LIMIT 10";
            var employeeIds = (
                await connection.QueryAsync<int>(getEmployeesSql, transaction: transaction)
            ).ToList();

            if (!employeeIds.Any())
            {
                throw new Exception("No employees found. Please insert employees first.");
            }

            var random = new Random();
            var employeeSpecialties = new List<dynamic>();

            foreach (var employeeId in employeeIds)
            {
                var specialtyId = specialtyIds[random.Next(specialtyIds.Count)];
                employeeSpecialties.Add(
                    new
                    {
                        EmployeeId = employeeId,
                        SpecialtyId = specialtyId,
                        DateFrom = DateTime.UtcNow.Date,
                        IsActual = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    }
                );
            }

            string insertEmployeeSpecialtiesSql =
                @"
                INSERT INTO employee_specialty_assignments (employee_id, specialty_id, date_from, is_actual, created_at, updated_at) 
                VALUES (@EmployeeId, @SpecialtyId, @DateFrom, @IsActual, @CreatedAt, @UpdatedAt)";

            await connection.ExecuteAsync(
                insertEmployeeSpecialtiesSql,
                employeeSpecialties,
                transaction
            );

            await transaction.CommitAsync();
            Console.WriteLine("Mock data inserted successfully.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Error inserting mock data: {ex.Message}");
        }
    }
}
