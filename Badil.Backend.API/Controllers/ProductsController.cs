using Badil.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Badil.Backend.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController(IProductsService service) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetProductsByBarcodeAsync(string barcode)
        {
            return Ok((await service.GetSimilarProductsAsync(barcode))
                .Select(x => new FrontendProduct()
                {
                    Barcode = x.Id,
                    Brand = x.Brands.Split(",").FirstOrDefault() ?? "",
                    Img = x.ImageThumbUrl,
                    NutriScore = x.NutriscoreGrade,
                    ProductName = x.ProductName,
                    Rating = x.Rating
                }));
        }

        [HttpPost("{originalBarcode}/Badils")]
        public async Task<IActionResult> AddBadilAsync(string originalBarcode, string barcode)
        {
            await service.AddBadil(barcode, originalBarcode);
            return Ok();
        }

        [HttpPost("{originalBarcode}")]
        public async Task<IActionResult> AddProduct(string originalBarcode)
        {
            await service.AddProduct(originalBarcode);
            return Ok();
        }


    }

    class FrontendProduct
    {
        public string Img { get; set; } = "";
        public required string ProductName { get; set; }
        public required string Brand { get; set; }
        public double? Rating { get; set; }
        public required string NutriScore { get; set; }
        public required string Barcode { get; set; }
    }

}
