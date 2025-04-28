using SchoolDatabase.Models;
using SchoolDatabase;
using Microsoft.EntityFrameworkCore;

namespace Test;

public static class SchoolDbContextExtensions
{
    public static Storekeeper InsertAndReturnStorekeeper(this SchoolDbContext dbContext, string id = null, 
        string fio = "fio", string login = "login", string password = "password", string mail = "mail")
    {
        var storekeeper = new Storekeeper()
        {
            Id = Guid.NewGuid().ToString(),
            FIO = fio,
            Login = login,
            Password = password,
            Mail = mail
        };
        dbContext.Storekeepers.Add(storekeeper);
        dbContext.SaveChanges();
        return storekeeper;
    }
    
    public static Worker InsertAndReturnWorker (this SchoolDbContext dbContext, string id = null, 
        string fio = "fio", string login = "login", string password = "password", string mail = "mail")
    {
        var worker = new Worker()
        {
            Id = Guid.NewGuid().ToString(),
            FIO = fio,
            Login = login,
            Password = password,
            Mail = mail
        };
        dbContext.Workers.Add(worker);
        dbContext.SaveChanges();
        return worker;
    }

    public static Circle InsertAndReturnCircle(this SchoolDbContext dbContext, string id = null, string storekeeperId = null,
        string circleName = "name", string description = "desc")
    {
        var circle = new Circle()
        {
            Id = id ?? Guid.NewGuid().ToString(),
            StorekeeperId = storekeeperId ?? Guid.NewGuid().ToString(),
            CircleName = circleName,
            Description = description
        };
        dbContext.Circles.Add(circle);
        dbContext.SaveChanges();
        return circle;
    }

    public static Material InsertAndReturnMaterial(this SchoolDbContext dbContext, string id = null, string storekeeperId = null,
        string materialName = "name", string description = "desc")
    {
        var material = new Material()
        {
            Id = id ?? Guid.NewGuid().ToString(),
            StorekeeperId = storekeeperId ?? Guid.NewGuid().ToString(),
            MaterialName = materialName,
            Description = description
        };
        dbContext.Materials.Add(material);
        dbContext.SaveChanges();
        return material;
    }

    public static CircleMaterial InsertAndReturnCircleMaterial(this SchoolDbContext dbContext, string circleId = null, string materialId = null, int count = 1)
    {
        var circleMaterial = new CircleMaterial()
        {
            CircleId = circleId ?? Guid.NewGuid().ToString(),
            MaterialId = materialId ?? Guid.NewGuid().ToString(),
            Count = count
        };
        dbContext.CircleMaterials.Add(circleMaterial);
        dbContext.SaveChanges();
        return circleMaterial;
    }

    public static Lesson InsertAndReturnLesson(this SchoolDbContext dbContext, string? id = null, string? workerId = null,
        string? achievementId = null, string lessonName = "name", string description = "desc")
    {
        var lesson = new Lesson()
        {
            Id = id ?? Guid.NewGuid().ToString(),
            WorkerId = workerId ?? Guid.NewGuid().ToString(),
            AchievementId = achievementId ?? Guid.NewGuid().ToString(),
            LessonName = lessonName,
            LessonDate = DateTime.UtcNow,
            Description = description
        };
        dbContext.Lessons.Add(lesson);
        dbContext.SaveChanges();
        return lesson;
    }

    public static LessonCircle InsertAndReturnLessonCircle(this SchoolDbContext dbContext, string? lessonId = null, 
        string? circleId = null, int count = 1)
    {
        var lessonCircle = new LessonCircle()
        {
            LessonId = lessonId ?? Guid.NewGuid().ToString(),
            CircleId = circleId ?? Guid.NewGuid().ToString()
        };
        dbContext.LessonCircles.Add(lessonCircle);
        dbContext.SaveChanges();
        return lessonCircle;
    }

    public static Achievement InsertAndReturnAchievement(this SchoolDbContext dbContext, string? id = null, 
        string? workerId = null, string achievementName = "name", string description = "description")
    {
        var achievement = new Achievement()
        {
            Id = Guid.NewGuid().ToString(),
            WorkerId = workerId ?? Guid.NewGuid().ToString(),
            AchievementName = achievementName,
            Description = description
        };
        dbContext.Achievements.Add(achievement);
        dbContext.SaveChanges();
        return achievement;
    }
     
    public static Medal InsertAndReturnMedal(this SchoolDbContext dbContext, string? id = null, string? storekeeperId = null,
        string? materialId = null, string medalName = "name", int range = 1)
    {
        var medal = new Medal()
        {
            Id = Guid.NewGuid().ToString(),
            StorekeeperId = storekeeperId ?? Guid.NewGuid().ToString(),
            MaterialId = materialId ?? Guid.NewGuid().ToString(),
            MedalName = medalName,
            Range = range
        };
        dbContext.Medals.Add(medal);
        dbContext.SaveChanges();
        return medal;
    }
}
