//using System.Text.Json.Serialization;
//using System.Text.Json;
//using RestSharp;
//using Badil.Backend.Services.Data;
//using Badil.Backend.Services.Tools;

//namespace Badil.Backend.Services.Implementation
//{
//    public class FoodFactProductsService(IOpenAiService openAi) : IProductsService
//    {
//        private readonly RestClient client = new RestClient("https://world.openfoodfacts.org");

//        public async Task<List<FoodProduct>> GetSimilarProductsAsync(string barcode)
//        {
//            // Fetch product by barcode to find its categories or main ingredients
//            var preq = new RestRequest($"api/v0/product/{barcode}.json");
//            OpenFoodFactsProductResponse? productData = await client.GetAsync<OpenFoodFactsProductResponse>(preq);
//            if (productData == null || productData.Product == null) return new List<FoodProduct>();

//            // Extract categories or main ingredients from the product data
//            // This assumes that the product data contains a 'Categories' or 'IngredientsText' field
//            var searchTerms = await openAi.GenerateSearchTerm(productData.Product.Brands, productData.Product.ProductName);
//            if (string.IsNullOrEmpty(searchTerms)) return [];

//            // Prepare search terms by selecting keywords or using the entire category string
//            // Optionally, you might want to process 'searchTerms' to refine the search

//            // Search for similar products using the extracted categories or ingredients
//            HashSet<FoodProduct> products = [];
//            int count = 0;
//            while (products.Count < 10 && count < 10)
//            {
//                var req = new RestRequest($"cgi/search.pl?search_simple=1&action=process&json=true&cc=us&page_size=200");
//                req.Parameters.AddParameter(new QueryParameter("search_terms", searchTerms));
//                var searchResponse = await client.GetAsync<OpenFoodFactsSearchResponse>(req);
//                if (searchResponse?.Products == null) break;
//                for (int i = searchResponse.Products.Count - 1; i > 0; i--)
//                {
//                    if (await IsBigNameBrand(searchResponse.Products[i])) continue;
//                    products.Add(searchResponse.Products[i]);
//                }
//                count++;
//            }
//        }

//        private static HashSet<string>? names = null;
//        private async Task<bool> IsBigNameBrand(FoodProduct product)
//        {
//            if (names == null)
//            {
//                string file = await File.ReadAllTextAsync(Path.Combine(Environment.CurrentDirectory, "brands.json"));
//                names = (JsonSerializer.Deserialize<string[]>(file)!)
//                    .Select(x => x.NormalizeBrand())
//                    .ToHashSet(StringComparer.OrdinalIgnoreCase);
//            }
//            return product.Brands.Split(',').Any(b => names.Contains(b));
//        }

//    }
//}
