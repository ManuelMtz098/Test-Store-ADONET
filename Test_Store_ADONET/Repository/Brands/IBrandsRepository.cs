using System.Data;
using Test_Store_ADONET.DTOs;

namespace Test_Store_ADONET.Repository.Brands
{
    public interface IBrandsRepository
    {
        Task<DataTable> GetBrands();
        Task<DataRow> GetBrandById(Guid idBrand);
        Task UpdateBrand(Guid idBrand, CreateBrandDTO updateBrand);
        Task CreateBrand(Guid idBrand, CreateBrandDTO createBrand);
        Task DeleteBrand(Guid idBrand);
    }
}
