namespace Test_Store_ADONET.Entities
{
    public class Product
    {
        public Guid IdProduct { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid IdBrand { get; set; }
        public string BrandName { get; set; }
    }
}
