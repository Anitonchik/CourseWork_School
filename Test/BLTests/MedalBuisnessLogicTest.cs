using Microsoft.Extensions.Logging;
using Moq;
using SchoolBusinessLogic.Implementations;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;
using System.ComponentModel.DataAnnotations;
using Test.Tests;
using UnauthorizedAccessException = SchoolContracts.Exceptions.UnauthorizedAccessException;

namespace Test.BLTests;

[TestFixture]
internal class MedalBuisnessLogicTest : BaseStorageContractTests
{
    private IMedalBuisnessLogicContract _medalBuisnessLogicContract;
    private Mock<IMedalStorageContract> _medalStorageContract;
    private Storekeeper _storekeeper;
    private Material _material;


    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _medalStorageContract = new Mock<IMedalStorageContract>();
        _medalBuisnessLogicContract = new MedalBuisnessLogicContract(_medalStorageContract.Object, new Mock<ILogger>().Object);

        _storekeeper = SchoolDbContext.InsertAndReturnStorekeeper();
        _material = SchoolDbContext.InsertAndReturnMaterial(storekeeperId: _storekeeper.Id);
    }

    [TearDown]
    public void TearDown()
    {
        _medalStorageContract.Reset();
    }

    [Test]
    public void GetAllMedals_ReturnListOfRecords_Test()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var listOriginal = new List<MedalDataModel>()
        {
            new(id, _storekeeper.Id, _material.Id, "name 1", 1, "desc"),
            new(Guid.NewGuid().ToString(), _storekeeper.Id, _material.Id, "name 2", 2, "desc"),
            new(Guid.NewGuid().ToString(), _storekeeper.Id, _material.Id, "name 3", 3, "desc"),
        };

        _medalStorageContract.Setup(x => x.GetList(_storekeeper.Id, null)).Returns(listOriginal);
        //Act
        var list = _medalBuisnessLogicContract.GetAllMedals(_storekeeper.Id);
        //Assert
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Is.EquivalentTo(listOriginal));
    }

    [Test]
    public void GetAllMedals_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        SchoolDbContext.InsertAndReturnStorekeeper(storekeeperId, login: "login 1", password: "psw 1", mail: "mail 1");

        var id = Guid.NewGuid().ToString();
        var listOriginal = new List<MedalDataModel>()
        {
            new(id, storekeeperId, _material.Id, "name 1", 1, "desc"),
            new(Guid.NewGuid().ToString(), storekeeperId, _material.Id, "name 2", 2, "desc"),
            new(Guid.NewGuid().ToString(), storekeeperId, _material.Id, "name 3", 3, "desc"),
        };

        _medalStorageContract.Setup(x => x.GetList(storekeeperId, null)).Returns(listOriginal);
        //Act&Assert
        Assert.That(() => _medalBuisnessLogicContract.GetAllMedals(_storekeeper.Id), Throws.TypeOf<NullListException>());
    }

    [Test]
    public void GetMedalsByRange_ReturnListOfRecords_Test()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var listOriginal = new List<MedalDataModel>()
        {
            new(id, _storekeeper.Id, _material.Id, "name 1", 1, "desc"),
            new(Guid.NewGuid().ToString(), _storekeeper.Id, _material.Id, "name 2", 2, "desc"),
            new(Guid.NewGuid().ToString(), _storekeeper.Id, _material.Id, "name 3", 1, "desc"),
            new(Guid.NewGuid().ToString(), _storekeeper.Id, _material.Id, "name 3", 3, "desc"),
            new(Guid.NewGuid().ToString(), _storekeeper.Id, _material.Id, "name 3", 1, "desc"),
        };

        _medalStorageContract.Setup(x => x.GetList(_storekeeper.Id, 1)).Returns(listOriginal);
        //Act
        var list = _medalBuisnessLogicContract.GetMedalsByRange(_storekeeper.Id, 1);
        //Assert
        Assert.That(list, Is.Not.Null);
        Assert.That(list.Count.Equals(3));
    }

    [Test]
    public void GetMedalsByRange_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        SchoolDbContext.InsertAndReturnStorekeeper(storekeeperId, login: "login 1", password: "psw 1", mail: "mail 1");

        var id = Guid.NewGuid().ToString();
        var listOriginal = new List<MedalDataModel>()
        {
            new(id, storekeeperId, _material.Id, "name 1", 1, "desc"),
            new(Guid.NewGuid().ToString(), storekeeperId, _material.Id, "name 2", 2, "desc"),
            new(Guid.NewGuid().ToString(), storekeeperId, _material.Id, "name 3", 1, "desc"),
            new(Guid.NewGuid().ToString(), storekeeperId, _material.Id, "name 3", 3, "desc"),
            new(Guid.NewGuid().ToString(), storekeeperId, _material.Id, "name 3", 1, "desc"),
        };

        _medalStorageContract.Setup(x => x.GetList(storekeeperId, null)).Returns(listOriginal);
        //Act&Assert
        Assert.That(() => _medalBuisnessLogicContract.GetAllMedals(_storekeeper.Id), Throws.TypeOf<NullListException>());
    }

    [Test]
    public void GetMedalById_ReturnRecord_Test()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var record = new MedalDataModel(id, _storekeeper.Id, _material.Id, "name 1", 1, "desc");
        _medalStorageContract.Setup(x => x.GetElementById(id)).Returns(record);
        //Act
        var element = _medalBuisnessLogicContract.GetMedalById(_storekeeper.Id, id);
        //Assert
        Assert.That(element, Is.Not.Null);
        Assert.That(element.Id, Is.EqualTo(id));
        _medalStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Once);
    }


    [Test]
    public void GetMedalById_EmptyData_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _medalBuisnessLogicContract.GetMedalById(_storekeeper.Id, null), Throws.TypeOf<ArgumentNullException>());
        Assert.That(() => _medalBuisnessLogicContract.GetMedalById(_storekeeper.Id, string.Empty), Throws.TypeOf<ArgumentNullException>());
        _medalStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Never);
        _medalStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetMedalById_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        var record = new MedalDataModel(Guid.NewGuid().ToString(), storekeeperId, _material.Id, "name 1", 1, "desc");
        _medalStorageContract.Setup(x => x.GetElementById(record.Id)).Returns(record);

        //Act&Assert
        Assert.That(() => _medalBuisnessLogicContract.GetMedalById(_storekeeper.Id, "name"), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void InsertMedal_CorrectRecord_Test()
    {
        //Arrange
        var flag = false;
        var record = new MedalDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, _material.Id, "name 1", 1, "desc");
        _medalStorageContract.Setup(x => x.AddElement(It.IsAny<MedalDataModel>()))
            .Callback((MedalDataModel x) =>
            {
                flag = x.Id == record.Id && x.MedalName == record.MedalName && x.Description == record.Description &&
                x.Range == record.Range;
            });
        //Act
        _medalBuisnessLogicContract.InsertMedal(_storekeeper.Id, record);
        //Assert
        _medalStorageContract.Verify(x => x.AddElement(It.IsAny<MedalDataModel>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void InsertMedal_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        var record = new MedalDataModel(Guid.NewGuid().ToString(), storekeeperId, _material.Id, "name 1", 1, "desc");
        _medalStorageContract.Setup(x => x.AddElement(It.IsAny<MedalDataModel>()));

        //Act&
        Assert.That(() => _medalBuisnessLogicContract.InsertMedal(_storekeeper.Id, record), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void InsertMedal_RecordWithExistsData_ThrowException_Test()
    {
        //Arrange
        _medalStorageContract.Setup(x => x.AddElement(It.IsAny<MedalDataModel>())).Throws(new ElementExistsException("Data", "Data"));
        //Act&Assert
        Assert.That(() => _medalBuisnessLogicContract.InsertMedal(_storekeeper.Id, new(Guid.NewGuid().ToString(), _storekeeper.Id, _material.Id, "name 1", 1, "desc")),
            Throws.TypeOf<ElementExistsException>());
        _medalStorageContract.Verify(x => x.AddElement(It.IsAny<MedalDataModel>()), Times.Once);
    }
    [Test]
    public void InsertMedal_NullRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _medalBuisnessLogicContract.InsertMedal(_storekeeper.Id, null),
        Throws.TypeOf<ArgumentNullException>());
        _medalStorageContract.Verify(x => x.AddElement(It.IsAny<MedalDataModel>()), Times.Never);
    }
    [Test]
    public void InsertMedal_InvalidRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _medalBuisnessLogicContract.InsertMedal(_storekeeper.Id, new MedalDataModel("id", _storekeeper.Id, _material.Id, "name 1", 1, "desc")), Throws.TypeOf<ValidationException>());
        _medalStorageContract.Verify(x => x.AddElement(It.IsAny<MedalDataModel>()), Times.Never);
    }

    [Test]
    public void UpdateMedal_CorrectRecord_Test()
    {
        //Arrange
        var flag = false;
        var record = new MedalDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, _material.Id, "name 1", 1, "desc");
        _medalStorageContract.Setup(x => x.UpdElement(It.IsAny<MedalDataModel>()))
        .Callback((MedalDataModel x) =>
        {
            flag = x.Id == record.Id && x.MedalName == record.MedalName && x.Description == record.Description && x.Range == record.Range;
        });
        //Act
        _medalBuisnessLogicContract.UpdateMedal(_storekeeper.Id, record);
        //Assert
        _medalStorageContract.Verify(x => x.UpdElement(It.IsAny<MedalDataModel>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void UpdateMedal_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        var record = new MedalDataModel(Guid.NewGuid().ToString(), storekeeperId, _material.Id, "name 1", 1, "desc");
        _medalStorageContract.Setup(x => x.UpdElement(It.IsAny<MedalDataModel>()));

        //Act&
        Assert.That(() => _medalBuisnessLogicContract.UpdateMedal(_storekeeper.Id, record), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void UpdateMedal_RecordWithIncorrectData_ThrowException_Test()
    {
        //Arrange
        _medalStorageContract.Setup(x => x.UpdElement(It.IsAny<MedalDataModel>())).Throws(new ElementNotFoundException(""));
        //Act&Assert
        Assert.That(() => _medalBuisnessLogicContract.UpdateMedal(_storekeeper.Id, new(Guid.NewGuid().ToString(), _storekeeper.Id, _material.Id, "name 1", 1, "desc")), Throws.TypeOf<ElementNotFoundException>());
        _medalStorageContract.Verify(x => x.UpdElement(It.IsAny<MedalDataModel>()), Times.Once);
    }

    [Test]
    public void UpdateMedal_RecordWithExistsData_ThrowException_Test()
    {
        //Arrange
        _medalStorageContract.Setup(x => x.UpdElement(It.IsAny<MedalDataModel>())).Throws(new ElementExistsException("Data", "Data"));
        //Act&Assert
        Assert.That(() => _medalBuisnessLogicContract.UpdateMedal(_storekeeper.Id, new(Guid.NewGuid().ToString(), _storekeeper.Id, _material.Id, "name 1", 1, "desc")), Throws.TypeOf<ElementExistsException>());
        _medalStorageContract.Verify(x => x.UpdElement(It.IsAny<MedalDataModel>()), Times.Once);
    }
    [Test]
    public void UpdateMedal_NullRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _medalBuisnessLogicContract.UpdateMedal(_storekeeper.Id, null), Throws.TypeOf<ArgumentNullException>());
        _medalStorageContract.Verify(x => x.UpdElement(It.IsAny<MedalDataModel>()), Times.Never);
    }
    [Test]
    public void UpdateMedal_InvalidRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _medalBuisnessLogicContract.UpdateMedal(_storekeeper.Id, new MedalDataModel("id", _storekeeper.Id, _material.Id, "name 1", 1, "desc")),
        Throws.TypeOf<ValidationException>());
        _medalStorageContract.Verify(x => x.UpdElement(It.IsAny<MedalDataModel>()), Times.Never);
    }

    [Test]
    public void DeleteMedal_CorrectRecord_Test()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var flag = false;
        _medalStorageContract.Setup(x => x.DelElement(It.Is((string x) => x == id))).Callback(() => { flag = true; });
        //Act
        _medalBuisnessLogicContract.DeleteMedal(_storekeeper.Id, id);
        //Assert
        _medalStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void DeleteMedal_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        var id = Guid.NewGuid().ToString();
        var record = new MedalDataModel(id, storekeeperId, _material.Id, "name 1", 1, "desc");
        _medalStorageContract.Setup(x => x.GetElementById(id)).Returns(record);
        _medalStorageContract.Setup(x => x.UpdElement(It.IsAny<MedalDataModel>()));

        //Act&
        Assert.That(() => _medalBuisnessLogicContract.DeleteMedal(_storekeeper.Id, id), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void DeleteMedal_RecordWithIncorrectId_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        var id = Guid.NewGuid().ToString();
        var record = new MedalDataModel(id, storekeeperId, _material.Id, "name 1", 1, "desc");
        _medalStorageContract.Setup(x => x.GetElementById(id)).Returns(record);
        _medalStorageContract.Setup(x => x.DelElement(It.IsAny<string>())).Throws(new ElementNotFoundException(""));
        //Act&Assert
        Assert.That(() => _medalBuisnessLogicContract.DeleteMedal(_storekeeper.Id, Guid.NewGuid().ToString()), Throws.TypeOf<ElementNotFoundException>());
        //_medalStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Once);
    }
    [Test]
    public void DeleteMedal_IdIsNullOrEmpty_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _medalBuisnessLogicContract.DeleteMedal(_storekeeper.Id, null), Throws.TypeOf<ArgumentNullException>());
        Assert.That(() => _medalBuisnessLogicContract.DeleteMedal(_storekeeper.Id, string.Empty),
        Throws.TypeOf<ArgumentNullException>());
        _medalStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Never);
    }
    [Test]
    public void DeleteMedal_IdIsNotGuid_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _medalBuisnessLogicContract.DeleteMedal(_storekeeper.Id, "id"), Throws.TypeOf<ValidationException>());
        _medalStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Never);
    }
}
