using System.Data;
using Test_Store_ADONET.Entities;

namespace Test_Store_ADONET.Mappers
{
    public static class ProductMapper
    {
        public static List<Product> MapToProducts(DataTable dtProducts)
        {
            return dtProducts.AsEnumerable().Select(MapToProduct).ToList();
        }

        public static Product MapToProduct(DataRow dr)
        {
            return new Product
            {
                IdProduct = dr.Field<Guid>("IdProduct"),
                Name = dr.Field<string>("Name"),
                Description = dr.Field<string>("Description"),
                IdBrand = dr.Field<Guid>("IdBrand"),
                BrandName = dr.Field<string>("BrandName"),
            };
        }
    }
}
