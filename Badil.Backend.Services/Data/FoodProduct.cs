using Badil.Backend.Data.Models;
using Badil.Backend.Services.Tools;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Badil.Backend.Services;
public class FoodProduct : IEquatable<FoodProduct?>
{
    [JsonPropertyName("_keywords")]
    public List<string> Keywords { get; set; } = [];

    [JsonPropertyName("product_name")]
    public required string ProductName { get; set; }

    [JsonPropertyName("image_thumb_url")]
    public string ImageThumbUrl { get; set; } = "";

    [JsonPropertyName("nutriments")]
    public NutritionalInformation Nutriments { get; set; } = new();

    [JsonPropertyName("nutriscore_grade")]
    public required string NutriscoreGrade { get; set; }
    [JsonPropertyName("categories")]
    public string? Categories { get; set; }
    [JsonPropertyName("ingredients_text")]
    public string IngredientsText { get; set; } = "";
    [JsonPropertyName("food_groups")]
    public string? FoodGroups { get; set; } = "";
    [JsonPropertyName("brands")]
    public string Brands { get; set; } = "";
    [JsonPropertyName("_id")]
    public required string Id { get; set; }

    public double? Rating { get; set; } = null;

    public override bool Equals(object? obj)
    {
        return Equals(obj as FoodProduct);
    }

    public bool Equals(FoodProduct? other)
    {
        return other is not null &&
               EqualityComparer<List<string>>.Default.Equals(Keywords, other.Keywords) &&
               ProductName == other.ProductName &&
               ImageThumbUrl == other.ImageThumbUrl &&
               EqualityComparer<NutritionalInformation>.Default.Equals(Nutriments, other.Nutriments) &&
               NutriscoreGrade == other.NutriscoreGrade &&
               Categories == other.Categories &&
               IngredientsText == other.IngredientsText &&
               FoodGroups == other.FoodGroups &&
               Brands == other.Brands &&
               Id == other.Id;
    }

    public override int GetHashCode()
    {
        HashCode hash = new();
        hash.Add(Id);
        return hash.ToHashCode();
    }

    public static bool operator ==(FoodProduct? left, FoodProduct? right)
    {
        return EqualityComparer<FoodProduct>.Default.Equals(left, right);
    }

    public static bool operator !=(FoodProduct? left, FoodProduct? right)
    {
        return !(left == right);
    }

    public FoodProduct()
    {

    }

    [SetsRequiredMembers]
    public FoodProduct(Product product)
    {
        this.ProductName = product.Title;
        this.ImageThumbUrl = product.Url;
        this.Brands = product.Brands;
        this.NutriscoreGrade = product.NutriscoreGrade;
        this.Id = product.Barcode;
        this.IngredientsText = product.Ingredients;
        this.Rating = product.UserReviews.Average(x => x.Score) ?? null;
    }

    public Product ToProduct(HashSet<string> bigNameBrands)
    {
        return new()
        {
            Barcode = Id,
            ProductId = long.Parse(Id),
            IsBigNameBrand = this.Brands.Split(',').Any(x => bigNameBrands.Contains(x.NormalizeBrand())),
            Brands = Brands,
            Title = ProductName,
            NutriscoreGrade = NutriscoreGrade,
            Url = ImageThumbUrl,
            Ingredients = IngredientsText,
        };
    }

}

public class NutritionalInformation
{
    [JsonPropertyName("energy")]
    public double? Energy { get; set; }

    [JsonPropertyName("fat")]
    public double? Fat { get; set; }

    [JsonPropertyName("saturated-fat")]
    public double? SaturatedFat { get; set; }

    [JsonPropertyName("carbohydrates")]
    public double? Carbohydrates { get; set; }

    [JsonPropertyName("sugars")]
    public double? Sugars { get; set; }

    [JsonPropertyName("fiber")]
    public double? Fiber { get; set; }

    [JsonPropertyName("proteins")]
    public double? Proteins { get; set; }

    [JsonPropertyName("salt")]
    public double? Salt { get; set; }
}


