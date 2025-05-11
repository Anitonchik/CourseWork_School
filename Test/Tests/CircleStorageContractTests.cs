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
            [new LessonCircleDataModel(_lesson.Id, circleId, 2)]);
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

        var list = _circleStorageContract.GetList(_storekeeper.Id);
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

        var lesson1 = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id,  lessonName: "name 1");
        var lesson2 = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id,  lessonName: "name 2");
        var lesson3 = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id,  lessonName: "name 3");

        var lessonCircle1 = SchoolDbContext.InsertAndReturnLessonCircle(lesson3.Id, circle2.Id);
        var lessonCircle2 = SchoolDbContext.InsertAndReturnLessonCircle(lesson3.Id, circle3.Id);
        var lessonCircle3 = SchoolDbContext.InsertAndReturnLessonCircle(lesson3.Id, circle1.Id);

        var medal1 = SchoolDbContext.InsertAndReturnMedal(storekeeperId: _storekeeper.Id, materialId: material2.Id, medalName: "name 1");
        var medal2 = SchoolDbContext.InsertAndReturnMedal(storekeeperId: _storekeeper.Id, materialId: material1.Id, medalName: "name 2");
        var medal3 = SchoolDbContext.InsertAndReturnMedal(storekeeperId: _storekeeper.Id, materialId: material3.Id, medalName: "name 3");

        var interest1 = SchoolDbContext.InsertAndReturnInterest(workerId: _worker.Id, interestName: "name 1", description: "desc 1");
        var interest2 = SchoolDbContext.InsertAndReturnInterest(workerId: _worker.Id, interestName: "name 2", description: "desc 2");
        var interest3 = SchoolDbContext.InsertAndReturnInterest(workerId: _worker.Id, interestName: "name 3", description: "desc 3");

        var lessonInterest1 = SchoolDbContext.InsertAndReturnLessonInterest(lessonId: lesson1.Id, interestId: interest2.Id);
        var lessonInterest2 = SchoolDbContext.InsertAndReturnLessonInterest(lessonId: lesson2.Id, interestId: interest2.Id);
        var lessonInterest3 = SchoolDbContext.InsertAndReturnLessonInterest(lessonId: lesson1.Id, interestId: interest1.Id);
        var lessonInterest4 = SchoolDbContext.InsertAndReturnLessonInterest(lessonId: lesson3.Id, interestId: interest1.Id);

        var interestMaterial1 = SchoolDbContext.InsertAndReturnInterestMaterial(interestId: interest1.Id, materialId: material3.Id);
        var interestMaterial2 = SchoolDbContext.InsertAndReturnInterestMaterial(interestId: interest2.Id, materialId: material3.Id);
        var interestMaterial3 = SchoolDbContext.InsertAndReturnInterestMaterial(interestId: interest2.Id, materialId: material2.Id);
        var interestMaterial4 = SchoolDbContext.InsertAndReturnInterestMaterial(interestId: interest1.Id, materialId: material2.Id);
        var interestMaterial5 = SchoolDbContext.InsertAndReturnInterestMaterial(interestId: interest3.Id, materialId: material1.Id);

        var list = _circleStorageContract.GetCirclesWithInterestsWithMedals(_storekeeper.Id, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(3));
        Assert.That(list.Count.Equals(16));
    }

    [Test]
    public void GetCircles_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        SchoolDbContext.InsertAndReturnStorekeeper(storekeeperId, login: "login 1", password: "psw 1", mail: "mail 1");

        var id = Guid.NewGuid().ToString();
        var listOriginal = new List<CircleDataModel>()
        {
            new(id, storekeeperId, "name 1", "desc", [(new CircleMaterialDataModel (id, _material.Id, 1))], []),
            new(Guid.NewGuid().ToString(), storekeeperId, "name 2", "desc", [], []),
            new(Guid.NewGuid().ToString(), storekeeperId, "name 3", "desc", [], []),
        };

        //Act
        var list = _circleStorageContract.GetList(_storekeeper.Id);
        //Assert
        Assert.That(list, Is.Not.Null);
        Assert.That(list.Count().Equals(0));
    }




    private void AssertElement(CircleDataModel actual, CircleDataModel expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.CircleName, Is.EqualTo(expected.CircleName));
        Assert.That(actual.Description, Is.EqualTo(expected.Description));
    }
}
