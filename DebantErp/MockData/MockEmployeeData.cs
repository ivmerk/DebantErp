using Dapper;
using DebantErp.DAL.Models;
using Npgsql;

public class MockEmployeeData
{
    private readonly string _connectionString;

    public MockEmployeeData(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InsertAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();

        var employeeCount = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM employees"
        );
        if (employeeCount > 0)
            return;

        var mockEmployees = new List<EmployeeModel>
        {
            new EmployeeModel
            {
                FirstName = "Ivan",
                MiddleName = "Evgeniyevich",
                LastName = "Merkulov",
                IsActual = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                EmployeeDetails = new EmployeeDetailsModel
                {
                    TaxCode = "44546542246",
                    Address = "123 Main St",
                    Email = "s2466546P7D@example.com",
                    Phone = "1234567890",
                    BirthDate = new DateTime(1990, 5, 15),
                    Gender = GenderEnum.Male,
                    Picture = "https://example.com/picture1.jpg",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
            },
            new EmployeeModel
            {
                FirstName = "Alexey",
                MiddleName = "Sergeevich",
                LastName = "Petrov",
                IsActual = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                EmployeeDetails = new EmployeeDetailsModel
                {
                    TaxCode = "33446542247",
                    Address = "456 Elm St",
                    Email = "petrov@example.com",
                    Phone = "2345678901",
                    BirthDate = new DateTime(1985, 7, 22),
                    Gender = GenderEnum.Male,
                    Picture = "https://example.com/picture2.jpg",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
            },
            new EmployeeModel
            {
                FirstName = "Elena",
                MiddleName = "Vladimirovna",
                LastName = "Sidorova",
                IsActual = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                EmployeeDetails = new EmployeeDetailsModel
                {
                    TaxCode = "55546542248",
                    Address = "789 Oak St",
                    Email = "sidorova@example.com",
                    Phone = "3456789012",
                    BirthDate = new DateTime(1992, 3, 10),
                    Gender = GenderEnum.Female,
                    Picture = "https://example.com/picture3.jpg",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
            },
            new EmployeeModel
            {
                FirstName = "Sergey",
                MiddleName = "Igorevich",
                LastName = "Smirnov",
                IsActual = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                EmployeeDetails = new EmployeeDetailsModel
                {
                    TaxCode = "66746542249",
                    Address = "321 Pine St",
                    Email = "smirnov@example.com",
                    Phone = "4567890123",
                    BirthDate = new DateTime(1988, 11, 5),
                    Gender = GenderEnum.Male,
                    Picture = "https://example.com/picture4.jpg",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
            },
            new EmployeeModel
            {
                FirstName = "Anna",
                MiddleName = "Alexandrovna",
                LastName = "Ivanova",
                IsActual = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                EmployeeDetails = new EmployeeDetailsModel
                {
                    TaxCode = "77846542250",
                    Address = "654 Cedar St",
                    Email = "ivanova@example.com",
                    Phone = "5678901234",
                    BirthDate = new DateTime(1995, 8, 18),
                    Gender = GenderEnum.Female,
                    Picture = "https://example.com/picture5.jpg",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
            },
            new EmployeeModel
            {
                FirstName = "Dmitry",
                MiddleName = "Pavlovich",
                LastName = "Kuznetsov",
                IsActual = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                EmployeeDetails = new EmployeeDetailsModel
                {
                    TaxCode = "88946542251",
                    Address = "987 Maple St",
                    Email = "kuznetsov@example.com",
                    Phone = "6789012345",
                    BirthDate = new DateTime(1983, 6, 25),
                    Gender = GenderEnum.Male,
                    Picture = "https://example.com/picture6.jpg",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
            },
            new EmployeeModel
            {
                FirstName = "Natalia",
                MiddleName = "Sergeevna",
                LastName = "Volkova",
                IsActual = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                EmployeeDetails = new EmployeeDetailsModel
                {
                    TaxCode = "99046542252",
                    Address = "123 Birch St",
                    Email = "volkova@example.com",
                    Phone = "7890123456",
                    BirthDate = new DateTime(1991, 12, 9),
                    Gender = GenderEnum.Female,
                    Picture = "https://example.com/picture7.jpg",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
            },
            new EmployeeModel
            {
                FirstName = "Maxim",
                MiddleName = "Andreevich",
                LastName = "Orlov",
                IsActual = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                EmployeeDetails = new EmployeeDetailsModel
                {
                    TaxCode = "11146542253",
                    Address = "456 Walnut St",
                    Email = "orlov@example.com",
                    Phone = "8901234567",
                    BirthDate = new DateTime(1989, 4, 3),
                    Gender = GenderEnum.Male,
                    Picture = "https://example.com/picture8.jpg",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
            },
            new EmployeeModel
            {
                FirstName = "Svetlana",
                MiddleName = "Petrovna",
                LastName = "Morozova",
                IsActual = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                EmployeeDetails = new EmployeeDetailsModel
                {
                    TaxCode = "22246542254",
                    Address = "789 Chestnut St",
                    Email = "morozova@example.com",
                    Phone = "9012345678",
                    BirthDate = new DateTime(1994, 1, 29),
                    Gender = GenderEnum.Female,
                    Picture = "https://example.com/picture9.jpg",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
            },
            new EmployeeModel
            {
                FirstName = "Roman",
                MiddleName = "Viktorovich",
                LastName = "Fedorov",
                IsActual = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                EmployeeDetails = new EmployeeDetailsModel
                {
                    TaxCode = "33346542255",
                    Address = "321 Willow St",
                    Email = "fedorov@example.com",
                    Phone = "0123456789",
                    BirthDate = new DateTime(1987, 10, 14),
                    Gender = GenderEnum.Male,
                    Picture = "https://example.com/picture10.jpg",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
            },
        };

        try
        {
            foreach (var employee in mockEmployees)
                if (employee != null)
                {
                    string insertEmployeeSql =
                        "INSERT INTO employees ( first_name, middle_name, last_name, is_actual, created_at, updated_at) VALUES (@FirstName, @MiddleName, @LastName, @IsActual, @CreatedAt, @UpdatedAt) RETURNING id";
                    int employeeId = await connection.ExecuteScalarAsync<int>(
                        insertEmployeeSql,
                        employee,
                        transaction: transaction
                    );

                    string insertDetailsSql =
                        @"
                    INSERT INTO employees_details (employee_id, tax_code, address, email, phone, birth_date, gender, picture, created_at, updated_at) 
                    VALUES (@EmployeeId, @TaxCode, @Address, @Email, @Phone, @BirthDate, @Gender, @Picture, @CreatedAt, @UpdatedAt);";

                    var employeeDetails = employee.EmployeeDetails;

                    if (employeeDetails != null)
                    {
                        employeeDetails.EmployeeId = employeeId;

                        await connection.ExecuteScalarAsync(
                            insertDetailsSql,
                            employeeDetails,
                            transaction: transaction
                        );
                    }
                }
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await transaction.RollbackAsync();
        }
    }
}
