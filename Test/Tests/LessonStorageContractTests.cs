using Microsoft.EntityFrameworkCore;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Implementations;
using SchoolDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Tests;
[TestFixture]
public class LessonStorageContractTests : BaseStorageContractTests
{
    private LessonStorageContract _lessonStorageContract;
    private Storekeeper _storekeeper;
    private Material _material;
    private Lesson _lesson;
    private Interest _interest;

    private Worker _worker;
    private Achievement _achievement;

    [SetUp]
    public void Setup()
    {
        _lessonStorageContract = new LessonStorageContract(SchoolDbContext);
        _storekeeper = SchoolDbContext.InsertAndReturnStorekeeper();
        _worker = SchoolDbContext.InsertAndReturnWorker();
        _interest = SchoolDbContext.InsertAndReturnInterest(workerId: _worker.Id);
        /*_worker = new Worker()
        {
            Id = Guid.NewGuid().ToString(),
            FIO = "fio",
            Login = "login",
            Password = "password",
            Mail = "mail"
        };
        SchoolDbContext.Workers.Add(_worker);*/
        /*_achievement = new Achievement()
        {
            Id = Guid.NewGuid().ToString(),
            WorkerId = _worker.Id,
            AchievementName = "name",
            AchievementDate = DateTime.UtcNow,
            Description = "nnn"
        };

        SchoolDbContext.Achievements.Add(_achievement);
        SchoolDbContext.SaveChanges();*/
        //_achievement = SchoolDbContext.InsertAndReturnAchievement(workerId: _worker.Id);

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
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Interests\" CASCADE; ");
    }
    [Test]
    public void TestAddLesson()
    {
        var lesson = new LessonDataModel(Guid.NewGuid().ToString(), _worker.Id,  "name", DateTime.UtcNow, "nnnn",[]);
        _lessonStorageContract.AddElement(lesson);

        var dbLesson = _lessonStorageContract.GetElementById(lesson.Id);
        AssertElement(dbLesson, lesson);
    }
    [Test]
    public void TestDeleteLesson()
    {
        var lesson = new LessonDataModel(Guid.NewGuid().ToString(), _worker.Id,  "name",DateTime.UtcNow, "nnnn", []);
        _lessonStorageContract.AddElement(lesson);
        _lessonStorageContract.DelElement(lesson.Id);
        //_lessonStorageContract.GetElementById(lesson.Id);
        Assert.That(() => _lessonStorageContract.GetElementById(lesson.Id), Throws.TypeOf<ElementNotFoundException>());
    }

    [Test]
    public void UpdateLesson()
    {
        var lesson = new LessonDataModel(Guid.NewGuid().ToString(), _worker.Id,  "name", DateTime.UtcNow, "nnnn",  []);
        _lessonStorageContract.AddElement(lesson);

        var lessonDataModel = new LessonDataModel(lesson.Id, _worker.Id, "new name", DateTime.UtcNow, "new description", []);
        _lessonStorageContract.UpdElement(lessonDataModel);

        AssertElement(_lessonStorageContract.GetElementById(lesson.Id), lessonDataModel);
    }

    [Test]
    public void GetListLessons()
    {

        var Lesson1 = new LessonDataModel(Guid.NewGuid().ToString(), _worker.Id, "name1", DateTime.UtcNow, "nnnn",  []);
        var Lesson2 = new LessonDataModel(Guid.NewGuid().ToString(), _worker.Id,  "name2", DateTime.UtcNow, "nnnn", []);
        var Lesson3 = new LessonDataModel(Guid.NewGuid().ToString(), _worker.Id,  "name3", DateTime.UtcNow, "nnnn", []);

        _lessonStorageContract.AddElement(Lesson1);
        _lessonStorageContract.AddElement(Lesson2);
        _lessonStorageContract.AddElement(Lesson3);

        var list = _lessonStorageContract.GetList(_worker.Id);
        Assert.That(list.Count, Is.EqualTo(3));
    }
    [Test]
    public void GetLessonsByMaterial()
    {
        var interest1 = SchoolDbContext.InsertAndReturnInterest(workerId: _worker.Id, interestName: "name 1");
         var interest2 = SchoolDbContext.InsertAndReturnInterest(workerId: _worker.Id, interestName: "name 2");
         var interest3 = SchoolDbContext.InsertAndReturnInterest(workerId: _worker.Id, interestName: "name 3");

         var lesson1 = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id, lessonName: "name 1");
         var lesson2 = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id, lessonName: "name 2");
         var lesson3 = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id, lessonName: "name 3");

         var lessonInterest1 = SchoolDbContext.InsertAndReturnLessonInterest(lessonId: lesson3.Id, interestId: interest1.Id, category: "name1");
         var lessonInterest2 = SchoolDbContext.InsertAndReturnLessonInterest(lessonId: lesson2.Id, interestId: interest2.Id, category: "name2");
         var lessonInterest3 = SchoolDbContext.InsertAndReturnLessonInterest(lessonId: lesson1.Id, interestId: interest3.Id, category: "name3");
         var lessonInterest4 = SchoolDbContext.InsertAndReturnLessonInterest(lessonId: lesson3.Id, interestId: interest2.Id, category: "name4");
         var lessonInterest5 = SchoolDbContext.InsertAndReturnLessonInterest(lessonId: lesson2.Id, interestId: interest3.Id, category: "name5");

         var material1 = SchoolDbContext.InsertAndReturnMaterial(storekeeperId: _storekeeper.Id, materialName: "name 1");
         var material2 = SchoolDbContext.InsertAndReturnMaterial(storekeeperId: _storekeeper.Id, materialName: "name 2");
         var material3 = SchoolDbContext.InsertAndReturnMaterial(storekeeperId: _storekeeper.Id, materialName: "name 3");

         var interestMaterial1 = SchoolDbContext.InsertAndReturnInterestMaterial(interest1.Id, material3.Id);
         var interestMaterial2 = SchoolDbContext.InsertAndReturnInterestMaterial(interest2.Id, material3.Id);
         var interestMaterial3 = SchoolDbContext.InsertAndReturnInterestMaterial(interest3.Id, material3.Id);

         var list = _lessonStorageContract.GetLessonsByMaterial(_worker.Id,material3.Id);
        Assert.That(list.Count, Is.EqualTo(5));

    }
    [Test]
    public void GetLessons_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var workerId = Guid.NewGuid().ToString();
        SchoolDbContext.InsertAndReturnStorekeeper(workerId, login: "login 2", password: "psw 2", mail: "mail 2");

        var id = Guid.NewGuid().ToString();
        var listOriginal = new List<LessonDataModel>()
        {
            new(id, workerId, "name 1",DateTime.UtcNow, "desc", [(new LessonInterestDataModel (id, _interest.Id,"name"))]),
            new(Guid.NewGuid().ToString(), workerId, "name 2",DateTime.UtcNow.AddDays(1), "desc", []),
            new(Guid.NewGuid().ToString(), workerId, "name 3",DateTime.UtcNow.AddDays(2), "desc", []),
        };

        //Act
        var list = _lessonStorageContract.GetList(_worker.Id);
        //Assert
        Assert.That(list, Is.Not.Null);
        Assert.That(list.Count().Equals(0));
    }

    private void AssertElement(LessonDataModel actual, LessonDataModel expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.LessonName, Is.EqualTo(expected.LessonName));
        Assert.That(actual.Description, Is.EqualTo(expected.Description));
        /*Assert.That(actual.Lessons.Count, Is.EqualTo(expected.Lessons.Count));*/
    }
}

