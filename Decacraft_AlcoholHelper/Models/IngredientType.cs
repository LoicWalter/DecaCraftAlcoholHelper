using System.Xml.Serialization;

namespace Decacraft_AlcoholHelper.Models;

    public enum IngredientType
    {
        [XmlEnum("Item")]
        Item,

        [XmlEnum("Boisson")]
        Boisson
    }


