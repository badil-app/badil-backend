using Badil.Backend.Data.Models;
using Badil.Backend.Services.Data;
using Badil.Backend.Services.Tools;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Badil.Backend.Services.Implementation
{
    public class CrowdSourcedProductsService : IProductsService
    {
        private readonly RestClient client = new RestClient("https://world.openfoodfacts.org");
        private readonly BadilContext context;
        private static HashSet<string>? bigBrandNames = null;
        public CrowdSourcedProductsService(BadilContext context)
        {
            this.context = context;
        }

        public async Task<FoodProduct?> GetFoodFactAsync(string barcode)
        {
            var preq = new RestRequest($"api/v0/product/{barcode}.json");
            OpenFoodFactsProductResponse? productData = await client.GetAsync<OpenFoodFactsProductResponse>(preq);
            return productData?.Product;
        }

        public async Task<List<FoodProduct>> GetSimilarProductsAsync(string barcode)
        {
            var fact = await GetFoodFactAsync(barcode);
            if (fact == null) return null!;
            var found = await context.Products.FindAsync(long.Parse(fact?.Id!));
            if (found == null)
            {
                found = fact!.ToProduct(bigBrandNames ?? InitBigBrandNames());
                context.Products.Add(found);
                await context.SaveChangesAsync();
                return [];
            }
            var similarProducts = context.Products
                .Where(x => !x.IsBigNameBrand && x.ProductId != found.ProductId && (
                    x.Products.Any(p => p.ProductId == found.ProductId) || x.AlternativeProducts.Any(p => p.ProductId == found.ProductId)
            )).Select(x => new FoodProduct(x));
            return await similarProducts.ToListAsync();
        }

        public async Task AddBadil(string barcode, string originalProductId)
        {
            using var trans = await context.Database.BeginTransactionAsync();
            var fact = await GetFoodFactAsync(barcode);
            if (fact == null) return;
            if (await context.Products.SingleOrDefaultAsync(x => x.Barcode == barcode) != null) return;
            var originalProduct = await context.Products.FindAsync(long.Parse(originalProductId));
            if (originalProduct == null)
            {
                var originalProductFact = await GetFoodFactAsync(barcode);
                if (originalProductFact == null) return;
                originalProduct = originalProductFact.ToProduct(bigBrandNames ?? InitBigBrandNames());
                context.Products.Add(originalProduct);
                await context.SaveChangesAsync();
            }
            Product newProduct = fact.ToProduct(bigBrandNames ?? InitBigBrandNames());
            context.Products.Add(newProduct);
            await context.SaveChangesAsync();
            await trans.CommitAsync();
        }

        public async Task AddProduct(string barcode)
        {
            var fact = await GetFoodFactAsync(barcode);
            if (fact == null) return;
            if (await context.Products.SingleOrDefaultAsync(x => x.Barcode == barcode) != null) return;
            Product newProduct = fact.ToProduct(bigBrandNames ?? InitBigBrandNames());
            context.Products.Add(newProduct);
            await context.SaveChangesAsync();
        }

        public static HashSet<string> InitBigBrandNames()
        {
            var brandsStr = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "brands.json"));
            var names = JsonSerializer.Deserialize<string[]>(brandsStr);
            bigBrandNames = names!.Select(x => x.NormalizeBrand()).ToHashSet(StringComparer.OrdinalIgnoreCase);
            return bigBrandNames;
        }

    }
}
