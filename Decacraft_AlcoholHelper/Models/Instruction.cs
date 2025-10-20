using System.Xml.Serialization;

namespace Decacraft_AlcoholHelper.Models;

    public class Instruction
    {
        [XmlElement("Name")]
        public string Name { get; set; } = string.Empty;
        
        [XmlElement("Type")]
        public InstructionType Type { get; set; }

        [XmlElement("Temps")]
        public int Temps { get; set; }
    }

