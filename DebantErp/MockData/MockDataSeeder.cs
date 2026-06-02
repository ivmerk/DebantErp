using DebantErp.BL.Auth;

namespace DebantErp.MockData
{
    public class MockDataSeeder
    {
        private readonly string _connectionString;
        private readonly IEncrypt _encrypt;

        public MockDataSeeder(string connectionString, IEncrypt encrypt)
        {
            _connectionString = connectionString;
            _encrypt = encrypt;
        }

        public async Task SeedAsync()
        {
            var userMock = new MockUserData(_connectionString, _encrypt);
            var employeeMock = new MockEmployeeData(_connectionString);
            var specialityMock = new MockSpecialityData(_connectionString);
            var orderMock = new MockOrderData(_connectionString);
            var productionOperationMock = new MockProductionOperation(_connectionString);
            var productionRateMock = new MockProductionRate(_connectionString);

            await userMock.InsertAsync();
            await employeeMock.InsertAsync();
            await specialityMock.InsertAsync();
            await orderMock.InsertAsync();
            await productionOperationMock.InsertAsync();
            await productionRateMock.InsertAsync();

            Console.WriteLine("Мок-данные добавлены!");
        }
    }
}
