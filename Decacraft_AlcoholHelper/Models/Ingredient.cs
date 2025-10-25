﻿using System.Xml.Serialization;
using MudBlazor;

namespace Decacraft_AlcoholHelper.Models;

    public class Ingredient
    {
        [XmlElement("Type")]
        public IngredientType Type { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("StackSize")]
        public int StackSize { get; set; }
        
        
        [XmlElement("Quantity")]
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
            return Type == IngredientType.Item
                ? Icons.Material.Filled.Inventory
                : Icons.Material.Filled.LocalBar;
        }
    }

