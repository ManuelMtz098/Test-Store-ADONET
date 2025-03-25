using Test_Store_ADONET.DTOs;
using Test_Store_ADONET.Entities;

namespace Test_Store_ADONET.Services.Products
{
    public interface IProductsService
    {
        Task<List<Product>> GetProducts();
        Task<Product> GetProductById(Guid idProduct);
        Task<Product> CreateProduct(CreateProductDTO createBrand);
        Task UpdateProduct(Guid idProduct, CreateProductDTO updateProduct);
        Task deleteProduct(Guid idProduct);
    }
}
