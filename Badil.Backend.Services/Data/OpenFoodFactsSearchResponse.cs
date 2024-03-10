using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Badil.Backend.Services.Data
{
    public class OpenFoodFactsSearchResponse
    {
        [JsonPropertyName("products")]
        public List<FoodProduct> Products { get; set; } = [];
    }

    public class OpenFoodFactsProductResponse
    {
        [JsonPropertyName("product")]
        public required FoodProduct Product { get; set; }
    }

}
