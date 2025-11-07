using System.Xml.Serialization;
using Decacraft_AlcoholHelper.Services;

namespace Decacraft_AlcoholHelper.Models;

public class Recipe
{
    [XmlAttribute("Id")] public int Id { get; set; }
    [XmlAttribute("Name")] public string Name { get; set; } = string.Empty;
    [XmlAttribute("Value")] public int Value { get; set; }

    [XmlArray("Ingredients")]
    [XmlArrayItem("Ingredient")]
    public List<Ingredient> IngredientList { get; set; } = [];

    [XmlArray("Instructions")]
    [XmlArrayItem("Instruction")]
    public List<Instruction> InstructionList { get; set; } = [];

    public Dictionary<string, List<Instruction>> GetInstructionsByRecipeName(bool includeSubRecipes = true)
    {
        var allInstructions = new Dictionary<string, List<Instruction>>();
        if (!includeSubRecipes)
        {
            allInstructions[Name] = InstructionList;
            return allInstructions;
        }
        
        if (IngredientList.Any(ing => ing.Type == IngredientType.Boisson))
        {
            foreach (var ingredient in IngredientList.Where(ing => ing.Type == IngredientType.Boisson))
            {
                var subRecipes = RecipesService.GetAllRecipes().FindAll(r => r.Name == ingredient.Name);
                foreach (var subRecipe in subRecipes)
                {
                    allInstructions[subRecipe.Name] = subRecipe.GetInstructionsByRecipeName(includeSubRecipes).SelectMany(kv => kv.Value).ToList();
                }
            }
        }
        allInstructions[Name] = InstructionList;
        return allInstructions;
    }
}