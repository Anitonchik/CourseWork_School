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

public class AchievementStorageContractTests : BaseStorageContractTests
{
    private AchievementStorageContract _achievementStorageContract;
    private Worker _worker;
    [SetUp]
    public void Setup()
    {
        _achievementStorageContract = new AchievementStorageContract(SchoolDbContext);
        _worker = new Worker()
        {
            Id = Guid.NewGuid().ToString(),
            FIO = "fio",
            Login = "login",
            Password = "password",
            Mail = "mail"
        };
        SchoolDbContext.Workers.Add(_worker);
    }

    [TearDown]
    public void TearDown()
    {
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Achievements\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Workers\" CASCADE; ");
    }
    [Test]
    public void TestAddLesson()
    {
        var achievement = new AchievementDataModel(Guid.NewGuid().ToString(), _worker.Id, "name", "nnnn");
        _achievementStorageContract.AddElement(achievement);

        var dbAchievement = _achievementStorageContract.GetElementById(achievement.Id);
        AssertElement(dbAchievement, achievement);
    }
    [Test]
    public void TestDeleteLesson()
    {
        var achievement = new AchievementDataModel(Guid.NewGuid().ToString(), _worker.Id, "name", "nnnn");
        _achievementStorageContract.AddElement(achievement);
        _achievementStorageContract.DelElement(achievement.Id);
        //_lessonStorageContract.GetElementById(lesson.Id);
        Assert.That(() => _achievementStorageContract.GetElementById(achievement.Id), Throws.TypeOf<ElementNotFoundException>());
    }

    [Test]
    public void UpdateLesson()
    {
        var achievement = new AchievementDataModel(Guid.NewGuid().ToString(), _worker.Id, "name", "nnnn");
        _achievementStorageContract.AddElement(achievement);

        var achievementDataModel = new AchievementDataModel(achievement.Id, _worker.Id, "new name", "new description");
        _achievementStorageContract.UpdElement(achievementDataModel);

        AssertElement(_achievementStorageContract.GetElementById(achievement.Id), achievementDataModel);
    }

    [Test]
    public void GetListLessons()
    {
        SchoolDbContext.Database.ExecuteSqlRaw("DELETE FROM \"Lessons\";");

        var achievement1 = new AchievementDataModel(Guid.NewGuid().ToString(), _worker.Id, "name1", "nnnn");
        var achievement2 = new AchievementDataModel(Guid.NewGuid().ToString(), _worker.Id, "name2", "nnnn");
        var achievement3 = new AchievementDataModel(Guid.NewGuid().ToString(), _worker.Id, "name3", "nnnn");

        _achievementStorageContract.AddElement(achievement1);
        _achievementStorageContract.AddElement(achievement2);
        _achievementStorageContract.AddElement(achievement3);

        var list = _achievementStorageContract.GetList();
        Assert.That(list.Count, Is.EqualTo(3));
    }
    private void AssertElement(AchievementDataModel actual, AchievementDataModel expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.AchievementName, Is.EqualTo(expected.AchievementName));
        Assert.That(actual.Description, Is.EqualTo(expected.Description));
        /*Assert.That(actual.Lessons.Count, Is.EqualTo(expected.Lessons.Count));*/
    }
}
