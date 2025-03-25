using System.Data;
using Test_Store_ADONET.DTOs;
using Test_Store_ADONET.Entities;
using Test_Store_ADONET.Exceptions;
using Test_Store_ADONET.Mappers;
using Test_Store_ADONET.Repository.Brands;
using Test_Store_ADONET.Repository.Products;
using Test_Store_ADONET.Services.Brands;

namespace Test_Store_ADONET.Services.Products
{
    public class ProductsService: IProductsService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IBrandsService _brandsService;

        public ProductsService(IProductsRepository productsRepository, IBrandsService brandsService)
        {
            _productsRepository = productsRepository;
            _brandsService = brandsService;
        }

        public async Task<List<Product>> GetProducts()
        {
            DataTable dtProducts = await _productsRepository.GetProducts();
            return ProductMapper.MapToProducts(dtProducts);
        }

        public async Task<Product> GetProductById(Guid idProduct)
        {
            DataRow drProduct = await _productsRepository.GetProductById(idProduct);

            if (drProduct == null)
                throw new NotFoundException("Product not found.");

            return ProductMapper.MapToProduct(drProduct);
        }

        public async Task<Product> CreateProduct(CreateProductDTO createProduct)
        {
            Brand brand = await _brandsService.GetBrandById(createProduct.IdBrand);

            Guid idProduct = Guid.NewGuid();

            await _productsRepository.CreateProduct(idProduct, createProduct);

            return new Product
            {
                IdProduct = idProduct,
                Name = createProduct.Name,
                Description = createProduct.Description,
                IdBrand = createProduct.IdBrand,
                BrandName = brand.Name
            };
        }

        public async Task UpdateProduct(Guid idProduct, CreateProductDTO updateProduct)
        {
            await GetProductById(idProduct); // Check if product exists
            await _productsRepository.UpdateProduct(idProduct, updateProduct);
        }

        public async Task deleteProduct(Guid idProduct)
        {
            await GetProductById(idProduct); // Check if product exists
            await _productsRepository.DeleteProduct(idProduct);
        }
    }
}
