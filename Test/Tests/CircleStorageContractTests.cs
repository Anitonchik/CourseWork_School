using Microsoft.EntityFrameworkCore;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolDatabase;
using SchoolDatabase.Implementations;
using SchoolDatabase.Models;

namespace Test.Tests;

[TestFixture]
public class CircleStorageContractTests : BaseStorageContractTests
{
    private CircleStorageContract _circleStorageContract;
    private Storekeeper _storekeeper;
    private Material _material;
    private Lesson _lesson;

    private Worker _worker;
    private Achievement _achievement;


    [SetUp]
    public void Setup()
    {
        _circleStorageContract = new CircleStorageContract(SchoolDbContext);

        _storekeeper = new Storekeeper() { 
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
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Circles\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Storekeepers\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Workers\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Materials\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Achievements\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Lessons\" CASCADE; ");
    }


    [Test]
    public void TestAddCircle()
    {
        var circleId = Guid.NewGuid().ToString();
        var circle = new CircleDataModel(circleId, _storekeeper.Id, "name", "nnnn", 
            [new CircleMaterialDataModel (circleId, _material.Id, 2)], 
            [new LessonCircleDataModel(_lesson.Id, circleId)]);
        _circleStorageContract.AddElement(circle);

        var dbCircle = _circleStorageContract.GetElementById(circle.Id);
        AssertElement(dbCircle, circle);
    }

    [Test]
    public void TestDeleteCircle()
    {
        var circle = new CircleDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "nnnn", [], []);
        _circleStorageContract.AddElement(circle);
        _circleStorageContract.DelElement(circle.Id);

        Assert.That(() => _circleStorageContract.GetElementById(circle.Id), Throws.TypeOf<ElementNotFoundException>());
    }

    [Test]
    public void UpdateCircle()
    {
        var circle = new CircleDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "nnnn", [], []);
        _circleStorageContract.AddElement(circle);

        var circleDataModel = new CircleDataModel(circle.Id, _storekeeper.Id, "new name", "new description", [], []);
        _circleStorageContract.UpdElement(circleDataModel);

        AssertElement(_circleStorageContract.GetElementById(circle.Id), circleDataModel);
    }

    [Test]
    public void GetListCircles()
    {

        var circle1 = new CircleDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name 1", "nnnn", [], []);
        var circle2 = new CircleDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name 2", "nnnn", [], []);
        var circle3 = new CircleDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name 3", "nnnn", [], []);

        _circleStorageContract.AddElement(circle1);
        _circleStorageContract.AddElement(circle2);
        _circleStorageContract.AddElement(circle3);

        var list = _circleStorageContract.GetList();
        Assert.That(list.Count, Is.EqualTo(3));
    }

    [Test]
    public void GetCirclesWithInterestsWithMedals()
    {
        var circle1 = SchoolDbContext.InsertAndReturnCircle(storekeeperId: _storekeeper.Id, circleName: "name 1");
        var circle2 = SchoolDbContext.InsertAndReturnCircle(storekeeperId: _storekeeper.Id, circleName: "name 2");
        var circle3 = SchoolDbContext.InsertAndReturnCircle(storekeeperId: _storekeeper.Id, circleName: "name 3");

        var material1 = SchoolDbContext.InsertAndReturnMaterial(storekeeperId: _storekeeper.Id, materialName: "name 1");
        var material2 = SchoolDbContext.InsertAndReturnMaterial(storekeeperId: _storekeeper.Id, materialName: "name 2");
        var material3 = SchoolDbContext.InsertAndReturnMaterial(storekeeperId: _storekeeper.Id, materialName: "name 3");

        var circleMaterial1 = SchoolDbContext.InsertAndReturnCircleMaterial(circleId: circle1.Id, materialId: material3.Id);
        var circleMaterial2 = SchoolDbContext.InsertAndReturnCircleMaterial(circleId: circle2.Id, materialId: material2.Id);
        var circleMaterial3 = SchoolDbContext.InsertAndReturnCircleMaterial(circleId: circle3.Id, materialId: material1.Id);
        var circleMaterial4 = SchoolDbContext.InsertAndReturnCircleMaterial(circleId: circle2.Id, materialId: material3.Id);
        var circleMaterial5 = SchoolDbContext.InsertAndReturnCircleMaterial(circleId: circle3.Id, materialId: material2.Id);

        var lesson1 = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id, achievementId: _achievement.Id, lessonName: "name 1");
        var lesson2 = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id, achievementId: _achievement.Id, lessonName: "name 1");
        var lesson3 = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id, achievementId: _achievement.Id, lessonName: "name 1");

        var lessonCircle1 = SchoolDbContext.InsertAndReturnLessonCircle(lesson3.Id, circle2.Id);
        var lessonCircle2 = SchoolDbContext.InsertAndReturnLessonCircle(lesson3.Id, circle3.Id);
        var lessonCircle3 = SchoolDbContext.InsertAndReturnLessonCircle(lesson3.Id, circle1.Id);

        var medal1 = SchoolDbContext.InsertAndReturnMedal(storekeeperId: _storekeeper.Id, materialId: material1.Id, medalName: "name 1");
        var medal1 = SchoolDbContext.InsertAndReturnMedal(storekeeperId: _storekeeper.Id, materialId: material1.Id, medalName: "name 1");
        var medal1 = SchoolDbContext.InsertAndReturnMedal(storekeeperId: _storekeeper.Id, materialId: material1.Id, medalName: "name 1");
    }




    private void AssertElement(CircleDataModel actual, CircleDataModel expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.CircleName, Is.EqualTo(expected.CircleName));
        Assert.That(actual.Description, Is.EqualTo(expected.Description));
    }
}
