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
    private Worker _worker;
    private Achievement _achievement;

    [SetUp]
    public void Setup()
    {
        _lessonStorageContract = new LessonStorageContract(SchoolDbContext);
        _worker = new Worker()
        {
            Id = Guid.NewGuid().ToString(),
            FIO = "fio",
            Login = "login",
            Password = "password",
            Mail = "mail"
        };
        SchoolDbContext.Workers.Add(_worker);
        _achievement = new Achievement()
        {
            Id = Guid.NewGuid().ToString(),
            WorkerId = _worker.Id,
            AchievementName = "name",
            AchievementDate = DateTime.UtcNow,
            Description = "nnn"
        };

        SchoolDbContext.Achievements.Add(_achievement);
        SchoolDbContext.SaveChanges();

    }

    [TearDown]
    public void TearDown()
    {
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Lessons\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Workers\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Achievements\" CASCADE; ");
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
    private void AssertElement(LessonDataModel actual, LessonDataModel expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.AchievementId, Is.EqualTo(expected.AchievementId));
        Assert.That(actual.LessonName, Is.EqualTo(expected.LessonName));
        Assert.That(actual.Description, Is.EqualTo(expected.Description));
        /*Assert.That(actual.Lessons.Count, Is.EqualTo(expected.Lessons.Count));*/
    }
}

