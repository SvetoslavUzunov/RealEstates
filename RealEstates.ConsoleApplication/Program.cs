using Microsoft.EntityFrameworkCore;
using RealEstates.Data;
using RealEstates.Services;
using System.Text;

namespace RealEstates.ConsoleApplication;

public class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.Unicode;

        var context = new AppDBContext();
        context.Database.Migrate();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Choose an option.");
            Console.WriteLine("1. Property search");
            Console.WriteLine("2. Most expensive districts");
            Console.WriteLine("3. Average price per square metre in Sofia");
            Console.WriteLine("4. Add Tag");
            Console.WriteLine("5. Bulk tag to properties");
            Console.WriteLine("0. EXIT");

            bool parser = int.TryParse(Console.ReadLine(), out int option);
            if (parser && option == 0)
            {
                break;
            }

            if (parser && (option >= 1 && option <= 5))
            {
                switch (option)
                {
                    case 1:
                        PropertySearch(context);
                        break;
                    case 2:
                        MostExpensiveDistricts(context);
                        break;
                    case 3:
                        AveragePricePerSquareMetre(context);
                        break;
                    case 4:
                        AddTag(context);
                        break;
                    case 5:
                        BulkTagToProperties(context);
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }

    public static void PropertySearch(AppDBContext context)
    {
        Console.Write("Min price: ");
        int minPrice = int.Parse(Console.ReadLine());
        Console.Write("Max price: ");
        int maxPrice = int.Parse(Console.ReadLine());
        Console.Write("Min size: ");
        int minSize = int.Parse(Console.ReadLine());
        Console.Write("Max size: ");
        int maxSize = int.Parse(Console.ReadLine());

        IPropertiesService service = new PropertiesService(context);
        var properties = service.Search(minPrice, maxPrice, minSize, maxSize);

        foreach (var property in properties)
        {
            Console.WriteLine($"{property.DistrictName}; {property.BuildingType}; {property.PropertyType} => {property.Price}€ => {property.Size}m2");
        }
    }

    public static void MostExpensiveDistricts(AppDBContext context)
    {
        Console.Write("Districts count: ");
        int districtCount = int.Parse(Console.ReadLine());

        IDistrictsService service = new DistrictsService(context);
        var districts = service.GetMostExpensiveDistricts(districtCount);

        foreach (var district in districts)
        {
            Console.WriteLine($"{district.Name} => {district.AveragePricePerSquareMeter:f2}€/m2 ({district.PropertiesCount})");
        }
    }

    public static void AveragePricePerSquareMetre(AppDBContext context)
    {
        IPropertiesService service = new PropertiesService(context);
        var averagePrices = service.AveragePricePerSquareMeter();

        Console.WriteLine($"Average price per square metre in Sofia is: {averagePrices:f2}€/m2 ");
    }

    public static void AddTag(AppDBContext context)
    {
        IPropertiesService propertiesService = new PropertiesService(context);
        ITagService service = new TagService(context, propertiesService);
        Console.Write("Tag name: ");
        string tagName = Console.ReadLine();
        Console.Write("Tag importance (optional): ");
        var parser = int.TryParse(Console.ReadLine(), out int tagImportance);
        int? importance = parser ? tagImportance : null;

        service.Add(tagName, importance);
    }

    public static void BulkTagToProperties(AppDBContext context)
    {
        IPropertiesService propertiesService = new PropertiesService(context);
        Console.WriteLine("Bulk starting");
        ITagService service = new TagService(context, propertiesService);
        service.BulkTagToProperties();
        Console.WriteLine("Bulk finished");
    }
}