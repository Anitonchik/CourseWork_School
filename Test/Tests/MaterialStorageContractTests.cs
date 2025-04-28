using Microsoft.EntityFrameworkCore;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolDatabase.Implementations;
using SchoolDatabase.Models;

namespace Test.Tests;

[TestFixture]
internal class MaterialStorageContractTests : BaseStorageContractTests
{
    private MaterialStorageContract _materialStorageContract;
    private Storekeeper _storekeeper;
    private Material _material;
    private Lesson _lesson;

    private Worker _worker;
    private Achievement _achievement;


    [SetUp]
    public void Setup()
    {
        _materialStorageContract = new MaterialStorageContract(SchoolDbContext);
        _storekeeper = new Storekeeper()
        {
            Id = Guid.NewGuid().ToString(),
            FIO = "fio",
            Login = "login",
            Password = "password",
            Mail = "mail"
        };
        SchoolDbContext.Storekeepers.Add(_storekeeper);

        _worker = new Worker()
        {
            Id = Guid.NewGuid().ToString(),
            FIO = "fio",
            Login = "login",
            Password = "password",
            Mail = "mail"
        };
        SchoolDbContext.Workers.Add(_worker);

        _material = new Material()
        {
            Id = Guid.NewGuid().ToString(),
            StorekeeperId = _storekeeper.Id,
            MaterialName = "name",
            Description = "description"
        };
        SchoolDbContext.Materials.Add(_material);

        _achievement = new Achievement()
        {
            Id = Guid.NewGuid().ToString(),
            WorkerId = _worker.Id,
            AchievementName = "name",
            Description = "description"
        };
        SchoolDbContext.Achievements.Add(_achievement);

        _lesson = new Lesson()
        {
            Id = Guid.NewGuid().ToString(),
            WorkerId = _worker.Id,
            AchievementId = _achievement.Id,
            LessonName = "name",
            LessonDate = DateTime.Now,
            Description = "description"
        };
        SchoolDbContext.Lessons.Add(_lesson);
    }

