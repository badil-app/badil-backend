namespace Badil.Backend.Data
{
    public class FrontendProduct
    {
        public required string Name { get; set; }
        public required string Barcode { get; set; }
        public required string ImageUrl { get; set; }
        public required string NutriscoreGrade { get; set; }
        public required long Id { get; set; }
        public required double Rating { get; set; }
    }
}
