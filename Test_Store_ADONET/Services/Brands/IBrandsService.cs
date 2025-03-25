using Test_Store_ADONET.DTOs;
using Test_Store_ADONET.Entities;

namespace Test_Store_ADONET.Services.Brands
{
    public interface IBrandsService
    {
        Task<List<Brand>> GetBrands();
        Task<Brand> GetBrandById(Guid idBrand);
        Task<Brand> CreateBrand(CreateBrandDTO createBrand);
        Task UpdateBrand(Guid idBrand, CreateBrandDTO updateBrand);
        Task deleteBrand(Guid idBrand);
    }
}
