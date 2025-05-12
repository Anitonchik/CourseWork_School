using Moq;
using SchoolBuisnessLogic.Implementations;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;
using SchoolDatabase;
using Test.Tests;
using Test;
using Microsoft.Extensions.Logging;
using SchoolContracts.DataModels;
using SchoolDatabase.Implementations;
using SchoolTests.Infrastructure;


namespace SchoolTests.BLTests;
[TestFixture]
internal class LessonInterestBuisnessLogicContractTests : BaseStorageContractTests
{
    private ILessonInterestBuisnessLogicContract _lessonInterestBuisnessLogicContract;
    private ILessonBuisnessLogicContract _lessonBuisnessLogicContract;
    private Mock<ILessonStorageContract> _lessonStorageContract;
    private Mock<ILessonInterestStorageContract> _lessonInterestStorageContract;
    private Worker _worker;
    Storekeeper _storekeeper;
    Interest _interest;
    //Lesson _lesson;


    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _lessonStorageContract = new Mock<ILessonStorageContract>();
        _lessonInterestStorageContract = new Mock<ILessonInterestStorageContract>();
        _lessonBuisnessLogicContract = new LessonBuisnessLogicContract(_lessonStorageContract.Object, new Mock<ILogger>().Object);

        _lessonInterestBuisnessLogicContract = new LessonInterestBuisnessLogicContract(_lessonInterestStorageContract.Object, _lessonBuisnessLogicContract, new Mock<ILogger>().Object);

        _storekeeper = SchoolDbContext.InsertAndReturnStorekeeper();
        _worker = SchoolDbContext.InsertAndReturnWorker();
        //_lesson = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id);
        _interest = SchoolDbContext.InsertAndReturnInterest(workerId: _worker.Id);
    }

    [TearDown]
    public void TearDown()
    {
        _lessonStorageContract.Reset();
    }

    [Test]
    public void TestAddLessonInterest()
    {
        //Arrange
        var lesson = new LessonDataModel(Guid.NewGuid().ToString(), _worker.Id, "name",DateTime.UtcNow, "desc", []);
        _lessonStorageContract.Setup(x => x.AddElement(It.IsAny<LessonDataModel>()));
        _lessonBuisnessLogicContract.InsertLesson(_worker.Id, lesson);
        var record = new LessonInterestDataModel(lesson.Id, _interest.Id, "name");
        //Act
        //circle.Lessons.Add(record);
        _lessonStorageContract.Setup(x => x.GetElementById(lesson.Id)).Returns(lesson);
        _lessonStorageContract.Setup(x => x.UpdElement(It.IsAny<LessonDataModel>()));
        _lessonInterestBuisnessLogicContract.CreateLessonInterest(_worker.Id, lesson, record);
        var rec = _lessonBuisnessLogicContract.GetLessonByData(_worker.Id, lesson.Id);
        //Assert
        Assert.That(rec, Is.Not.Null);
        Assert.That(rec.Interests.Count.Equals(1));
    }

    [Test]
    public void TestDeleteLessonCircle()
    {
        //Arrange
        var lessonId = Guid.NewGuid().ToString();
        var lessonInterest = new LessonInterestDataModel(lessonId,_interest.Id, "name");

        var lesson = new LessonDataModel(lessonId, _worker.Id, "name", DateTime.UtcNow,"desc",  [lessonInterest]);

        _lessonStorageContract.Setup(x => x.AddElement(It.IsAny<LessonDataModel>()));
        _lessonBuisnessLogicContract.InsertLesson(_worker.Id, lesson);

        //Act
        //circle.Lessons.Add(record);
        _lessonStorageContract.Setup(x => x.GetElementById(lesson.Id)).Returns(lesson);
        var bef = _lessonBuisnessLogicContract.GetLessonByData(_worker.Id, lesson.Id);
        Assert.That(bef.Interests.Count.Equals(1));

        _lessonStorageContract.Setup(x => x.GetElementById(lesson.Id)).Returns(lesson);
        _lessonInterestStorageContract.Setup(x => x.GetLessonInterestById(lessonId,_interest.Id )).Returns(lessonInterest);

        _lessonInterestBuisnessLogicContract.DeleteLessonInterest(_worker.Id, lessonId, _interest.Id);
        var rec = _lessonBuisnessLogicContract.GetLessonByData(_worker.Id, lesson.Id);
        //Assert

        Assert.That(rec.Interests.Count.Equals(0));
    }
}
