using Badil.Backend.Data.Models;
using Badil.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Badil.Backend.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController(IProductsService service, BadilContext context) : Controller
    {

        [HttpGet("{barcode}")]
        public async Task<IActionResult> GetProductAsync(string barcode)
        {
            var product = await service.GetFoodFactAsync(barcode);
            if (product == null) return NotFound();
            var found = await context.Products.FindAsync(long.Parse(product?.Id!));
            return Ok(new FrontendProduct()
            {
                Barcode = product!.Id,
                Brand = product.Brands.Split(",").FirstOrDefault() ?? "",
                Img = found?.Url ?? product.ImageThumbUrl,
                NutriScore = product.NutriscoreGrade == "unknown" ? "C" : product.NutriscoreGrade,
                ProductName = product.ProductName,
                Rating = product.Rating
            });
        }
        
        [HttpGet]
        public async Task<IActionResult> GetProductsByBarcodeAsync(string barcode)
        {
            return Ok((await service.GetSimilarProductsAsync(barcode))
                .Select(x => new FrontendProduct()
                {
                    Barcode = x.Id,
                    Brand = x.Brands.Split(",").FirstOrDefault() ?? "",
                    Img = x.ImageThumbUrl,
                    NutriScore = x.NutriscoreGrade == "unknown" ? "C" : x.NutriscoreGrade,
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
