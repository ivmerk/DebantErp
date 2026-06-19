using Dapper;
using Npgsql;

namespace DebantErp.DAL
{
    public static class DbHelper
    {
        public static string ConnectionString = string.Empty;

        private static ILogger? _logger;

        static DbHelper()
        {
            // БД использует snake_case (is_actual, created_at, first_name...),
            // а модели — PascalCase. Без этого Dapper не маппит такие колонки
            // на SELECT * и свойства остаются дефолтными (напр. IsActual=true
            // у мягко удалённых записей).
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public static void SetLogger(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public static async Task<int> ExecuteAsync(string sql, object model)
        {
            try
            {
                using (var connection = new NpgsqlConnection(DbHelper.ConnectionString))
                {
                    await connection.OpenAsync();
                    return await connection.ExecuteAsync(sql, model);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Ошибка при выполнении SQL: {Sql}", sql);
                return -1;
            }
        }

        public static async Task<T?> ExecuteScalarAsync<T>(string sql, object model)
        {
            try
            {
                using (var connection = new NpgsqlConnection(DbHelper.ConnectionString))
                {
                    await connection.OpenAsync();
                    return await connection.ExecuteScalarAsync<T>(sql, model);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Ошибка при выполнении SQL: {Sql}", sql);
                return default(T);
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(string sql, object model)
        {
            try
            {
                using (var connection = new NpgsqlConnection(DbHelper.ConnectionString))
                {
                    await connection.OpenAsync();
                    return await connection.QueryAsync<T>(sql, model);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Ошибка при выполнении SQL: {Sql}", sql);
                return new List<T>();
            }
        }
    }
}
