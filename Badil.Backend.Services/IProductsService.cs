namespace Badil.Backend.Services
{
    public interface IProductsService
    {
        Task AddBadil(string barcode, string originalProductId);
        Task AddProduct(string barcode);
        Task<List<FoodProduct>> GetSimilarProductsAsync(string barcode);
    }
}
