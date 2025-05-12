using Microsoft.EntityFrameworkCore;
using SchoolContracts.DataModels;
using SchoolDatabase.Implementations;
using SchoolDatabase.Models;
using Test;
using Test.Tests;

namespace Test.Tests;

[TestFixture]
internal class LessonCircleStorageContractTests : BaseStorageContractTests
{
    LessonCircleStorageContract _lessonCircleStorageContract;
    CircleStorageContract _circleStorageContract;
    LessonStorageContract _lessonStorageContract;

    Storekeeper _storekeeper;
    Worker _worker;
    Circle _circle;

    [SetUp]
    public void Setup()
    {
        _circleStorageContract = new CircleStorageContract(SchoolDbContext);
        _lessonStorageContract = new LessonStorageContract(SchoolDbContext);
        //_lessonCircleStorageContract = new LessonCircleStorageContract(SchoolDbContext, _circleStorageContract, _lessonStorageContract);

        _storekeeper = SchoolDbContext.InsertAndReturnStorekeeper();
        _worker = SchoolDbContext.InsertAndReturnWorker();
        _circle = SchoolDbContext.InsertAndReturnCircle(_storekeeper.Id);
    }

    [TearDown]
    public void TearDown()
    {
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Circles\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"LessonCircles\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Storekeepers\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Workers\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Lessons\" CASCADE; ");
    }

    [Test]
    public void TestAddLessonCircle()
    {
        /*_circle.LessonCircles.Add(new LessonCircleDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 1);
        _circleStorageContract.AddElement(circle);

        var dbCircle = _circleStorageContract.GetElementById(circle.Id);
        AssertElement(dbCircle, circle);*/
    }

}
