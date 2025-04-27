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

public class InterestStorageContractTests : BaseStorageContractTests
{
    private InterestStorageContract _interestStorageContract;
    private Worker _worker;
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
    }

    [TearDown]
    public void TearDown()
    {
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Interests\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Workers\" CASCADE; ");
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

        var list = _interestStorageContract.GetList();
        Assert.That(list.Count, Is.EqualTo(3));
    }
    private void AssertElement(InterestDataModel actual, InterestDataModel expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.InterestName, Is.EqualTo(expected.InterestName));
        Assert.That(actual.Description, Is.EqualTo(expected.Description));
        /*Assert.That(actual.Lessons.Count, Is.EqualTo(expected.Lessons.Count));*/
    }

}