    [TearDown]
    public void TearDown()
    {
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Materials\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Storekeepers\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Workers\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Materials\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Achievements\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Lessons\" CASCADE; ");
    }


    [Test]
    public void TestAddMaterial()
    {
        var material = new MaterialDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "nnnn");
        _materialStorageContract.AddElement(material);

        var dbMaterial = _materialStorageContract.GetElementById(material.Id);
        AssertElement(dbMaterial, material);
    }

    [Test]
    public void TestDeleteMaterial()
    {
        var material = new MaterialDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "nnnn");
        _materialStorageContract.AddElement(material);
        _materialStorageContract.DelElement(material.Id);

        Assert.That(() => _materialStorageContract.GetElementById(material.Id), Throws.TypeOf<ElementNotFoundException>());
    }

    [Test]
    public void UpdateMaterial()
    {
        var material = new MaterialDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "nnnn");
        _materialStorageContract.AddElement(material);

        var materialDataModel = new MaterialDataModel(material.Id, _storekeeper.Id, "new name", "new description");
        _materialStorageContract.UpdElement(materialDataModel);

        AssertElement(_materialStorageContract.GetElementById(material.Id), materialDataModel);
    }

    [Test]
    public void GetListMaterials()
    {

        var material1 = new MaterialDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name 1", "nnnn");
        var material2 = new MaterialDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name 2", "nnnn");
        var material3 = new MaterialDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name 3", "nnnn");

        _materialStorageContract.AddElement(material1);
        _materialStorageContract.AddElement(material2);
        _materialStorageContract.AddElement(material3);

        var list = _materialStorageContract.GetList();
        Assert.That(list.Count, Is.EqualTo(3));
    }

    [Test]
    public void GetMaterialsByLesson()
    {
        var circle1 = InsertAndReturnCircle(circleName: "name 1");
        var circle2 = InsertAndReturnCircle(circleName: "name 2");
        var circle3 = InsertAndReturnCircle(circleName: "name 3");

        var material1 = InsertAndReturnMaterial(materialName: "name 1");
        var material2 = InsertAndReturnMaterial(materialName: "name 2");
        var material3 = InsertAndReturnMaterial(materialName: "name 3");

        var circleMaterial1 = InsertAndReturnCircleMaterial(circleId: circle1.Id, materialId: material3.Id);
        var circleMaterial2 = InsertAndReturnCircleMaterial(circleId: circle2.Id, materialId: material2.Id);
        var circleMaterial3 = InsertAndReturnCircleMaterial(circleId: circle3.Id, materialId: material1.Id);

        var lesson1 = InsertAndReturnLesson(workerId: _worker.Id, achievementId: _achievement.Id, lessonName: "name 1");
        var lesson2 = InsertAndReturnLesson(workerId: _worker.Id, achievementId: _achievement.Id, lessonName: "name 1");
        var lesson3 = InsertAndReturnLesson(workerId: _worker.Id, achievementId: _achievement.Id, lessonName: "name 1");

        var lessonCircle1 = InsertAndReturnLessonCircle(lesson3.Id, circle2.Id);
        var lessonCircle2 = InsertAndReturnLessonCircle(lesson3.Id, circle3.Id);

        var list = _materialStorageContract.GetMaterialsByLesson(lesson3.Id);
        Assert.That(list.Count, Is.EqualTo(2));
    }

    private void AssertElement(MaterialDataModel actual, MaterialDataModel expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.MaterialName, Is.EqualTo(expected.MaterialName));
        Assert.That(actual.Description, Is.EqualTo(expected.Description));
        /*Assert.That(actual.Lessons.Count, Is.EqualTo(expected.Lessons.Count));*/
    }

    private Circle InsertAndReturnCircle(string id = null, string storekeeperId = null,
        string circleName = "name", string description = "desc")
    {
        var circle = new Circle()
        {
            Id = id ?? Guid.NewGuid().ToString(),
            StorekeeperId = storekeeperId ?? Guid.NewGuid().ToString(),
            CircleName = circleName,
            Description = description
        };
        SchoolDbContext.Circles.Add(circle);
        return circle;
    }

    private Material InsertAndReturnMaterial(string id = null, string storekeeperId = null,
        string materialName = "name", string description = "desc")
    {
        var material = new Material()
        {
            Id = id ?? Guid.NewGuid().ToString(),
            StorekeeperId = storekeeperId ?? Guid.NewGuid().ToString(),
            MaterialName = materialName,
            Description = description
        };
        SchoolDbContext.Materials.Add(material);
        return material;
    }

    private CircleMaterial InsertAndReturnCircleMaterial(string circleId = null, string materialId = null, int count = 1)
    {
        var circleMaterial = new CircleMaterial()
        {
            CircleId = circleId ?? Guid.NewGuid().ToString(),
            MaterialId = materialId ?? Guid.NewGuid().ToString(),
            Count = count
        };
        SchoolDbContext.CircleMaterials.Add(circleMaterial);
        return circleMaterial;
    }

    private Lesson InsertAndReturnLesson(string id = null, string workerId = null,
        string achievementId = null, string lessonName = "name")
    {
        var lesson = new Lesson()
        {
            Id = id ?? Guid.NewGuid().ToString(),
            WorkerId = workerId ?? Guid.NewGuid().ToString(),
            AchievementId = achievementId ?? Guid.NewGuid().ToString(),
            LessonName = lessonName,
            LessonDate = DateTime.UtcNow
        };
        SchoolDbContext.Lessons.Add(lesson);
        return lesson;
    }

    private LessonCircle InsertAndReturnLessonCircle(string lessonId = null, string circleId = null, int count = 1)
    {
        var lessonCircle = new LessonCircle()
        {
            LessonId = lessonId ?? Guid.NewGuid().ToString(),
            CircleId = circleId ?? Guid.NewGuid().ToString()
        };
        SchoolDbContext.LessonCircles.Add(lessonCircle);
        return lessonCircle;
    }
}
