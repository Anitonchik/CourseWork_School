using Microsoft.EntityFrameworkCore;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolDatabase.Implementations;
using SchoolDatabase.Models;

namespace Test.Tests;

[TestFixture]
internal class MaterialStorageContractTests : BaseStorageContractTests
{
    private MaterialStorageContract _materialStorageContract;
    private Storekeeper _storekeeper;


    [SetUp]
    public void Setup()
    {
        _materialStorageContract = new MaterialStorageContract(SchoolDbContext);
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
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Materials\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Storekeepers\" CASCADE; ");
    }


    [Test]
    public void TestAddMaterial()
    {
        var material = new MaterialDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "nnnn");
        _materialStorageContract.AddElement(material);

        var dbMaterial = _materialStorageContract.GetElementById(material.Id);
        AssertElement(dbMaterial, material);
    }

    [Test]
    public void TestDeleteMaterial()
    {
        var material = new MaterialDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "nnnn");
        _materialStorageContract.AddElement(material);
        _materialStorageContract.DelElement(material.Id);

        Assert.That(() => _materialStorageContract.GetElementById(material.Id), Throws.TypeOf<ElementNotFoundException>());
    }

    [Test]
    public void UpdateMaterial()
    {
        var material = new MaterialDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "nnnn");
        _materialStorageContract.AddElement(material);

        var materialDataModel = new MaterialDataModel(material.Id, _storekeeper.Id, "new name", "new description");
        _materialStorageContract.UpdElement(materialDataModel);

        AssertElement(_materialStorageContract.GetElementById(material.Id), materialDataModel);
    }

    [Test]
    public void GetListMaterials()
    {

        var material1 = new MaterialDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name 1", "nnnn");
        var material2 = new MaterialDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name 2", "nnnn");
        var material3 = new MaterialDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name 3", "nnnn");

        _materialStorageContract.AddElement(material1);
        _materialStorageContract.AddElement(material2);
        _materialStorageContract.AddElement(material3);

        var list = _materialStorageContract.GetList();
        Assert.That(list.Count, Is.EqualTo(3));
    }

    private void AssertElement(MaterialDataModel actual, MaterialDataModel expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.MaterialName, Is.EqualTo(expected.MaterialName));
        Assert.That(actual.Description, Is.EqualTo(expected.Description));
        /*Assert.That(actual.Lessons.Count, Is.EqualTo(expected.Lessons.Count));*/
    }
}
