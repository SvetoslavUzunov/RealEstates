using RealEstates.Data;
using RealEstates.Models;

namespace RealEstates.Services;

public class TagService : ITagService
{
    private readonly AppDBContext dbContext;
    private readonly IPropertiesService propertiesService;

    public TagService(AppDBContext dbContext, IPropertiesService propertiesService)
    {
        this.dbContext = dbContext;
        this.propertiesService = propertiesService;
    }

    public void Add(string Name, int? Importance = null)
    {
        var tag = new Tag
        {
            Name = Name,
            Importance = Importance,
        };

        dbContext.Tags.Add(tag);
        dbContext.SaveChanges();
    }

    public void BulkTagToProperties()
    {
        var properties = dbContext.Properties.ToList();

        foreach (var property in properties)
        {
            var averagePricePerSquareMeter = propertiesService.AveragePricePerSquareMeter(property.DistrictId);

            var tag = dbContext.Tags.FirstOrDefault(x => x.Name == "евтин-имот");
            if (property.Price > averagePricePerSquareMeter)
            {
                tag = dbContext.Tags.FirstOrDefault(x => x.Name == "скъп-имот");
            }
            property.Tags.Add(tag);
        }

        dbContext.SaveChanges();
    }
}
