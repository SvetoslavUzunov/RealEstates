using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.Models;

namespace RealEstates.Services;

public class PropertiesService : IPropertiesService
{
    private readonly AppDBContext dbContext;

    public PropertiesService(AppDBContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Add(int size, int? yardSize, byte? floor, byte? totalFloor, int? year, int? price, string district, string propertyType, string buildingType)
    {
        var property = new Property
        {
            Size = size,
            Price = price <= 0 ? null : price,
            Floor = floor <= 0 || floor > 255 ? null : floor,
            TotalFloors = totalFloor <= 0 || totalFloor > 255 ? null : totalFloor,
            YardSize = yardSize <= 0 ? null : yardSize,
            Year = year <= 1800 ? null : year,
        };

        var dbDistrict = dbContext.Districts.FirstOrDefault(x => x.Name == district);
        if (dbDistrict == null)
        {
            dbDistrict = new District { Name = district };
        }
        property.District = dbDistrict;

        var dbBuildingType = dbContext.Buildings.FirstOrDefault(x => x.Name == buildingType);
        if (dbBuildingType == null)
        {
            dbBuildingType = new BuildingType { Name = buildingType };
        }
        property.BuildingType = dbBuildingType;

        var dbPropertyType = dbContext.PropertyTypes.FirstOrDefault(x => x.Name == propertyType);
        if (dbPropertyType == null)
        {
            dbPropertyType = new PropertyType { Name = propertyType };
        }
        property.Type = dbPropertyType;

        dbContext.Properties.Add(property);
        dbContext.SaveChanges();
    }

    public decimal AveragePricePerSquareMeter()
    {
        var averagePrice = dbContext.Properties
            .Where(x => x.Price.HasValue)
            .Average(x => x.Price / (decimal)x.Size) ?? 0;

        return averagePrice;
    }

    public decimal AveragePricePerSquareMeter(int districtId)
    {
        var averagePrice = dbContext.Properties
            .Where(x => x.DistrictId == districtId)
            .Average(x => x.Price / (decimal)x.Size) ?? 0;

        return averagePrice;
    }

    public IEnumerable<PropertyInfoDto> Search(int minPrice, int maxPrice, int minSize, int maxSize)
    {
        var properties = dbContext.Properties
            .Where(x => x.Price >= minPrice && x.Price <= maxPrice && x.Size >= minSize && x.Size <= maxSize)
            .Select(x => new PropertyInfoDto
            {
                Size = x.Size,
                Price = x.Price ?? 0,
                BuildingType = x.BuildingType.Name,
                DistrictName = x.District.Name,
                PropertyType = x.Type.Name,
            })
            .ToList();

        return properties;
    }
}