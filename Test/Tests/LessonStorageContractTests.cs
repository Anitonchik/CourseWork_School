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

    private Worker _worker;
    private Achievement _achievement;

    [SetUp]
    public void Setup()
    {
        _lessonStorageContract = new LessonStorageContract(SchoolDbContext);
        _storekeeper = SchoolDbContext.InsertAndReturnStorekeeper();
        _worker = SchoolDbContext.InsertAndReturnWorker();
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
        _achievement = SchoolDbContext.InsertAndReturnAchievement(workerId: _worker.Id);

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
    public void TestAddLesson()
    {
        var lesson = new LessonDataModel(Guid.NewGuid().ToString(), _worker.Id, _achievement.Id, "name", "nnnn",[]);
        _lessonStorageContract.AddElement(lesson);

        var dbLesson = _lessonStorageContract.GetElementById(lesson.Id);
        AssertElement(dbLesson, lesson);
    }
    [Test]
    public void TestDeleteLesson()
    {
        var lesson = new LessonDataModel(Guid.NewGuid().ToString(), _worker.Id, _achievement.Id, "name", "nnnn", []);
        _lessonStorageContract.AddElement(lesson);
        _lessonStorageContract.DelElement(lesson.Id);
        //_lessonStorageContract.GetElementById(lesson.Id);
        Assert.That(() => _lessonStorageContract.GetElementById(lesson.Id), Throws.TypeOf<ElementNotFoundException>());
    }

    [Test]
    public void UpdateLesson()
    {
        var lesson = new LessonDataModel(Guid.NewGuid().ToString(), _worker.Id, _achievement.Id, "name", "nnnn",  []);
        _lessonStorageContract.AddElement(lesson);

        var lessonDataModel = new LessonDataModel(lesson.Id, _worker.Id, _achievement.Id, "new name", "new description", []);
        _lessonStorageContract.UpdElement(lessonDataModel);

        AssertElement(_lessonStorageContract.GetElementById(lesson.Id), lessonDataModel);
    }

    [Test]
    public void GetListLessons()
    {
        SchoolDbContext.Database.ExecuteSqlRaw("DELETE FROM \"Lessons\";");

        var Lesson1 = new LessonDataModel(Guid.NewGuid().ToString(), _worker.Id, _achievement.Id, "name1", "nnnn",  []);
        var Lesson2 = new LessonDataModel(Guid.NewGuid().ToString(), _worker.Id, _achievement.Id, "name2", "nnnn", []);
        var Lesson3 = new LessonDataModel(Guid.NewGuid().ToString(), _worker.Id, _achievement.Id, "name3", "nnnn", []);

        _lessonStorageContract.AddElement(Lesson1);
        _lessonStorageContract.AddElement(Lesson2);
        _lessonStorageContract.AddElement(Lesson3);

        var list = _lessonStorageContract.GetList();
        Assert.That(list.Count, Is.EqualTo(3));
    }
    [Test]
    public void GetLessonsByMaterial()
    {
        var interest1 = SchoolDbContext.InsertAndReturnInterest(workerId: _worker.Id, interestName: "name 1");
         var interest2 = SchoolDbContext.InsertAndReturnInterest(workerId: _worker.Id, interestName: "name 2");
         var interest3 = SchoolDbContext.InsertAndReturnInterest(workerId: _worker.Id, interestName: "name 3");

         var lesson1 = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id, achievementId: _achievement.Id, lessonName: "name 1");
         var lesson2 = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id, achievementId: _achievement.Id, lessonName: "name 2");
         var lesson3 = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id, achievementId: _achievement.Id, lessonName: "name 3");

         var lessonInterest1 = SchoolDbContext.InsertAndReturnLessonInterest(lessonId: lesson3.Id, interestId: interest1.Id);
         var lessonInterest2 = SchoolDbContext.InsertAndReturnLessonInterest(lessonId: lesson2.Id, interestId: interest2.Id);
         var lessonInterest3 = SchoolDbContext.InsertAndReturnLessonInterest(lessonId: lesson1.Id, interestId: interest3.Id);
         var lessonInterest4 = SchoolDbContext.InsertAndReturnLessonInterest(lessonId: lesson3.Id, interestId: interest2.Id);
         var lessonInterest5 = SchoolDbContext.InsertAndReturnLessonInterest(lessonId: lesson2.Id, interestId: interest3.Id);

         var material1 = SchoolDbContext.InsertAndReturnMaterial(storekeeperId: _storekeeper.Id, materialName: "name 1");
         var material2 = SchoolDbContext.InsertAndReturnMaterial(storekeeperId: _storekeeper.Id, materialName: "name 2");
         var material3 = SchoolDbContext.InsertAndReturnMaterial(storekeeperId: _storekeeper.Id, materialName: "name 3");

         var interestMaterial1 = SchoolDbContext.InsertAndReturnInterestMaterial(interest1.Id, material3.Id);
         var interestMaterial2 = SchoolDbContext.InsertAndReturnInterestMaterial(interest2.Id, material3.Id);
         var interestMaterial3 = SchoolDbContext.InsertAndReturnInterestMaterial(interest3.Id, material3.Id);

         var list = _lessonStorageContract.GetLessonsByMaterial(material3.Id);
        //Assert.That(list.Count, Is.EqualTo(3));

    }
    
    private void AssertElement(LessonDataModel actual, LessonDataModel expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.AchievementId, Is.EqualTo(expected.AchievementId));
        Assert.That(actual.LessonName, Is.EqualTo(expected.LessonName));
        Assert.That(actual.Description, Is.EqualTo(expected.Description));
        /*Assert.That(actual.Lessons.Count, Is.EqualTo(expected.Lessons.Count));*/
    }
}

