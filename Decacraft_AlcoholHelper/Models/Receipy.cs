using System.Xml.Serialization;

namespace Decacraft_AlcoholHelper.Models;

public class Receipy
{
    [XmlElement("Name")] public string Name { get; set; } = string.Empty;

    [XmlArray("Ingredients")]
    [XmlArrayItem("Ingredient")]
    public List<Ingredient> IngredientList { get; set; } = new List<Ingredient>();

    [XmlArray("Instructions")]
    [XmlArrayItem("Instruction")]
    public List<Instruction> InstructionList { get; set; } = new List<Instruction>();
}