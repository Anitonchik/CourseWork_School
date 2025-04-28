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
    private Material _material;
    private Lesson _lesson;

    private Worker _worker;
    private Achievement _achievement;


    [SetUp]
    public void Setup()
    {
        _materialStorageContract = new MaterialStorageContract(SchoolDbContext);

        _storekeeper = SchoolDbContext.InsertAndReturnStorekeeper();
        _worker = SchoolDbContext.InsertAndReturnWorker();
        //_material = SchoolDbContext.InsertAndReturnMaterial(storekeeperId: _storekeeper.Id);
        _achievement = SchoolDbContext.InsertAndReturnAchievement(workerId: _worker.Id);
        //_lesson = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id, achievementId: _achievement.Id);
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

    [Test]
    public void GetMaterialsByLesson()
    {
        var circle1 = SchoolDbContext.InsertAndReturnCircle(storekeeperId: _storekeeper.Id, circleName: "name 1");
        var circle2 = SchoolDbContext.InsertAndReturnCircle(storekeeperId: _storekeeper.Id, circleName: "name 2");
        var circle3 = SchoolDbContext.InsertAndReturnCircle(storekeeperId: _storekeeper.Id, circleName: "name 3");

        var material1 = SchoolDbContext.InsertAndReturnMaterial(storekeeperId: _storekeeper.Id, materialName: "name 1");
        var material2 = SchoolDbContext.InsertAndReturnMaterial(storekeeperId: _storekeeper.Id, materialName: "name 2");
        var material3 = SchoolDbContext.InsertAndReturnMaterial(storekeeperId: _storekeeper.Id, materialName: "name 3");

        var circleMaterial1 = SchoolDbContext.InsertAndReturnCircleMaterial(circleId: circle1.Id, materialId: material3.Id);
        var circleMaterial2 = SchoolDbContext.InsertAndReturnCircleMaterial(circleId: circle2.Id, materialId: material2.Id);
        var circleMaterial3 = SchoolDbContext.InsertAndReturnCircleMaterial(circleId: circle3.Id, materialId: material1.Id);
        var circleMaterial4 = SchoolDbContext.InsertAndReturnCircleMaterial(circleId: circle2.Id, materialId: material3.Id);
        var circleMaterial5 = SchoolDbContext.InsertAndReturnCircleMaterial(circleId: circle3.Id, materialId: material2.Id);

        var lesson1 = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id, achievementId: _achievement.Id, lessonName: "name 1");
        var lesson2 = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id, achievementId: _achievement.Id, lessonName: "name 1");
        var lesson3 = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id, achievementId: _achievement.Id, lessonName: "name 1");

        var lessonCircle1 = SchoolDbContext.InsertAndReturnLessonCircle(lesson3.Id, circle2.Id);
        var lessonCircle2 = SchoolDbContext.InsertAndReturnLessonCircle(lesson3.Id, circle3.Id);
        var lessonCircle3 = SchoolDbContext.InsertAndReturnLessonCircle(lesson3.Id, circle1.Id);

        var list = _materialStorageContract.GetMaterialsByLesson(lesson3.Id);
        /*Assert.That(list.Count, Is.EqualTo(2));*/
    }

    private void AssertElement(MaterialDataModel actual, MaterialDataModel expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.MaterialName, Is.EqualTo(expected.MaterialName));
        Assert.That(actual.Description, Is.EqualTo(expected.Description));
        /*Assert.That(actual.Lessons.Count, Is.EqualTo(expected.Lessons.Count));*/
    }

    
}
