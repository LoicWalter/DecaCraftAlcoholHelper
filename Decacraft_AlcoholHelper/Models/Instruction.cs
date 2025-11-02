using System.Xml.Serialization;
using Decacraft_AlcoholHelper.Services;
using MudBlazor;

namespace Decacraft_AlcoholHelper.Models;

    public class Instruction
    {
        [XmlElement("Name")]
        public string Name { get; set; } = string.Empty;
        
        [XmlElement("Type")]
        public InstructionType Type { get; set; }

        [XmlElement("Temps")]
        public int Temps { get; set; }
        
        public Color GetInstructionColor()
        {
            return Type switch
            {
                InstructionType.Chaudron => Color.Error,
                InstructionType.Alambic => Color.Info,
                _ => Color.Warning
            };
        }
        
        public string GetInstructionTitle()
        {
            return Type switch
            {
                InstructionType.Chaudron => "Chaudron",
                InstructionType.Alambic => "Alambic",
                InstructionType.Barrel => "Fût en " + Name,
                _ => Type.ToString()
            };
        }
        
        public string GetInstructionDescription(string recipeName)
        {
            return Type switch
            {
                InstructionType.Chaudron => GenerateCauldronDescription(recipeName),
                InstructionType.Alambic => $"Distiller pendant {Temps} secondes. Soit {Temps / 15} cycles de distillation.",
                _ => GenerateWoodBarrelDescription()
            };
        }

        private string GenerateCauldronDescription(string recipeName)
        {
            var recipe = RecipesService.GetRecipeByName(recipeName);
            var description = $"Laisser mijoter {Temps} minutes.";

            description += " Ingrédients nécessaires :";
            foreach (var ingredient in recipe.IngredientList)
            {
                description += $" {ingredient.Name} x{ingredient.Quantity},";
            }
            description = description.TrimEnd(',');
            
            return description;
        }

        private string GenerateWoodBarrelDescription()
        {
            return $"Laisser vieillir {Temps} années en fût {GetTextWithPrefix(Name)} (soit {Temps * 20} minutes / {Math.DivRem(Temps * 20, 60, out var rem)}h{rem.ToString().PadLeft(2, '0')})";
        }
        
        private string GetTextWithPrefix(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var firstChar = char.ToLower(text[0]);
            return "aeiouyh".Contains(firstChar) ? "d'" + text : "de " + text;
        }
    }

