using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace AgendaWeb.Data
{
    public class SQLServer
    {
        private readonly string _connectionString;
        public SQLServer(string connectionString)
        {
            _connectionString = connectionString;
        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
        public async Task<int> NonQueryAsync(string query, SqlParameter[] parameters = null)
        {
            await using var connection = GetConnection();
            await using var command = new SqlCommand(query, connection);
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            await connection.OpenAsync();
            return await command.ExecuteNonQueryAsync();
        }
        public async Task<T> ScalarAsync<T>(string query, SqlParameter[] parameters = null)
        {
            await using SqlConnection sqlConnection = GetConnection();
            await using SqlCommand sqlCommand = new(query, sqlConnection)
            {
                CommandType = CommandType.Text
            };

            if (parameters is not null)
            {
                sqlCommand.Parameters.AddRange(parameters);
            }

            await sqlConnection.OpenAsync();
            object result = await sqlCommand.ExecuteScalarAsync();

            return result is T value ? value : default;
        }
        public async Task<T> ReaderAsync<T>(string query, SqlParameter[] parameters = null) where T : class, new()
        {
            await using SqlConnection sqlConnection = GetConnection();
            await using SqlCommand sqlCommand = new(query, sqlConnection)
            {
                CommandType = CommandType.Text
            };

            if (parameters is not null)
            {
                sqlCommand.Parameters.AddRange(parameters);
            }

            await sqlConnection.OpenAsync();

            try
            {
                await using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                if (await reader.ReadAsync())
                {
                    T item = new();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);

                        var property = props.FirstOrDefault(p =>
                            string.Equals(p.Name, columnName, StringComparison.OrdinalIgnoreCase));

                        if (property != null && !reader.IsDBNull(i))
                        {
                            object value = reader.GetValue(i);
                            object converted = Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                            property.SetValue(item, converted);
                        }
                    }

                    return item;
                }

                return null;
            }
            catch (SqlException ex)
            {
                var dbName = sqlConnection?.Database ?? "(desconocida)";
                var cmd = sqlCommand?.CommandText ?? "(vacío)";
                throw new InvalidOperationException($"SqlException en BD='{dbName}'; CommandText='{cmd}': {ex.Message}", ex);
            }
        }
        public async Task<List<T>> ReaderListAsync<T>(string query, SqlParameter[] parameters = null) where T : class, new()
        {

            await using SqlConnection sqlConnection = GetConnection();
            await using SqlCommand sqlCommand = new(query, sqlConnection)
            {
                CommandType = CommandType.Text
            };

            if (parameters is not null)
            {
                sqlCommand.Parameters.AddRange(parameters);
            }

            await sqlConnection.OpenAsync();

            try
            {
                await using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                var lista = new List<T>();
                var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                while (await reader.ReadAsync())
                {
                    T item = new();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);

                        var property = props.FirstOrDefault(p =>
                            string.Equals(p.Name, columnName, StringComparison.OrdinalIgnoreCase));

                        if (property != null && !reader.IsDBNull(i))
                        {
                            object value = reader.GetValue(i);
                            object converted = Convert.
                                ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                            property.SetValue(item, converted);
                        }
                    }

                    lista.Add(item);
                }

                return lista;
            }
            catch (SqlException ex)
            {
                var dbName = sqlConnection?.Database ?? "(desconocida)";
                var cmd = sqlCommand?.CommandText ?? "(vacío)";
                throw new InvalidOperationException($"SqlException en BD='{dbName}'; CommandText='{cmd}': {ex.Message}", ex);
            }
        }
    }
}
