using Microsoft.Data.SqlClient;
using System.Data;
using Test_Store_ADONET.Database;
using Test_Store_ADONET.Entities;

namespace Test_Store_ADONET.Repository.AppUser
{
    public class AppUsersRepository : IAppUsersRepository
    {
        private readonly IDatabaseConnection _databaseConnection;
        public AppUsersRepository(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        public async Task<DataRow> GetUserByUsername(string username)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@Username", SqlDbType.VarChar) { Value = username },
            };

            return await _databaseConnection.ExecuteSingleRowQuery("usp_GetUserByUsername", parameters);
        }
    }
}