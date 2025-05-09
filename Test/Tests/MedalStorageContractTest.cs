using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolDatabase.Implementations;
using SchoolDatabase.Models;
using SchoolDatabase;
using Microsoft.EntityFrameworkCore;
using SchoolContracts.StoragesContracts;

namespace Test.Tests;

[TestFixture]
internal class MedalStorageContractTest : BaseStorageContractTests
{
    private MedalStorageContract _medalStorageContract;
    private MaterialStorageContract _materialStorageContract;
    private Storekeeper _storekeeper;
    private Material _material;


    [SetUp]
    public void Setup()
    {
        _materialStorageContract = new MaterialStorageContract(SchoolDbContext);
        _medalStorageContract = new MedalStorageContract(SchoolDbContext);

        _storekeeper = new Storekeeper()
        {
            Id = Guid.NewGuid().ToString(),
            FIO = "fio",
            Login = "login",
            Password = "password",
            Mail = "mail"
        };
        SchoolDbContext.Storekeepers.Add(_storekeeper);

        _material = new Material()
        {
            Id = Guid.NewGuid().ToString(),
            StorekeeperId = _storekeeper.Id,
            MaterialName = "name",
            Description = "description"
        };
        SchoolDbContext.Materials.Add(_material);
    }

    [TearDown]
    public void TearDown()
    {
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Medals\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Storekeepers\" CASCADE; ");
    }


    [Test]
    public void TestAddmedal()
    {
        var medal = new MedalDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, _material.Id, "name", 1, "nnnn");
        _medalStorageContract.AddElement(medal);

        var dbMedal = _medalStorageContract.GetElementById(medal.Id);
        AssertElement(dbMedal, medal);
    }

    [Test]
    public void TestDeletemedal()
    {
        var medal = new MedalDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, _material.Id, "name", 1, "nnnn");
        _medalStorageContract.AddElement(medal);
        _medalStorageContract.DelElement(medal.Id);

        Assert.That(() => _medalStorageContract.GetElementById(medal.Id), Throws.TypeOf<ElementNotFoundException>());
    }

    [Test]
    public void UpdateMedal()
    {
        var medal = new MedalDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, _material.Id, "name", 1, "nnnn");
        _medalStorageContract.AddElement(medal);

        var MedalDataModel = new MedalDataModel(medal.Id, _storekeeper.Id, _material.Id, "new name", 2, "nnnn");
        _medalStorageContract.UpdElement(MedalDataModel);

        AssertElement(_medalStorageContract.GetElementById(medal.Id), MedalDataModel);
    }

    [Test]
    public void GetListmedals()
    {

        var medal1 = new MedalDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, _material.Id, "name 1", 1, "nnnn");
        var medal2 = new MedalDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, _material.Id, "name 2", 1, "nnnn");
        var medal3 = new MedalDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, _material.Id, "name 3", 1, "nnnn");

        _medalStorageContract.AddElement(medal1);
        _medalStorageContract.AddElement(medal2);
        _medalStorageContract.AddElement(medal3);

        var list = _medalStorageContract.GetList(_storekeeper.Id, null);
        Assert.That(list.Count, Is.EqualTo(3));
    }

    private void AssertElement(MedalDataModel actual, MedalDataModel expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.StorekeeperId, Is.EqualTo(expected.StorekeeperId));
        Assert.That(actual.MaterialId, Is.EqualTo(expected.MaterialId));
        Assert.That(actual.MedalName, Is.EqualTo(expected.MedalName));
        Assert.That(actual.Range, Is.EqualTo(expected.Range));
        Assert.That(actual.Description, Is.EqualTo(expected.Description));
    }
}
