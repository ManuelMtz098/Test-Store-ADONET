using Microsoft.Data.SqlClient;
using System.Data;
using Test_Store_ADONET.Database;
using Test_Store_ADONET.DTOs;

namespace Test_Store_ADONET.Repository.Products
{
    public class ProductsRepository: IProductsRepository
    {
        private readonly IDatabaseConnection _databaseConnection;
        public ProductsRepository(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task<DataTable> GetProducts()
        {
            SqlParameter[] parameters = { };

            return await _databaseConnection.ExecuteQuery("usp_GetProducts", parameters);
        }

        public async Task<DataRow> GetProductById(Guid idProduct)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@IdProduct", SqlDbType.UniqueIdentifier) { Value = idProduct },
            };

            return await _databaseConnection.ExecuteSingleRowQuery("usp_GetProductById", parameters);
        }

        public async Task CreateProduct(Guid idProduct, CreateProductDTO createProduct)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@IdProduct", SqlDbType.UniqueIdentifier) { Value = idProduct },
                new SqlParameter("@Name", SqlDbType.VarChar) { Value = createProduct.Name },
                new SqlParameter("@Description", SqlDbType.VarChar) { Value = createProduct.Description },
                new SqlParameter("@IdBrand", SqlDbType.UniqueIdentifier) { Value = createProduct.IdBrand },
            };

            await _databaseConnection.ExecuteNonQuery("usp_CreateProduct", parameters);
        }

        public async Task UpdateProduct(Guid idProduct, CreateProductDTO updateProduct)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@IdProduct", SqlDbType.UniqueIdentifier) { Value = idProduct },
                new SqlParameter("@Name", SqlDbType.VarChar) { Value = updateProduct.Name },
                new SqlParameter("@Description", SqlDbType.VarChar) { Value = updateProduct.Description },
                new SqlParameter("@IdBrand", SqlDbType.UniqueIdentifier) { Value = updateProduct.IdBrand },
            };

            await _databaseConnection.ExecuteNonQuery("usp_UpdateProduct", parameters);
        }

        public async Task DeleteProduct(Guid idProduct)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@IdProduct", SqlDbType.UniqueIdentifier) { Value = idProduct },
            };

            await _databaseConnection.ExecuteNonQuery("usp_DeleteProduct", parameters);
        }
    }
}
