using Decacraft_AlcoholHelper.Models;
using System.Net.Http;

namespace Decacraft_AlcoholHelper.Services;

public class ReceipiesService
{
    private Receipies Receipies { get; set; } = new Receipies();
    private readonly HttpClient _httpClient;

    public ReceipiesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Changed: Initialize is now asynchronous and loads the XML via HttpClient (works in Blazor WASM)
    public async Task InitializeAsync()
    {
        var xml = await _httpClient.GetStringAsync("Data/Receipies.xml");
        Receipies = XmlLoaderService.DeserializeXmlFromString<Receipies>(xml) ??
                     throw new InvalidOperationException(
                         "Failed to load Data/Receipies.xml. The file may be missing or corrupted.");
    }

    public Receipy GetReceipyByName(string name)
    {
        return Receipies.ReceipyList.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
               ?? throw new KeyNotFoundException($"Receipy with name '{name}' not found.");
    }

    public List<Receipy> GetAllReceipies()
    {
        return Receipies.ReceipyList;
    }

    /// <summary>
    /// Sub receipies correspond to ingredients that are themselves receipies (boisson). This method will return the full list of ingredients needed for a receipy, including those from sub-receipies if specified.
    /// It takes into account the quantities needed from sub-receipies, and that each receipy produces 3 units.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="desiredQuantity">Number of items you want to produce (not the number of times to do the recipe)</param>
    /// <param name="includeSubReceipies"></param>
    /// <returns>A list of ingredients</returns>
    public List<Ingredient> GetIngredientsForReceipy(string name, int desiredQuantity = 3, bool includeSubReceipies = true)
    {
        var ingredientQuantities = new Dictionary<string, (IngredientType Type, int Quantity)>();

        void AccumulateIngredients(string receipyName, int timesToMake)
        {
            var currentReceipy = GetReceipyByName(receipyName);

            foreach (var ingredient in currentReceipy.IngredientList)
            {
                if (includeSubReceipies && ingredient.Type == IngredientType.Boisson)
                {
                    var neededQuantity = ingredient.Quantity * timesToMake;
                    var subTimesToMake = (int)Math.Ceiling(neededQuantity / 3.0);

                    AccumulateIngredients(ingredient.Name, subTimesToMake);
                }
                else
                {
                    var totalQuantity = ingredient.Quantity * timesToMake;
                    var key = $"{ingredient.Type}:{ingredient.Name}";

                    if (ingredientQuantities.TryGetValue(key, out var value))
                    {
                        ingredientQuantities[key] = (ingredient.Type, value.Quantity + totalQuantity);
                    }
                    else
                    {
                        ingredientQuantities[key] = (ingredient.Type, totalQuantity);
                    }
                }
            }
        }

        var timesToMakeMainRecipe = (int)Math.Ceiling(desiredQuantity / 3.0);
        AccumulateIngredients(name, timesToMakeMainRecipe);

        return ingredientQuantities.Select(kvp => new Ingredient
        {
            Type = kvp.Value.Type,
            Name = kvp.Key.Split(':')[1],
            Quantity = kvp.Value.Quantity
        }).ToList();
    }
}