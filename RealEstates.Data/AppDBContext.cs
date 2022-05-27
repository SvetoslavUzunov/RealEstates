using Microsoft.EntityFrameworkCore;
using RealEstates.Models;

namespace RealEstates.Data;

public class AppDBContext : DbContext
{
    public AppDBContext() { }

    public AppDBContext(DbContextOptions options) : base(options) { }

    public DbSet<Property> Properties { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<PropertyType> PropertyTypes { get; set; }
    public DbSet<BuildingType> Buildings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Server=.\MSSQLSERVER01;Database=RealEstates;Integrated Security=true;Encrypt=false");
        }
    }
}