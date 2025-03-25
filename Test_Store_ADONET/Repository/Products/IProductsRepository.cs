using System.Data;
using Test_Store_ADONET.DTOs;

namespace Test_Store_ADONET.Repository.Products
{
    public interface IProductsRepository
    {
        Task<DataTable> GetProducts();
        Task<DataRow> GetProductById(Guid idProduct);
        Task UpdateProduct(Guid idProduct, CreateProductDTO updateProduct);
        Task CreateProduct(Guid idProduct, CreateProductDTO createProduct);
        Task DeleteProduct(Guid idProduct);
    }
}
