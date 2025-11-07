using Decacraft_AlcoholHelper.Models;
using System.Net.Http;

namespace Decacraft_AlcoholHelper.Services;

// Service to manage recipes loaded from an XML file.
public class RecipesService(HttpClient httpClient)
{
    private static Recipes? _recipes;
    private static Recipes Recipes
    {
        get => _recipes ?? throw new InvalidOperationException("RecipesService not initialized. Call InitializeAsync() first.");
        set => _recipes = value;
    }

    public async Task InitializeAsync()
    {
        if (_recipes != null)
            return;
        
        var xml = await httpClient.GetStringAsync("Data/Recipes.xml");
        Recipes = XmlLoaderService.DeserializeXmlFromString<Recipes>(xml) ??
                     throw new InvalidOperationException(
                         "Failed to load Data/Recipes.xml. The file may be missing or corrupted.");
    }

    public static Recipe GetRecipeByName(string name)
    {
        return Recipes.RecipeList.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
               ?? throw new KeyNotFoundException($"Recipe with name '{name}' not found.");
    }

    public static List<Recipe> GetAllRecipes()
    {
        return Recipes.RecipeList.OrderBy(x => x.Id).ToList();
    }

    /// <summary>
    /// Sub recipes correspond to ingredients that are themselves recipes (boisson). This method will return the full list of ingredients needed for a recipe, including those from sub-recipes if specified.
    /// It takes into account the quantities needed from sub-recipes, and that each recipe produces 3 units.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="desiredQuantity">Number of items you want to produce (not the number of times to do the recipe)</param>
    /// <param name="includeSubRecipes"></param>
    /// <returns>A list of ingredients</returns>
    public static List<Ingredient> GetIngredientsForRecipe(string name, int desiredQuantity = 3, bool includeSubRecipes = true)
    {
        var ingredientQuantities = new Dictionary<string, (IngredientType Type, int Quantity, int StackSize)>();

        var timesToMakeMainRecipe = (int)Math.Ceiling(desiredQuantity / 3.0);
        AccumulateIngredients(name, timesToMakeMainRecipe);

        return ingredientQuantities.Select(kvp => new Ingredient
        {
            Type = kvp.Value.Type,
            Name = kvp.Key.Split(':')[1],
            Quantity = kvp.Value.Quantity,
            StackSize = kvp.Value.StackSize
        }).ToList();

        void AccumulateIngredients(string recipeName, int timesToMake)
        {
            var currentRecipe = GetRecipeByName(recipeName);

            foreach (var ingredient in currentRecipe.IngredientList)
            {
                if (includeSubRecipes && ingredient.Type == IngredientType.Boisson)
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
                        ingredientQuantities[key] = (ingredient.Type, value.Quantity + totalQuantity, ingredient.StackSize);
                    }
                    else
                    {
                        ingredientQuantities[key] = (ingredient.Type, totalQuantity, ingredient.StackSize);
                    }
                }
            }
        }
    }

    public static bool TryGetRecipeByName(string selectedRecipeName, out Recipe o)
    {
        try
        {
            o = GetRecipeByName(selectedRecipeName);
            return true;
        }
        catch (KeyNotFoundException)
        {
            o = null!;
            return false;
        }
    }
}