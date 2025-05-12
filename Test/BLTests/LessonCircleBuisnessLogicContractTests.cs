using Moq;
using SchoolBusinessLogic.Implementations;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;
using SchoolDatabase;
using Test.Tests;
using Test;
using Microsoft.Extensions.Logging;
using SchoolBuisnessLogic.Implementations;
using SchoolContracts.DataModels;
using SchoolDatabase.Implementations;

namespace SchoolTests.BLTests;

[TestFixture]
internal class LessonCircleBuisnessLogicContractTests : BaseStorageContractTests
{
    private ILessonCircleBuisnessLogicContract _lessonCirlceBuisnessLogicContract;
    private ICircleBuisnessLogicContract _cirlceBuisnessLogicContract;
    private Mock<ICircleStorageContract> _circleStorageContract;
    private Mock<ILessonCircleStorageContract> _lessonCircleStorageContract;
    private Storekeeper _storekeeper;
    Worker _worker;
    Circle _circle;
    Lesson _lesson;


    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _circleStorageContract = new Mock<ICircleStorageContract>();
        _lessonCircleStorageContract = new Mock<ILessonCircleStorageContract>();
        _cirlceBuisnessLogicContract = new CircleBuisnessLogicContract(_circleStorageContract.Object, new Mock<ILogger>().Object);

        _lessonCirlceBuisnessLogicContract = new LessonCircleBuisnessLogicContract(_lessonCircleStorageContract.Object, _cirlceBuisnessLogicContract, new Mock<ILogger>().Object);

        _storekeeper = SchoolDbContext.InsertAndReturnStorekeeper();
        _worker = SchoolDbContext.InsertAndReturnWorker();
        //_circle = SchoolDbContext.InsertAndReturnCircle(storekeeperId: _storekeeper.Id);
        _lesson = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id);
    }

    [TearDown]
    public void TearDown()
    {
        _circleStorageContract.Reset();
    }
    
    [Test]
    public void TestAddLessonCircle()
    {
        //Arrange
        var circle = new CircleDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "desc", [], []);
        _circleStorageContract.Setup(x => x.AddElement(It.IsAny<CircleDataModel>()));
        _cirlceBuisnessLogicContract.InsertCircle(_storekeeper.Id, circle);
        var record = new LessonCircleDataModel(_lesson.Id, circle.Id, 1);
        //Act
        //circle.Lessons.Add(record);
        _circleStorageContract.Setup(x => x.GetElementById(circle.Id)).Returns(circle);
        _circleStorageContract.Setup(x => x.UpdElement(It.IsAny<CircleDataModel>()));
        _lessonCirlceBuisnessLogicContract.CreateLessonCircle(_storekeeper.Id, circle, record);
        var rec = _cirlceBuisnessLogicContract.GetCircleByData(_storekeeper.Id, circle.Id);
        //Assert
        Assert.That(rec, Is.Not.Null);
        Assert.That(rec.Lessons.Count.Equals(1));
    }
    
    [Test]
    public void TestDeleteLessonCircle()
    {
        //Arrange
        var circleId = Guid.NewGuid().ToString();
        var lessonCircle = new LessonCircleDataModel(_lesson.Id, circleId, 2);

        var circle = new CircleDataModel(circleId, _storekeeper.Id, "name", "desc", [], [lessonCircle]);

        _circleStorageContract.Setup(x => x.AddElement(It.IsAny<CircleDataModel>()));
        _cirlceBuisnessLogicContract.InsertCircle(_storekeeper.Id, circle);

        //Act
        //circle.Lessons.Add(record);
        _circleStorageContract.Setup(x => x.GetElementById(circle.Id)).Returns(circle);
        var bef = _cirlceBuisnessLogicContract.GetCircleByData(_storekeeper.Id, circle.Id);
        Assert.That(bef.Lessons.Count.Equals(1));

        _circleStorageContract.Setup(x => x.GetElementById(circle.Id)).Returns(circle);
        _lessonCircleStorageContract.Setup(x => x.GetLessonCircleById(_lesson.Id, circleId)).Returns(lessonCircle);

        _lessonCirlceBuisnessLogicContract.DeleteLessonCircle(_storekeeper.Id, _lesson.Id, circleId);
        var rec = _cirlceBuisnessLogicContract.GetCircleByData(_storekeeper.Id, circle.Id);
        //Assert
        
        Assert.That(rec.Lessons.Count.Equals(0));
    }
}
