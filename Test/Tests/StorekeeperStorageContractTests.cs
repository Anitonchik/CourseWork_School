using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolDatabase.Implementations;
using SchoolDatabase.Models;
using SchoolDatabase;
using Microsoft.EntityFrameworkCore;

namespace Test.Tests;

internal class StorekeeperStorageContractTests : BaseStorageContractTests
{
    private StorekeeperStorageContract _storekeeperStorageContract;


    [SetUp]
    public void Setup()
    {
        _storekeeperStorageContract = new StorekeeperStorageContract(SchoolDbContext);
    }

    [TearDown]
    public void TearDown()
    {
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Storekeepers\" CASCADE; ");
    }


    [Test]
    public void TestAddmedal()
    {
        var storekeeper = new StorekeeperDataModel(Guid.NewGuid().ToString(), "fio", "login", "password", "mail");
        _storekeeperStorageContract.AddElement(storekeeper);

        var dbStorekeeper = _storekeeperStorageContract.GetElementById(storekeeper.Id);
        AssertElement(dbStorekeeper, storekeeper);
    }

    [Test]
    public void TestDeletemedal()
    {
        var storekeeper = new StorekeeperDataModel(Guid.NewGuid().ToString(), "fio", "login", "password", "mail");
        _storekeeperStorageContract.AddElement(storekeeper);
        _storekeeperStorageContract.DelElement(storekeeper.Id);

        Assert.That(() => _storekeeperStorageContract.GetElementById(storekeeper.Id), Throws.TypeOf<ElementNotFoundException>());
    }

    [Test]
    public void UpdateMedal()
    {
        var storekeeper = new StorekeeperDataModel(Guid.NewGuid().ToString(), "fio", "login", "password", "mail");
        _storekeeperStorageContract.AddElement(storekeeper);

        var newStorekeeper = new StorekeeperDataModel(storekeeper.Id, "new fio", "login", "password", "mail");
        _storekeeperStorageContract.UpdElement(newStorekeeper);

        AssertElement(_storekeeperStorageContract.GetElementById(storekeeper.Id), newStorekeeper);
    }

    [Test]
    public void GetListmedals()
    {
        var storekeeper1 = new StorekeeperDataModel(Guid.NewGuid().ToString(), "fio 1", "login 1", "password", "mail 1");
        var storekeeper2 = new StorekeeperDataModel(Guid.NewGuid().ToString(), "fio 2", "login 2", "password", "mail 2");
        var storekeeper3 = new StorekeeperDataModel(Guid.NewGuid().ToString(), "fio 3", "login 3", "password", "mail 3");

        _storekeeperStorageContract.AddElement(storekeeper1);
        _storekeeperStorageContract.AddElement(storekeeper2);
        _storekeeperStorageContract.AddElement(storekeeper3);

        var list = _storekeeperStorageContract.GetList();
        Assert.That(list.Count, Is.EqualTo(3));
    }

    private void AssertElement(StorekeeperDataModel actual, StorekeeperDataModel expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.Login, Is.EqualTo(expected.Login));
        Assert.That(actual.Mail, Is.EqualTo(expected.Mail));
        Assert.That(actual.Password, Is.EqualTo(expected.Password));
    }
}
