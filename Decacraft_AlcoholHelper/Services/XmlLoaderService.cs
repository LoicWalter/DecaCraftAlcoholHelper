using System.Xml.Serialization;

namespace Decacraft_AlcoholHelper.Services;

public static class XmlLoaderService
{
    // Added: helper to deserialize directly from an XML string. Useful for Blazor WebAssembly where
    // files are fetched via HttpClient rather than accessed from the server file system.
    public static T? DeserializeXmlFromString<T>(string xml) where T : class
    {
        try
        {
            var serializer = new XmlSerializer(typeof(T));
            using var reader = new System.IO.StringReader(xml);
            return (T?)serializer.Deserialize(reader);
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine("Erreur de désérialisation XML depuis string : " + ex.Message);
            if (ex.InnerException != null) Console.Error.WriteLine("Inner: " + ex.InnerException.Message);
            return null;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Erreur inattendue lors du chargement depuis string : " + ex);
            return null;
        }
    }
}