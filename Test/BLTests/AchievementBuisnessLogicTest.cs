using Microsoft.Extensions.Logging;
using Moq;
using SchoolBusinessLogic.Implementations;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;
using SchoolDatabase;
using Test.Tests;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using System.ComponentModel.DataAnnotations;
using UnauthorizedAccessException = SchoolContracts.Exceptions.UnauthorizedAccessException;
using SchoolBuisnessLogic.Implementations;

namespace Test.BLTests;

[TestFixture]
internal class AchievementBuisnessLogicTest : BaseStorageContractTests
{
    private IAchievementBuisnessLogicContract _achievementBuisnessLogicContract;
    private Mock<IAchievementStorageContract> _achievementStorageContract;
    private Worker _worker;
    private Lesson _lesson;


    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _achievementStorageContract = new Mock<IAchievementStorageContract>();
       // _achievementBuisnessLogicContract = new AchievementBuisnessLogicContract(_achievementStorageContract.Object, new Mock<ILogger>().Object);

        _worker = SchoolDbContext.InsertAndReturnWorker();
        _lesson = SchoolDbContext.InsertAndReturnLesson(workerId: _worker.Id);
    }

    [TearDown]
    public void TearDown()
    {
        _achievementStorageContract.Reset();
    }

    [Test]
    public void GetAllAchievements_ReturnListOfRecords_Test()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var listOriginal = new List<AchievementDataModel>()
        {
            new(id, _worker.Id, _lesson.Id, "name 1",  "desc"),
            new(Guid.NewGuid().ToString(), _worker.Id, _lesson.Id, "name 2",  "desc"),
            new(Guid.NewGuid().ToString(), _worker.Id, _lesson.Id, "name 3",  "desc"),
        };

        _achievementStorageContract.Setup(x => x.GetList(_worker.Id)).Returns(listOriginal);
        //Act
        var list = _achievementBuisnessLogicContract.GetAllAchievements(_worker.Id);
        //Assert
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Is.EquivalentTo(listOriginal));
    }

    [Test]
    public void GetAllAchievements_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var workerId = Guid.NewGuid().ToString();
        SchoolDbContext.InsertAndReturnStorekeeper(workerId, login: "login 1", password: "psw 1", mail: "mail 1");

        var id = Guid.NewGuid().ToString();
        var listOriginal = new List<AchievementDataModel>()
        {
            new(id, workerId, _lesson.Id, "name 1", "desc"),
            new(Guid.NewGuid().ToString(), workerId, _lesson.Id, "name 2",  "desc"),
            new(Guid.NewGuid().ToString(), workerId, _lesson.Id, "name 3",  "desc"),
        };

        _achievementStorageContract.Setup(x => x.GetList(workerId)).Returns(listOriginal);
        //Act&Assert
        Assert.That(() => _achievementBuisnessLogicContract.GetAllAchievements(_worker.Id), Throws.TypeOf<NullListException>());
    }

    [Test]
    public void GetAchievementById_ReturnRecord_Test()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var record = new AchievementDataModel(id, _worker.Id, _lesson.Id, "name 1",  "desc");
        _achievementStorageContract.Setup(x => x.GetElementById(id)).Returns(record);
        //Act
        var element = _achievementBuisnessLogicContract.GetAchievementById(_worker.Id, id);
        //Assert
        Assert.That(element, Is.Not.Null);
        Assert.That(element.Id, Is.EqualTo(id));
        _achievementStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Once);
    }


    [Test]
    public void GetAchievementById_EmptyData_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _achievementBuisnessLogicContract.GetAchievementById(_worker.Id, null), Throws.TypeOf<ArgumentNullException>());
        Assert.That(() => _achievementBuisnessLogicContract.GetAchievementById(_worker.Id, string.Empty), Throws.TypeOf<ArgumentNullException>());
        _achievementStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Never);
        _achievementStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetAchievementById_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var workerId = Guid.NewGuid().ToString();
        var record = new AchievementDataModel(Guid.NewGuid().ToString(), workerId, _lesson.Id, "name 1",  "desc");
        _achievementStorageContract.Setup(x => x.GetElementById(record.Id)).Returns(record);

        //Act&Assert
        Assert.That(() => _achievementBuisnessLogicContract.GetAchievementById(_worker.Id, "name"), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void InsertAchievement_CorrectRecord_Test()
    {
        //Arrange
        var flag = false;
        var record = new AchievementDataModel(Guid.NewGuid().ToString(), _worker.Id, _lesson.Id, "name 1",  "desc");
        _achievementStorageContract.Setup(x => x.AddElement(It.IsAny<AchievementDataModel>()))
            .Callback((AchievementDataModel x) =>
            {
                flag = x.Id == record.Id && x.AchievementName == record.AchievementName && x.Description == record.Description ;
            });
        //Act
        _achievementBuisnessLogicContract.InsertAchievement(_worker.Id, record);
        //Assert
        _achievementStorageContract.Verify(x => x.AddElement(It.IsAny<AchievementDataModel>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void InsertAchievement_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var workerId = Guid.NewGuid().ToString();
        var record = new AchievementDataModel(Guid.NewGuid().ToString(), workerId, _lesson.Id, "name 1",  "desc");
        _achievementStorageContract.Setup(x => x.AddElement(It.IsAny<AchievementDataModel>()));

        //Act&
        Assert.That(() => _achievementBuisnessLogicContract.InsertAchievement(_worker.Id, record), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void InsertAchievement_RecordWithExistsData_ThrowException_Test()
    {
        //Arrange
        _achievementStorageContract.Setup(x => x.AddElement(It.IsAny<AchievementDataModel>())).Throws(new ElementExistsException("Data", "Data"));
        //Act&Assert
        Assert.That(() => _achievementBuisnessLogicContract.InsertAchievement(_worker.Id, new(Guid.NewGuid().ToString(), _worker.Id, _lesson.Id, "name 1",  "desc")),
            Throws.TypeOf<ElementExistsException>());
        _achievementStorageContract.Verify(x => x.AddElement(It.IsAny<AchievementDataModel>()), Times.Once);
    }
    [Test]
    public void InsertAchievement_NullRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _achievementBuisnessLogicContract.InsertAchievement(_worker.Id, null),
        Throws.TypeOf<ArgumentNullException>());
        _achievementStorageContract.Verify(x => x.AddElement(It.IsAny<AchievementDataModel>()), Times.Never);
    }
    [Test]
    public void InsertAchievement_InvalidRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _achievementBuisnessLogicContract.InsertAchievement(_worker.Id, new AchievementDataModel("id", _worker.Id, _lesson.Id, "name 1",  "desc")), Throws.TypeOf<ValidationException>());
        _achievementStorageContract.Verify(x => x.AddElement(It.IsAny<AchievementDataModel>()), Times.Never);
    }

    [Test]
    public void UpdateAchievement_CorrectRecord_Test()
    {
        //Arrange
        var flag = false;
        var record = new AchievementDataModel(Guid.NewGuid().ToString(), _worker.Id, _lesson.Id, "name 1",  "desc");
        _achievementStorageContract.Setup(x => x.UpdElement(It.IsAny<AchievementDataModel>()))
        .Callback((AchievementDataModel x) =>
        {
            flag = x.Id == record.Id && x.AchievementName == record.AchievementName && x.Description == record.Description ;
        });
        //Act
        _achievementBuisnessLogicContract.UpdateAchievement(_worker.Id, record);
        //Assert
        _achievementStorageContract.Verify(x => x.UpdElement(It.IsAny<AchievementDataModel>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void UpdateAchievement_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var workerId = Guid.NewGuid().ToString();
        var record = new AchievementDataModel(Guid.NewGuid().ToString(), workerId, _lesson.Id, "name 1",  "desc");
        _achievementStorageContract.Setup(x => x.UpdElement(It.IsAny<AchievementDataModel>()));

        //Act&
        Assert.That(() => _achievementBuisnessLogicContract.UpdateAchievement(_worker.Id, record), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void UpdateAchievement_RecordWithIncorrectData_ThrowException_Test()
    {
        //Arrange
        _achievementStorageContract.Setup(x => x.UpdElement(It.IsAny<AchievementDataModel>())).Throws(new ElementNotFoundException(""));
        //Act&Assert
        Assert.That(() => _achievementBuisnessLogicContract.UpdateAchievement(_worker.Id, new(Guid.NewGuid().ToString(), _worker.Id, _lesson.Id, "name 1",  "desc")), Throws.TypeOf<ElementNotFoundException>());
        _achievementStorageContract.Verify(x => x.UpdElement(It.IsAny<AchievementDataModel>()), Times.Once);
    }

    [Test]
    public void UpdateAchievement_RecordWithExistsData_ThrowException_Test()
    {
        //Arrange
        _achievementStorageContract.Setup(x => x.UpdElement(It.IsAny<AchievementDataModel>())).Throws(new ElementExistsException("Data", "Data"));
        //Act&Assert
        Assert.That(() => _achievementBuisnessLogicContract.UpdateAchievement(_worker.Id, new(Guid.NewGuid().ToString(), _worker.Id, _lesson.Id, "name 1",  "desc")), Throws.TypeOf<ElementExistsException>());
        _achievementStorageContract.Verify(x => x.UpdElement(It.IsAny<AchievementDataModel>()), Times.Once);
    }
    [Test]
    public void UpdateAchievement_NullRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _achievementBuisnessLogicContract.UpdateAchievement(_worker.Id, null), Throws.TypeOf<ArgumentNullException>());
        _achievementStorageContract.Verify(x => x.UpdElement(It.IsAny<AchievementDataModel>()), Times.Never);
    }
    [Test]
    public void UpdateAchievement_InvalidRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _achievementBuisnessLogicContract.UpdateAchievement(_worker.Id, new AchievementDataModel("id", _worker.Id, _lesson.Id, "name 1",  "desc")),
        Throws.TypeOf<ValidationException>());
        _achievementStorageContract.Verify(x => x.UpdElement(It.IsAny<AchievementDataModel>()), Times.Never);
    }

    [Test]
    public void DeleteAchievement_CorrectRecord_Test()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var flag = false;
        _achievementStorageContract.Setup(x => x.DelElement(It.Is((string x) => x == id))).Callback(() => { flag = true; });
        //Act
        _achievementBuisnessLogicContract.DeleteAchievement(_worker.Id, id);
        //Assert
        _achievementStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void DeleteAchievement_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var workerId = Guid.NewGuid().ToString();
        var id = Guid.NewGuid().ToString();
        var record = new AchievementDataModel(id, workerId, _lesson.Id, "name 1",  "desc");
        _achievementStorageContract.Setup(x => x.GetElementById(id)).Returns(record);
        _achievementStorageContract.Setup(x => x.UpdElement(It.IsAny<AchievementDataModel>()));

        //Act&
        Assert.That(() => _achievementBuisnessLogicContract.DeleteAchievement(_worker.Id, id), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void DeleteAchievement_RecordWithIncorrectId_ThrowException_Test()
    {
        //Arrange
        var workerId = Guid.NewGuid().ToString();
        var id = Guid.NewGuid().ToString();
        var record = new AchievementDataModel(id, workerId, _lesson.Id, "name 1",  "desc");
        _achievementStorageContract.Setup(x => x.GetElementById(id)).Returns(record);
        _achievementStorageContract.Setup(x => x.DelElement(It.IsAny<string>())).Throws(new ElementNotFoundException(""));
        //Act&Assert
        Assert.That(() => _achievementBuisnessLogicContract.DeleteAchievement(_worker.Id, Guid.NewGuid().ToString()), Throws.TypeOf<ElementNotFoundException>());
        //_medalStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Once);
    }
    [Test]
    public void DeleteAchievement_IdIsNullOrEmpty_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _achievementBuisnessLogicContract.DeleteAchievement(_worker.Id, null), Throws.TypeOf<ArgumentNullException>());
        Assert.That(() => _achievementBuisnessLogicContract.DeleteAchievement(_worker.Id, string.Empty),
        Throws.TypeOf<ArgumentNullException>());
        _achievementStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Never);
    }
    [Test]
    public void DeleteAchievement_IdIsNotGuid_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _achievementBuisnessLogicContract.DeleteAchievement(_worker.Id, "id"), Throws.TypeOf<ValidationException>());
        _achievementStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Never);
    }
}
