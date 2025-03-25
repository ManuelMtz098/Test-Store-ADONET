using System.Data;
using Test_Store_ADONET.Entities;

namespace Test_Store_ADONET.Mappers
{
    public static class BrandMapper
    {
        public static List<Brand> MapToBrands(DataTable dtBrands)
        {
            return dtBrands.AsEnumerable().Select(MapToBrand).ToList();
        }

        public static Brand MapToBrand(DataRow dr)
        {
            return new Brand
            {
                IdBrand = dr.Field<Guid>("IdBrand"),
                Name = dr.Field<string>("Name"),
            };
        }
    }
}
