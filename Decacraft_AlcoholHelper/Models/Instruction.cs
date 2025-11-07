using System.Xml.Serialization;
using Decacraft_AlcoholHelper.Services;
using MudBlazor;

namespace Decacraft_AlcoholHelper.Models;

    public class Instruction
    {
        [XmlAttribute("Name")]
        public string Name { get; set; } = string.Empty;
        
        [XmlAttribute("Type")]
        public InstructionType Type { get; set; }

        [XmlAttribute("Time")]
        public int Time { get; set; }
        
        [XmlAttribute("NumberOfCycles")]
        public int NumberOfCycles { get; set; }
        
        public Color GetInstructionColor()
        {
            return Type switch
            {
                InstructionType.Chaudron => Color.Error,
                InstructionType.Alambic => Color.Info,
                InstructionType.Barrel => Color.Warning,
                _ => Color.Default
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
                InstructionType.Alambic => GenerateAlambicDescription(),
                InstructionType.Barrel => GenerateWoodBarrelDescription(),
                _ => string.Empty
            };
        }

        private string GenerateCauldronDescription(string recipeName)
        {
            var recipe = RecipesService.GetRecipeByName(recipeName);
            var description = $"Laisser mijoter {Time} minutes.";

            description += " Ingrédients nécessaires :";
            description = recipe.IngredientList.Aggregate(description, (current, ingredient) => current + $" {ingredient.Name} x{ingredient.Quantity},").TrimEnd(',');
            
            return description;
        }
        
        private string GenerateAlambicDescription()
        {
            return $"Distiller {NumberOfCycles} fois pendant {Time} secondes. Soit {NumberOfCycles * Time} secondes au total.";
        }

        private string GenerateWoodBarrelDescription()
        {
            return $"Laisser vieillir {Time} années en fût {GetTextWithPrefix(Name)} (soit {Time * 20} minutes / {Math.DivRem(Time * 20, 60, out var rem)}h{rem.ToString().PadLeft(2, '0')})";
        }
        
        private string GetTextWithPrefix(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var firstChar = char.ToLower(text[0]);
            return "aeiouyh".Contains(firstChar) ? "d'" + text : "de " + text;
        }
    }

