using Microsoft.EntityFrameworkCore;
using SchoolContracts;
using SchoolDatabase.Models;
using SchoolDatabase.Models.ModelsForReports;

namespace SchoolDatabase;

public class SchoolDbContext : DbContext
{
    private readonly IConnectionString? _connectionString;

    public SchoolDbContext(IConnectionString connectionString)
    {
        _connectionString = connectionString;
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString?.ConnectionString, o => o.SetPostgresVersion(12, 2));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Storekeeper>().HasIndex(x => x.Login).IsUnique();
        modelBuilder.Entity<Storekeeper>().HasIndex(x => x.Mail).IsUnique();
        modelBuilder.Entity<Circle>().HasIndex(x => x.CircleName).IsUnique();
        modelBuilder.Entity<Material>().HasIndex(x => x.MaterialName).IsUnique();

        modelBuilder.Entity<Worker>().HasIndex(x => x.Login).IsUnique();
        modelBuilder.Entity<Worker>().HasIndex(x => x.Mail).IsUnique();
        modelBuilder.Entity<Lesson>().HasIndex(x => x.LessonName).IsUnique();
        modelBuilder.Entity<Interest>().HasIndex(x => x.InterestName).IsUnique();

        modelBuilder.Entity<CircleMaterial>().HasKey(x => new
        {
            x.MaterialId,
            x.CircleId
        });
        modelBuilder.Entity<InterestMaterial>().HasKey(x => new
        {
            x.MaterialId,
            x.InterestId
        });

        modelBuilder.Entity<LessonCircle>().HasKey(x => new
        {
            x.LessonId,
            x.CircleId
        });
        modelBuilder.Entity<LessonInterest>().HasKey(x => new
        {
            x.LessonId,
            x.InterestId
        });
        modelBuilder.Entity<MaterialByLesson>().HasNoKey();
    }

    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<Circle> Circles { get; set; }
    public DbSet<CircleMaterial> CircleMaterials { get; set; }
    public DbSet<Interest> Interests { get; set; }
    public DbSet<InterestMaterial> InterestMaterials { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<LessonCircle> LessonCircles { get; set; }
    public DbSet<LessonInterest> LessonInterests { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Medal> Medals { get; set; }
    public DbSet<Storekeeper> Storekeepers { get; set; }
    public DbSet<Worker> Workers { get; set; }
    public DbSet<MaterialByLesson> MaterialByLessons { get; set; }
}
