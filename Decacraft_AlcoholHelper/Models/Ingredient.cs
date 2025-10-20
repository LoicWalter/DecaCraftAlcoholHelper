using System.Xml.Serialization;

namespace Decacraft_AlcoholHelper.Models;

    public class Ingredient
    {
        [XmlElement("Type")]
        public IngredientType Type { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("Quantity")]
        public int Quantity { get; set; }
    }

