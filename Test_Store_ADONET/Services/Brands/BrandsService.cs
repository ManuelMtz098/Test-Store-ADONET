using System.Data;
using Test_Store_ADONET.DTOs;
using Test_Store_ADONET.Entities;
using Test_Store_ADONET.Exceptions;
using Test_Store_ADONET.Mappers;
using Test_Store_ADONET.Repository.Brands;

namespace Test_Store_ADONET.Services.Brands
{
    public class BrandsService : IBrandsService
    {
        private readonly IBrandsRepository _brandsRepository;

        public BrandsService(IBrandsRepository brandsRepository)
        {
            _brandsRepository = brandsRepository;
        }

        public async Task<List<Brand>> GetBrands()
        {
            DataTable dtBrands = await _brandsRepository.GetBrands();
            return BrandMapper.MapToBrands(dtBrands);
        }

        public async Task<Brand> GetBrandById(Guid idBrand)
        {
            DataRow drBrand = await _brandsRepository.GetBrandById(idBrand);

            if (drBrand == null)
                throw new NotFoundException("Brand not found.");

            return BrandMapper.MapToBrand(drBrand);
        }

        public async Task<Brand> CreateBrand(CreateBrandDTO createBrand)
        {
            Guid idBrand = Guid.NewGuid();

            await _brandsRepository.CreateBrand(idBrand, createBrand);

            return new Brand
            {
                IdBrand = idBrand,
                Name = createBrand.Name
            };
        }

        public async Task UpdateBrand(Guid idBrand, CreateBrandDTO updateBrand)
        {
            await GetBrandById(idBrand); // Check if brand exists
            await _brandsRepository.UpdateBrand(idBrand, updateBrand);
        }

        public async Task deleteBrand(Guid idBrand)
        {
            await GetBrandById(idBrand); // Check if brand exists
            await _brandsRepository.DeleteBrand(idBrand);
        }
    }
}