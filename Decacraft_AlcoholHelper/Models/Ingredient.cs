using System.Xml.Serialization;
using MudBlazor;

namespace Decacraft_AlcoholHelper.Models;

    public class Ingredient
    {
        [XmlAttribute("Type")]
        public IngredientType Type { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("StackSize")]
        public int StackSize { get; set; }
        
        
        [XmlAttribute("Quantity")]
        public int Quantity { get; set; }
        
        public string GetStackText()
        {
            if (StackSize == 1)
                return "";
            
            var stacks = Math.DivRem(Quantity, StackSize, out var rem);
            var isStackPlural = stacks > 1;
            var isRemPlural = rem > 1;
            return rem == 0 ? $"({stacks} stack{(isStackPlural ? "s" : "")})" : $"({stacks} stack{(isStackPlural ? "s" : "")} et {rem} unité{(isRemPlural ? "s" : "")})";
        }
        
        public string GetIngredientIcon()
        {
            return Type switch
            {
                IngredientType.Item => Icons.Material.Filled.Inventory,
                IngredientType.Boisson => Icons.Material.Filled.LocalBar,
                _ => Icons.Material.Filled.Help
            };
        }
    }

