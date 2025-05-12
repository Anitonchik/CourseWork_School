using Microsoft.Extensions.Logging;
using Moq;
using SchoolBuisnessLogic.Implementations;
using SchoolBusinessLogic.Implementations;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Implementations;
using SchoolDatabase.Models;
using SchoolTests.Infrastructure;
using System.ComponentModel.DataAnnotations;
using Test.Tests;
using UnauthorizedAccessException = SchoolContracts.Exceptions.UnauthorizedAccessException;

namespace Test.BLTests;

internal class LessonBuisnessLogicTest : BaseStorageContractTests
{
    private ILessonBuisnessLogicContract _lessonBuisnessLogicContract;
    private Mock<ILessonStorageContract> _lessonStorageContract;
    private Worker _worker;


    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _lessonStorageContract = new Mock<ILessonStorageContract>();
        _lessonBuisnessLogicContract = new LessonBuisnessLogicContract(_lessonStorageContract.Object, new Mock<ILogger>().Object);

        _worker = SchoolDbContext.InsertAndReturnWorker();
    }

    [TearDown]
    public void TearDown()
    {
        _lessonStorageContract.Reset();
    }

    [Test]
    public void GetAllLessons_ReturnListOfRecords_Test()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var listOriginal = new List<LessonDataModel>()
        {
            new(id, _worker.Id, "name 1", "desc",[]),
            new(Guid.NewGuid().ToString(), _worker.Id, "name 2", "desc",[]),
            new(Guid.NewGuid().ToString(), _worker.Id, "name 3", "desc",[]),
        };

        _lessonStorageContract.Setup(x => x.GetList(_worker.Id)).Returns(listOriginal);
        //Act
        var list = _lessonBuisnessLogicContract.GetAllLessons(_worker.Id);
        //Assert
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Is.EquivalentTo(listOriginal));
    }

    [Test]
    public void GetAllLessons_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var workerId = Guid.NewGuid().ToString();
        SchoolDbContext.InsertAndReturnWorker(workerId, login: "login 1", password: "psw 1", mail: "mail 1");

        var id = Guid.NewGuid().ToString();
        var listOriginal = new List<LessonDataModel>()
        {
            new(id, workerId, "name 1", "desc",[]),
            new(Guid.NewGuid().ToString(), workerId, "name 2", "desc",[]),
            new(Guid.NewGuid().ToString(), workerId, "name 3", "desc",[]),
        };

        _lessonStorageContract.Setup(x => x.GetList(workerId)).Returns(listOriginal);
        //Act&Assert
        Assert.That(() => _lessonBuisnessLogicContract.GetAllLessons(_worker.Id), Throws.TypeOf<NullListException>());
    }

    [Test]
    public void GetLessonByData_GetById_ReturnRecord_Test()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var record = new LessonDataModel(id, _worker.Id, "name 2", "desc", []);
        _lessonStorageContract.Setup(x => x.GetElementById(id)).Returns(record);
        //Act
        var element = _lessonBuisnessLogicContract.GetLessonByData(_worker.Id, id);
        //Assert
        Assert.That(element, Is.Not.Null);
        Assert.That(element.Id, Is.EqualTo(id));
        _lessonStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetLessonByData_GetByName_ReturnRecord_Test()
    {
        //Arrange
        var name = "new name";
        var record = new LessonDataModel(Guid.NewGuid().ToString(), _worker.Id, name, "desc", []);
        _lessonStorageContract.Setup(x => x.GetElementByName(name)).Returns(record);
        //Act
        var element = _lessonBuisnessLogicContract.GetLessonByData(_worker.Id, name);
        //Assert
        Assert.That(element, Is.Not.Null);
        Assert.That(element.LessonName, Is.EqualTo(name));
        _lessonStorageContract.Verify(x => x.GetElementByName(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetLessonByData_EmptyData_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _lessonBuisnessLogicContract.GetLessonByData(_worker.Id, null), Throws.TypeOf<ArgumentNullException>());
        Assert.That(() => _lessonBuisnessLogicContract.GetLessonByData(_worker.Id, string.Empty), Throws.TypeOf<ArgumentNullException>());
        _lessonStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Never);
        _lessonStorageContract.Verify(x => x.GetElementByName(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetLessonByData_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var workerId = Guid.NewGuid().ToString();
        var record = new LessonDataModel(Guid.NewGuid().ToString(), workerId, "name", "desc", []);
        _lessonStorageContract.Setup(x => x.GetElementByName("name")).Returns(record);

        //Act&Assert
        Assert.That(() => _lessonBuisnessLogicContract.GetLessonByData(_worker.Id, "name"), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void InsertLesson_CorrectRecord_Test()
    {
        //Arrange
        var flag = false;
        var record = new LessonDataModel(Guid.NewGuid().ToString(), _worker.Id, "name", "desc", []);
        _lessonStorageContract.Setup(x => x.AddElement(It.IsAny<LessonDataModel>()))
            .Callback((LessonDataModel x) =>
            {
                flag = x.Id == record.Id && x.LessonName == record.LessonName && x.Description == record.Description;
            });
        //Act
        _lessonBuisnessLogicContract.InsertLesson(_worker.Id, record);
        //Assert
        _lessonStorageContract.Verify(x => x.AddElement(It.IsAny<LessonDataModel>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void InsertLesson_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var workerId = Guid.NewGuid().ToString();
        var record = new LessonDataModel(Guid.NewGuid().ToString(), workerId, "name", "desc", []);
        _lessonStorageContract.Setup(x => x.AddElement(It.IsAny<LessonDataModel>()));

        //Act&
        Assert.That(() => _lessonBuisnessLogicContract.InsertLesson(_worker.Id, record), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void InsertLesson_RecordWithExistsData_ThrowException_Test()
    {
        //Arrange
        _lessonStorageContract.Setup(x => x.AddElement(It.IsAny<LessonDataModel>())).Throws(new ElementExistsException("Data", "Data"));
        //Act&Assert
        Assert.That(() => _lessonBuisnessLogicContract.InsertLesson(_worker.Id, new(Guid.NewGuid().ToString(), _worker.Id, "name", "desc", [])),
            Throws.TypeOf<ElementExistsException>());
        _lessonStorageContract.Verify(x => x.AddElement(It.IsAny<LessonDataModel>()), Times.Once);
    }
    [Test]
    public void InsertLesson_NullRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _lessonBuisnessLogicContract.InsertLesson(_worker.Id, null),
        Throws.TypeOf<ArgumentNullException>());
        _lessonStorageContract.Verify(x => x.AddElement(It.IsAny<LessonDataModel>()), Times.Never);
    }
    [Test]
    public void InsertLesson_InvalidRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _lessonBuisnessLogicContract.InsertLesson(_worker.Id, new LessonDataModel("id", _worker.Id, "name", "desc", [])), Throws.TypeOf<ValidationException>());
        _lessonStorageContract.Verify(x => x.AddElement(It.IsAny<LessonDataModel>()), Times.Never);
    }

    [Test]
    public void UpdateLesson_CorrectRecord_Test()
    {
        //Arrange
        var flag = false;
        var record = new LessonDataModel(Guid.NewGuid().ToString(), _worker.Id, "name", "desc", []);
        _lessonStorageContract.Setup(x => x.UpdElement(It.IsAny<LessonDataModel>()))
        .Callback((LessonDataModel x) =>
        {
            flag = x.Id == record.Id && x.LessonName == record.LessonName && x.Description == record.Description;
        });
        //Act
        _lessonBuisnessLogicContract.UpdateLesson(_worker.Id, record);
        //Assert
        _lessonStorageContract.Verify(x => x.UpdElement(It.IsAny<LessonDataModel>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void UpdateLesson_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var workerId = Guid.NewGuid().ToString();
        var record = new LessonDataModel(Guid.NewGuid().ToString(), workerId, "name", "desc", []);
        _lessonStorageContract.Setup(x => x.UpdElement(It.IsAny<LessonDataModel>()));

        //Act&
        Assert.That(() => _lessonBuisnessLogicContract.UpdateLesson(_worker.Id, record), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void UpdateLesson_RecordWithIncorrectData_ThrowException_Test()
    {
        //Arrange
        _lessonStorageContract.Setup(x => x.UpdElement(It.IsAny<LessonDataModel>())).Throws(new ElementNotFoundException(""));
        //Act&Assert
        Assert.That(() => _lessonBuisnessLogicContract.UpdateLesson(_worker.Id, new(Guid.NewGuid().ToString(), _worker.Id, "name", "desc", [])), Throws.TypeOf<ElementNotFoundException>());
        _lessonStorageContract.Verify(x => x.UpdElement(It.IsAny<LessonDataModel>()), Times.Once);
    }

    [Test]
    public void UpdateLesson_RecordWithExistsData_ThrowException_Test()
    {
        //Arrange
        _lessonStorageContract.Setup(x => x.UpdElement(It.IsAny<LessonDataModel>())).Throws(new ElementExistsException("Data", "Data"));
        //Act&Assert
        Assert.That(() => _lessonBuisnessLogicContract.UpdateLesson(_worker.Id, new(Guid.NewGuid().ToString(), _worker.Id, "name", "desc", [])), Throws.TypeOf<ElementExistsException>());
        _lessonStorageContract.Verify(x => x.UpdElement(It.IsAny<LessonDataModel>()), Times.Once);
    }
    [Test]
    public void UpdateLesson_NullRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _lessonBuisnessLogicContract.UpdateLesson(_worker.Id, null), Throws.TypeOf<ArgumentNullException>());
        _lessonStorageContract.Verify(x => x.UpdElement(It.IsAny<LessonDataModel>()), Times.Never);
    }
    [Test]
    public void UpdateLesson_InvalidRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _lessonBuisnessLogicContract.UpdateLesson(_worker.Id, new LessonDataModel("id", _worker.Id, "name", "desc", [])),
        Throws.TypeOf<ValidationException>());
        _lessonStorageContract.Verify(x => x.UpdElement(It.IsAny<LessonDataModel>()), Times.Never);
    }

    [Test]
    public void DeleteLesson_CorrectRecord_Test()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var flag = false;
        _lessonStorageContract.Setup(x => x.DelElement(It.Is((string x) => x == id))).Callback(() => { flag = true; });
        //Act
        _lessonBuisnessLogicContract.DeleteLesson(_worker.Id, id);
        //Assert
        _lessonStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void DeleteLesson_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        var id = Guid.NewGuid().ToString();
        var record = new LessonDataModel(id, storekeeperId, "name", "desc", []);
        _lessonStorageContract.Setup(x => x.GetElementById(id)).Returns(record);
        _lessonStorageContract.Setup(x => x.UpdElement(It.IsAny<LessonDataModel>()));

        //Act&
        Assert.That(() => _lessonBuisnessLogicContract.DeleteLesson(_worker.Id, id), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void DeleteLesson_RecordWithIncorrectId_ThrowException_Test()
    {
        //Arrange
        _lessonStorageContract.Setup(x => x.DelElement(It.IsAny<string>())).Throws(new ElementNotFoundException(""));
        //Act&Assert
        Assert.That(() => _lessonBuisnessLogicContract.DeleteLesson(_worker.Id, Guid.NewGuid().ToString()), Throws.TypeOf<ElementNotFoundException>());
        _lessonStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Once);
    }
    [Test]
    public void DeleteLesson_IdIsNullOrEmpty_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _lessonBuisnessLogicContract.DeleteLesson(_worker.Id, null), Throws.TypeOf<ArgumentNullException>());
        Assert.That(() => _lessonBuisnessLogicContract.DeleteLesson(_worker.Id, string.Empty),
        Throws.TypeOf<ArgumentNullException>());
        _lessonStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Never);
    }
    [Test]
    public void DeleteLesson_IdIsNotGuid_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _lessonBuisnessLogicContract.DeleteLesson(_worker.Id, "id"), Throws.TypeOf<ValidationException>());
        _lessonStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Never);
    }
}
