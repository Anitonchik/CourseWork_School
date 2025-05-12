using Microsoft.EntityFrameworkCore;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.ModelsForReports;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Implementations;
using SchoolDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Tests;

public class InterestStorageContractTests : BaseStorageContractTests
{
    private InterestStorageContract _interestStorageContract;
    private Worker _worker;
    private Storekeeper _storekeeper;
    [SetUp]
    public void Setup()
    {
        _interestStorageContract = new InterestStorageContract(SchoolDbContext);
        _worker = new Worker()
        {
            Id = Guid.NewGuid().ToString(),
            FIO = "fio",
            Login = "login",
            Password = "password",
            Mail = "mail"
        };
        SchoolDbContext.Workers.Add(_worker);
        _storekeeper = new Storekeeper()
        {
            Id = Guid.NewGuid().ToString(),
            FIO = "fio",
            Login = "login",
            Password = "password",
            Mail = "mail"
        };
        SchoolDbContext.Storekeepers.Add(_storekeeper);
    }

    [TearDown]
    public void TearDown()
    {
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Interests\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Workers\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Storekeepers\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Lessons\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Interests\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Achievements\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Circles\" CASCADE; ");
    }
    [Test]
    public void TestAddLesson()
    {
        var interest = new InterestDataModel(Guid.NewGuid().ToString(), _worker.Id, "name", "nnnn", []);
        _interestStorageContract.AddElement(interest);

        var dbInterest = _interestStorageContract.GetElementById(interest.Id);
        AssertElement(dbInterest, interest);
    }
    [Test]
    public void TestDeleteLesson()
    {
        var interest = new InterestDataModel(Guid.NewGuid().ToString(), _worker.Id, "name", "nnnn", []);
        _interestStorageContract.AddElement(interest);
        _interestStorageContract.DelElement(interest.Id);
        //_lessonStorageContract.GetElementById(lesson.Id);
        Assert.That(() => _interestStorageContract.GetElementById(interest.Id), Throws.TypeOf<ElementNotFoundException>());
    }

    [Test]
    public void UpdateLesson()
    {
        var lesson = new InterestDataModel(Guid.NewGuid().ToString(), _worker.Id, "name", "nnnn", []);
        _interestStorageContract.AddElement(lesson);

        var lessonDataModel = new InterestDataModel(lesson.Id, _worker.Id, "new name", "new description", []);
        _interestStorageContract.UpdElement(lessonDataModel);

        AssertElement(_interestStorageContract.GetElementById(lesson.Id), lessonDataModel);
    }

    [Test]
    public void GetListLessons()
    {
        SchoolDbContext.Database.ExecuteSqlRaw("DELETE FROM \"Lessons\";");

        var Lesson1 = new InterestDataModel(Guid.NewGuid().ToString(), _worker.Id, "name1", "nnnn", []);
        var Lesson2 = new InterestDataModel(Guid.NewGuid().ToString(), _worker.Id, "name2", "nnnn", []);
        var Lesson3 = new InterestDataModel(Guid.NewGuid().ToString(), _worker.Id, "name3", "nnnn", []);

        _interestStorageContract.AddElement(Lesson1);
        _interestStorageContract.AddElement(Lesson2);
        _interestStorageContract.AddElement(Lesson3);

        var list = _interestStorageContract.GetList(_worker.Id);
        Assert.That(list.Count, Is.EqualTo(3));
    }

    
    [Test]
    public void GetInterestsWithAchievementsWithCircles()
    {
        var interest1 = SchoolDbContext.InsertAndReturnInterest(workerId: _worker.Id, interestName: "name 1");
        var interest2 = SchoolDbContext.InsertAndReturnInterest(workerId: _worker.Id, interestName: "name 2");
        var interest3 = SchoolDbContext.InsertAndReturnInterest(workerId: _worker.Id, interestName: "name 3");

        var lesson1 = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id, lessonName: "name 1");
        var lesson2 = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id, lessonName: "name 2");
        var lesson3 = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id, lessonName: "name 3");

        var lessonInterest1 = SchoolDbContext.InsertAndReturnLessonInterest(lessonId: lesson1.Id, interestId: interest3.Id, category: "name 1");
        var lessonInterest2 = SchoolDbContext.InsertAndReturnLessonInterest(lessonId: lesson2.Id, interestId: interest2.Id, category: "name 2");
        var lessonInterest3 = SchoolDbContext.InsertAndReturnLessonInterest(lessonId: lesson3.Id, interestId: interest1.Id, category: "name 3");
        var lessonInterest4 = SchoolDbContext.InsertAndReturnLessonInterest(lessonId: lesson2.Id, interestId: interest3.Id, category: "name 4");
        var lessonInterest5 = SchoolDbContext.InsertAndReturnLessonInterest(lessonId: lesson3.Id, interestId: interest2.Id, category: "name 5");

        var circle1 = SchoolDbContext.InsertAndReturnCircle(storekeeperId: _storekeeper.Id, circleName: "name 1");
        var circle2 = SchoolDbContext.InsertAndReturnCircle(storekeeperId: _storekeeper.Id, circleName: "name 2");
        var circle3 = SchoolDbContext.InsertAndReturnCircle(storekeeperId: _storekeeper.Id, circleName: "name 3");

        var lessonCircle1 = SchoolDbContext.InsertAndReturnLessonCircle(lesson3.Id, circle2.Id);
        var lessonCircle2 = SchoolDbContext.InsertAndReturnLessonCircle(lesson3.Id, circle3.Id);
        var lessonCircle3 = SchoolDbContext.InsertAndReturnLessonCircle(lesson3.Id, circle1.Id);

        var achievement1 = SchoolDbContext.InsertAndReturnAchievement(workerId: _worker.Id, lessonId: lesson1.Id, achievementName: "name 1");
        var achievement2 = SchoolDbContext.InsertAndReturnAchievement(workerId: _worker.Id, lessonId: lesson2.Id, achievementName: "name 1");
        var achievement3 = SchoolDbContext.InsertAndReturnAchievement(workerId: _worker.Id, lessonId: lesson3.Id, achievementName: "name 1");

        var results = _interestStorageContract.GetInterestsWithAchievementsWithCircles(_worker.Id, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(+1));
        Assert.That(results.Count, Is.EqualTo(6));
    }
    private void AssertElement(InterestDataModel actual, InterestDataModel expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.InterestName, Is.EqualTo(expected.InterestName));
        Assert.That(actual.Description, Is.EqualTo(expected.Description));
        /*Assert.That(actual.Lessons.Count, Is.EqualTo(expected.Lessons.Count));*/
    }

}
