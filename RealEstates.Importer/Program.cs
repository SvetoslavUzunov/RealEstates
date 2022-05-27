using RealEstates.Data;
using RealEstates.Services;
using System.Text.Json;

namespace RealEstates.Importer;

public class Program
{
    static void Main()
    {
        ImportJsonFile("imot.bg-houses-Sofia-raw-data-2021-03-18.json");
        Console.WriteLine();
        ImportJsonFile("imot.bg-raw-data-2021-03-18.json");
    }

    private static void ImportJsonFile(string fileName)
    {
        var context = new AppDBContext();
        IPropertiesService propertiesService = new PropertiesService(context);

        var properties = JsonSerializer.Deserialize<IEnumerable<PropertyAsJson>>(File.ReadAllText(fileName));

        foreach (var property in properties)
        {
            propertiesService.Add(property.Size, property.YardSize, (byte)property.Floor, (byte)property.TotalFloor, property.Year, property.Price, property.District, property.Type, property.BuildingType);
            Console.Write(".");
        }
    }
}