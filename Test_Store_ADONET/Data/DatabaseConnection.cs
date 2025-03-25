using Microsoft.Data.SqlClient;
using System.Data;

namespace Test_Store_ADONET.Database
{
    public interface IDatabaseConnection
    {
        Task<DataTable> ExecuteQuery(string query, SqlParameter[] parameters = null);
        Task<DataRow> ExecuteSingleRowQuery(string query, SqlParameter[] parameters = null);
        Task<int> ExecuteNonQuery(string query, SqlParameter[] parameters);
    }

    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly IConfiguration _config;

        public DatabaseConnection(IConfiguration config)
        {
            _config = config;
        }

        private string? _connectionString;
        private string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    _connectionString = _config.GetConnectionString("DbConnection") ?? string.Empty;
                }
                return _connectionString;
            }
        }

        public async Task<DataTable> ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable result = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmmd.Parameters.AddRange(parameters);
                    }
                    cmmd.CommandType = CommandType.StoredProcedure;

                    await conn.OpenAsync();

                    using (SqlDataReader sqlDataReader = await cmmd.ExecuteReaderAsync())
                    {
                        result.Load(sqlDataReader);
                    }
                }
            }

            return result;
        }

        public async Task<DataRow> ExecuteSingleRowQuery(string query, SqlParameter[] parameters = null)
        {
            DataRow result = null;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmmd.Parameters.AddRange(parameters);
                    }
                    cmmd.CommandType = CommandType.StoredProcedure;

                    await conn.OpenAsync();

                    using (SqlDataReader sqlDataReader = await cmmd.ExecuteReaderAsync())
                    {
                        if (await sqlDataReader.ReadAsync())
                        {
                            DataTable dt = new DataTable();
                            for (int i = 0; i < sqlDataReader.FieldCount; i++)
                            {
                                dt.Columns.Add(sqlDataReader.GetName(i), sqlDataReader.GetFieldType(i));
                            }

                            DataRow row = dt.NewRow();
                            for (int i = 0; i < sqlDataReader.FieldCount; i++)
                            {
                                row[i] = sqlDataReader[i];
                            }
                            dt.Rows.Add(row);

                            result = dt.Rows[0];
                        }
                    }
                }
            }

            return result;
        }


        public async Task<int> ExecuteNonQuery(string query, SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmmd = new SqlCommand(query, conn))
                {
                    cmmd.Parameters.AddRange(parameters);
                    cmmd.CommandType = CommandType.StoredProcedure;

                    await conn.OpenAsync();
                    return await cmmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
