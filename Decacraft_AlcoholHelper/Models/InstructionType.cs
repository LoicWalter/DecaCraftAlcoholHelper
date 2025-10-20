using System.Xml.Serialization;

namespace Decacraft_AlcoholHelper.Models;

    public enum InstructionType
    {
        [XmlEnum("Chaudron")]
        Chaudron,

        [XmlEnum("Alambic")]
        Alambic,

        [XmlEnum("Barrel")]
        Barrel
    }


