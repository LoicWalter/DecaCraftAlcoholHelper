using System.Xml.Serialization;

namespace Decacraft_AlcoholHelper.Models;

[XmlRoot("Receipies")]
public class Receipies
{
    [XmlElement("Receipy")] public List<Receipy> ReceipyList { get; set; } = new();
}