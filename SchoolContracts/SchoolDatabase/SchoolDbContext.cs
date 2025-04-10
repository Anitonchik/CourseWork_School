using Microsoft.EntityFrameworkCore;
using SchoolContracts;
using SchoolDatabase.Models;
using System;

namespace SchoolDatabase;

public class SchoolDbContext : DbContext
{
    private readonly IConfigurationDatabase? _configurationDatabase;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Storekeeper>().HasIndex(x => x.Login).IsUnique();
    }

    public DbSet<Circle> Circles { get; set; }
    public DbSet<CircleMaterial> CircleMaterials { get; set; }
    public DbSet<InterestMaterial> InterestMaterials { get; set; }
    public DbSet<LessonCircle> LessonCircles { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Medal> Medals { get; set; }
    public DbSet<Storekeeper> Storekeepers { get; set; }
}
