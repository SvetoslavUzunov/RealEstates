namespace RealEstates.Services;

public interface ITagService
{
    void Add(string Name, int? Importance = null);
    void BulkTagToProperties();
}