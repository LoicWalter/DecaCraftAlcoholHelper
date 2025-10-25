using System.Xml.Serialization;

namespace Decacraft_AlcoholHelper.Models;

[XmlRoot("Recipes")]
public class Recipes
{
    [XmlElement("Recipe")] public List<Recipe> RecipeList { get; set; } = [];
}