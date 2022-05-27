using RealEstates.Services.Models;

namespace RealEstates.Services;

public interface IPropertiesService
{
    void Add(int size, int? yardSize, byte? floor, byte? totalFloor, int? year, int? price, string district, string propertyType, string buildingType);

    IEnumerable<PropertyInfoDto> Search(int minPrice, int maxPrice, int minSize, int maxSize);

    decimal AveragePricePerSquareMeter();
    decimal AveragePricePerSquareMeter(int districtId);
}