using RealEstates.Data;
using RealEstates.Services.Models;

namespace RealEstates.Services;

public class DistrictsService : IDistrictsService
{
    private readonly AppDBContext dbContext;

    public DistrictsService(AppDBContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IEnumerable<DistrictInfoDto> GetMostExpensiveDistricts(int count)
    {
        var districts = dbContext.Districts
            .Select(x => new DistrictInfoDto
            {
                Name = x.Name,
                AveragePricePerSquareMeter = x.Properties.Where(p => p.Price.HasValue).Average(p => p.Price / (decimal)p.Size) ?? 0,
                PropertiesCount = x.Properties.Count(),
            })
            .OrderByDescending(x => x.AveragePricePerSquareMeter)
            .Take(count)
            .ToList();

        return districts;
    }
}