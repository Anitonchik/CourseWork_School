using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolDatabase.Implementations;
using SchoolDatabase.Models;
using SchoolDatabase;
using Microsoft.EntityFrameworkCore;
using SchoolContracts.StoragesContracts;

namespace Test.Tests;

[TestFixture]
public class AchievementStorageContractTests : BaseStorageContractTests
{
    private AchievementStorageContract _achievementStorageContract;
    private LessonStorageContract _lessonStorageContract;
    private Worker _worker;
    private Lesson _lesson;
    [SetUp]
    public void Setup()
    {
        _lessonStorageContract = new LessonStorageContract(SchoolDbContext);
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
        _lesson = new Lesson()
        {
            Id = Guid.NewGuid().ToString(),
            WorkerId = _worker.Id,
            LessonName = "name",
            Description = "description"
        };
        SchoolDbContext.Lessons.Add(_lesson);
    }

    [TearDown]
    public void TearDown()
    {
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Achievements\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Workers\" CASCADE; ");
    }
    [Test]
    public void TestAddAchievement()
    {
        var achievement = new AchievementDataModel(Guid.NewGuid().ToString(), _worker.Id, _lesson.Id, "name", "nnnn");
        _achievementStorageContract.AddElement(achievement);

        var dbAchievement = _achievementStorageContract.GetElementById(achievement.Id);
        AssertElement(dbAchievement, achievement);
    }
    [Test]
    public void TestDeleteAchievement()
    {
        var achievement = new AchievementDataModel(Guid.NewGuid().ToString(), _worker.Id, _lesson.Id, "name", "nnnn");
        _achievementStorageContract.AddElement(achievement);
        _achievementStorageContract.DelElement(achievement.Id);
        //_lessonStorageContract.GetElementById(lesson.Id);
        Assert.That(() => _achievementStorageContract.GetElementById(achievement.Id), Throws.TypeOf<ElementNotFoundException>());
    }

    [Test]
    public void UpdateAchievement()
    {
        var achievement = new AchievementDataModel(Guid.NewGuid().ToString(), _worker.Id, _lesson.Id, "name", "nnnn");
        _achievementStorageContract.AddElement(achievement);

        var achievementDataModel = new AchievementDataModel(achievement.Id, _worker.Id, _lesson.Id, "new name", "new description");
        _achievementStorageContract.UpdElement(achievementDataModel);

        AssertElement(_achievementStorageContract.GetElementById(achievement.Id), achievementDataModel);
    }

    [Test]
    public void GetListAchievements()
    {

        var achievement1 = new AchievementDataModel(Guid.NewGuid().ToString(), _worker.Id, _lesson.Id, "name1", "nnnn");
        var achievement2 = new AchievementDataModel(Guid.NewGuid().ToString(), _worker.Id, _lesson.Id,"name2", "nnnn");
        var achievement3 = new AchievementDataModel(Guid.NewGuid().ToString(), _worker.Id, _lesson.Id, "name3", "nnnn");

        _achievementStorageContract.AddElement(achievement1);
        _achievementStorageContract.AddElement(achievement2);
        _achievementStorageContract.AddElement(achievement3);

        var list = _achievementStorageContract.GetList(_worker.Id);
        Assert.That(list.Count, Is.EqualTo(3));
    }
    private void AssertElement(AchievementDataModel actual, AchievementDataModel expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.WorkerId, Is.EqualTo(expected.WorkerId));
        Assert.That(actual.LessonId, Is.EqualTo(expected.LessonId));
        Assert.That(actual.AchievementName, Is.EqualTo(expected.AchievementName));
        Assert.That(actual.Description, Is.EqualTo(expected.Description));
    }
}
