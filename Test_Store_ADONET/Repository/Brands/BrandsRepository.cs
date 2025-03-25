using Microsoft.Data.SqlClient;
using System.Data;
using Test_Store_ADONET.Database;
using Test_Store_ADONET.DTOs;

namespace Test_Store_ADONET.Repository.Brands
{
    public class BrandsRepository : IBrandsRepository
    {
        private readonly IDatabaseConnection _databaseConnection;
        public BrandsRepository(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task<DataTable> GetBrands()
        {
            SqlParameter[] parameters = {};

            return await _databaseConnection.ExecuteQuery("usp_GetBrands", parameters);
        }

        public async Task<DataRow> GetBrandById(Guid idBrand)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@IdBrand", SqlDbType.UniqueIdentifier) { Value = idBrand },
            };

            return await _databaseConnection.ExecuteSingleRowQuery("usp_GetBrandById", parameters);
        }

        public async Task CreateBrand(Guid idBrand, CreateBrandDTO createBrand)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@IdBrand", SqlDbType.UniqueIdentifier) { Value = idBrand },
                new SqlParameter("@Name", SqlDbType.VarChar) { Value = createBrand.Name },
            };

            await _databaseConnection.ExecuteNonQuery("usp_CreateBrand", parameters);
        }

        public async Task UpdateBrand(Guid idBrand, CreateBrandDTO updateBrand)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@IdBrand", SqlDbType.UniqueIdentifier) { Value = idBrand },
                new SqlParameter("@Name", SqlDbType.VarChar) { Value = updateBrand.Name },
            };

            await _databaseConnection.ExecuteNonQuery("usp_UpdateBrand", parameters);
        }

        public async Task DeleteBrand(Guid idBrand)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@IdBrand", SqlDbType.UniqueIdentifier) { Value = idBrand },
            };

            await _databaseConnection.ExecuteNonQuery("usp_DeleteBrand", parameters);
        }
    }
}
