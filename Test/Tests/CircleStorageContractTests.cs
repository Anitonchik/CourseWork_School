using Microsoft.EntityFrameworkCore;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolDatabase;
using SchoolDatabase.Implementations;
using SchoolDatabase.Models;
using System;

namespace Test.Tests;

[TestFixture]
public class CircleStorageContractTests : BaseStorageContractTests
{
    private CircleStorageContract _circleStorageContract;
    private Storekeeper _storekeeper;


    [SetUp]
    public void Setup()
    {
        _circleStorageContract = new CircleStorageContract(SchoolDbContext);
        _storekeeper = new Storekeeper() { Id = Guid.NewGuid().ToString(), FIO = "fio", 
            Login = "login", Password = "password", Mail = "mail" };
        SchoolDbContext.Storekeepers.Add(_storekeeper);
    }

    [TearDown]
    public void TearDown()
    {
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Circles\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Storekeepers\" CASCADE; ");
    }


    [Test]
    public void TestAddCircle()
    {
        var circle = new CircleDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "nnnn", [], []);
        _circleStorageContract.AddElement(circle);

        var dbCircle = _circleStorageContract.GetElementById(circle.Id);
        AssertElement(dbCircle, circle);
    }

    [Test]
    public void TestDeleteCircle()
    {
        var circle = new CircleDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "name", "nnnn", [], []);
        _circleStorageContract.AddElement(circle);
        _circleStorageContract.DelElement(circle.Id);
        _circleStorageContract.GetElementById(circle.Id);
        Assert.That(() => _circleStorageContract.GetElementById(circle.Id), Throws.TypeOf<ElementNotFoundException>());
    }

    [Test]
    public void UpdateCircle()
    {
        var circle = new CircleDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "name", "nnnn", [], []);
        _circleStorageContract.AddElement(circle);

        var circleDataModel = new CircleDataModel(circle.Id, Guid.NewGuid().ToString(), "new name", "new description", [], []);
        _circleStorageContract.UpdElement(circleDataModel);

        AssertElement(_circleStorageContract.GetElementById(circle.Id), circleDataModel);
    }

    [Test]
    public void GetListCircles()
    {
        SchoolDbContext.Database.ExecuteSqlRaw("DELETE FROM \"Circles\";");

        var circle1 = new CircleDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "name", "nnnn", [], []);
        var circle2 = new CircleDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "name", "nnnn", [], []);
        var circle3 = new CircleDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "name", "nnnn", [], []);

        _circleStorageContract.AddElement(circle1);
        _circleStorageContract.AddElement(circle2);
        _circleStorageContract.AddElement(circle3);

        var list = _circleStorageContract.GetList();
        Assert.That(list.Count, Is.EqualTo(3));
    }

    private void AssertElement(CircleDataModel actual, CircleDataModel expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.CircleName, Is.EqualTo(expected.CircleName));
        Assert.That(actual.Description, Is.EqualTo(expected.Description));
        /*Assert.That(actual.Lessons.Count, Is.EqualTo(expected.Lessons.Count));*/
    }
}
