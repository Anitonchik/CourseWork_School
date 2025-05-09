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

namespace Test.BLTests;


[TestFixture]
internal class CircleBuisnessLogicTest : BaseStorageContractTests
{
    private ICircleBuisnessLogicContract _cirlceBuisnessLogicContract;
    private Mock<ICircleStorageContract> _circleStorageContract;
    private Storekeeper _storekeeper;
    private Material _material;


    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _circleStorageContract = new Mock<ICircleStorageContract>();
        _cirlceBuisnessLogicContract = new CircleBuisnessLogicContract(_circleStorageContract.Object, new Mock<ILogger>().Object);

        _storekeeper = SchoolDbContext.InsertAndReturnStorekeeper();
        _material = SchoolDbContext.InsertAndReturnMaterial(storekeeperId: _storekeeper.Id);
    }

    [TearDown]
    public void TearDown()
    {
        _circleStorageContract.Reset();
    }

    [Test]
    public void GetAllCirles_ReturnListOfRecords_Test()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var listOriginal = new List<CircleDataModel>()
        {
            new(id, _storekeeper.Id, "name 1", "desc", [(new CircleMaterialDataModel (id, _material.Id, 1))], []),
            new(Guid.NewGuid().ToString(), _storekeeper.Id, "name 2", "desc", [], []),
            new(Guid.NewGuid().ToString(), _storekeeper.Id, "name 3", "desc", [], []),
        };

        _circleStorageContract.Setup(x => x.GetList(_storekeeper.Id)).Returns(listOriginal);
        //Act
        var list = _cirlceBuisnessLogicContract.GetAllCircles(_storekeeper.Id);
        //Assert
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Is.EquivalentTo(listOriginal));
    }

    [Test]
    public void GetAllCircles_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        SchoolDbContext.InsertAndReturnStorekeeper(storekeeperId, login: "login 1", password: "psw 1", mail: "mail 1");

        var id = Guid.NewGuid().ToString();
        var listOriginal = new List<CircleDataModel>()
        {
            new(id, storekeeperId, "name 1", "desc", [(new CircleMaterialDataModel (id, _material.Id, 1))], []),
            new(Guid.NewGuid().ToString(), storekeeperId, "name 2", "desc", [], []),
            new(Guid.NewGuid().ToString(), storekeeperId, "name 3", "desc", [], []),
        };

        _circleStorageContract.Setup(x => x.GetList(storekeeperId)).Returns(listOriginal);
        //Act&Assert
        Assert.That(() => _cirlceBuisnessLogicContract.GetAllCircles(_storekeeper.Id), Throws.TypeOf<NullListException>());
    }

    [Test]
    public void GetCircleByData_GetById_ReturnRecord_Test()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var record = new CircleDataModel(id, _storekeeper.Id, "name 2", "desc", [], []);
        _circleStorageContract.Setup(x => x.GetElementById(id)).Returns(record);
        //Act
        var element = _cirlceBuisnessLogicContract.GetCircleByData(_storekeeper.Id, id);
        //Assert
        Assert.That(element, Is.Not.Null);
        Assert.That(element.Id, Is.EqualTo(id));
        _circleStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetCircleByData_GetByName_ReturnRecord_Test()
    {
        //Arrange
        var name = "new name";
        var record = new CircleDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, name, "desc", [], []);
        _circleStorageContract.Setup(x => x.GetElementByName(name)).Returns(record);
        //Act
        var element = _cirlceBuisnessLogicContract.GetCircleByData(_storekeeper.Id, name);
        //Assert
        Assert.That(element, Is.Not.Null);
        Assert.That(element.CircleName, Is.EqualTo(name));
        _circleStorageContract.Verify(x => x.GetElementByName(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetCircleByData_EmptyData_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _cirlceBuisnessLogicContract.GetCircleByData(_storekeeper.Id, null), Throws.TypeOf<ArgumentNullException>());
        Assert.That(() => _cirlceBuisnessLogicContract.GetCircleByData(_storekeeper.Id, string.Empty), Throws.TypeOf<ArgumentNullException>());
        _circleStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Never);
        _circleStorageContract.Verify(x => x.GetElementByName(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetCircleByData_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        var record = new CircleDataModel(Guid.NewGuid().ToString(), storekeeperId, "name", "desc", [], []);
        _circleStorageContract.Setup(x => x.GetElementByName("name")).Returns(record);

        //Act&Assert
        Assert.That(() => _cirlceBuisnessLogicContract.GetCircleByData(_storekeeper.Id, "name"), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void InsertCircle_CorrectRecord_Test()
    {
        //Arrange
        var flag = false;
        var record = new CircleDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "desc", [], []);
        _circleStorageContract.Setup(x => x.AddElement(It.IsAny<CircleDataModel>()))
            .Callback((CircleDataModel x) =>
        {
            flag = x.Id == record.Id && x.CircleName == record.CircleName && x.Description == record.Description;
        });
        //Act
        _cirlceBuisnessLogicContract.InsertCircle(_storekeeper.Id, record);
        //Assert
        _circleStorageContract.Verify(x => x.AddElement(It.IsAny<CircleDataModel>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void InsertCircle_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        var record = new CircleDataModel(Guid.NewGuid().ToString(), storekeeperId, "name", "desc", [], []);
        _circleStorageContract.Setup(x => x.AddElement(It.IsAny<CircleDataModel>()));

        //Act&
        Assert.That(() => _cirlceBuisnessLogicContract.InsertCircle(_storekeeper.Id, record), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void InsertCircle_RecordWithExistsData_ThrowException_Test()
    {
        //Arrange
        _circleStorageContract.Setup(x => x.AddElement(It.IsAny<CircleDataModel>())).Throws(new ElementExistsException("Data", "Data"));
        //Act&Assert
        Assert.That(() => _cirlceBuisnessLogicContract.InsertCircle(_storekeeper.Id, new(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "desc", [], [])),
            Throws.TypeOf<ElementExistsException>());
        _circleStorageContract.Verify(x => x.AddElement(It.IsAny<CircleDataModel>()), Times.Once);
    }
    [Test]
    public void InsertCircle_NullRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _cirlceBuisnessLogicContract.InsertCircle(_storekeeper.Id, null),
        Throws.TypeOf<ArgumentNullException>());
        _circleStorageContract.Verify(x => x.AddElement(It.IsAny<CircleDataModel>()), Times.Never);
    }
    [Test]
    public void InsertCircle_InvalidRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _cirlceBuisnessLogicContract.InsertCircle(_storekeeper.Id, new CircleDataModel("id", _storekeeper.Id, "name", "desc", [], [])), Throws.TypeOf<ValidationException>());
        _circleStorageContract.Verify(x => x.AddElement(It.IsAny<CircleDataModel>()), Times.Never);
    }

    [Test]
    public void UpdateCircle_CorrectRecord_Test()
    {
        //Arrange
        var flag = false;
        var record = new CircleDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "desc", [], []);
        _circleStorageContract.Setup(x => x.UpdElement(It.IsAny<CircleDataModel>()))
        .Callback((CircleDataModel x) =>
        {
            flag = x.Id == record.Id && x.CircleName == record.CircleName && x.Description == record.Description;
        });
        //Act
        _cirlceBuisnessLogicContract.UpdateCircle(_storekeeper.Id, record);
        //Assert
        _circleStorageContract.Verify(x => x.UpdElement(It.IsAny<CircleDataModel>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void UpdateCircle_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        var record = new CircleDataModel(Guid.NewGuid().ToString(), storekeeperId, "name", "desc", [], []);
        _circleStorageContract.Setup(x => x.UpdElement(It.IsAny<CircleDataModel>()));

        //Act&
        Assert.That(() => _cirlceBuisnessLogicContract.UpdateCircle(_storekeeper.Id, record), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void UpdateCircle_RecordWithIncorrectData_ThrowException_Test()
    {
        //Arrange
        _circleStorageContract.Setup(x => x.UpdElement(It.IsAny<CircleDataModel>())).Throws(new ElementNotFoundException(""));
        //Act&Assert
        Assert.That(() => _cirlceBuisnessLogicContract.UpdateCircle(_storekeeper.Id, new(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "desc", [], [])), Throws.TypeOf<ElementNotFoundException>());
        _circleStorageContract.Verify(x => x.UpdElement(It.IsAny<CircleDataModel>()), Times.Once);
    }

    [Test]
    public void UpdateCircle_RecordWithExistsData_ThrowException_Test()
    {
        //Arrange
        _circleStorageContract.Setup(x => x.UpdElement(It.IsAny<CircleDataModel>())).Throws(new ElementExistsException("Data", "Data"));
        //Act&Assert
        Assert.That(() => _cirlceBuisnessLogicContract.UpdateCircle(_storekeeper.Id, new(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "desc", [], [])), Throws.TypeOf<ElementExistsException>());
        _circleStorageContract.Verify(x => x.UpdElement(It.IsAny<CircleDataModel>()), Times.Once);
    }
    [Test]
    public void UpdateCircle_NullRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _cirlceBuisnessLogicContract.UpdateCircle(_storekeeper.Id, null), Throws.TypeOf<ArgumentNullException>());
        _circleStorageContract.Verify(x => x.UpdElement(It.IsAny<CircleDataModel>()), Times.Never);
    }
    [Test]
    public void UpdateCircle_InvalidRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _cirlceBuisnessLogicContract.UpdateCircle(_storekeeper.Id, new CircleDataModel("id", _storekeeper.Id, "name", "desc", [], [])),
        Throws.TypeOf<ValidationException>());
        _circleStorageContract.Verify(x => x.UpdElement(It.IsAny<CircleDataModel>()), Times.Never);
    }

    [Test]
    public void DeleteCircle_CorrectRecord_Test()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var flag = false;
        _circleStorageContract.Setup(x => x.DelElement(It.Is((string x) => x == id))).Callback(() => { flag = true; });
        //Act
        _cirlceBuisnessLogicContract.DeleteCircle(_storekeeper.Id, id);
        //Assert
        _circleStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void DeleteCircle_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        var id = Guid.NewGuid().ToString();
        var record = new CircleDataModel(id, storekeeperId, "name", "desc", [], []);
        _circleStorageContract.Setup(x => x.GetElementById(id)).Returns(record);
        _circleStorageContract.Setup(x => x.UpdElement(It.IsAny<CircleDataModel>()));

        //Act&
        Assert.That(() => _cirlceBuisnessLogicContract.DeleteCircle(_storekeeper.Id, id), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void DeleteCircle_RecordWithIncorrectId_ThrowException_Test()
    {
        //Arrange
        _circleStorageContract.Setup(x => x.DelElement(It.IsAny<string>())).Throws(new ElementNotFoundException(""));
        //Act&Assert
        Assert.That(() => _cirlceBuisnessLogicContract.DeleteCircle(_storekeeper.Id, Guid.NewGuid().ToString()), Throws.TypeOf<ElementNotFoundException>());
        _circleStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Once);
    }
    [Test]
    public void DeleteCircle_IdIsNullOrEmpty_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _cirlceBuisnessLogicContract.DeleteCircle(_storekeeper.Id, null), Throws.TypeOf<ArgumentNullException>());
        Assert.That(() => _cirlceBuisnessLogicContract.DeleteCircle(_storekeeper.Id, string.Empty),
        Throws.TypeOf<ArgumentNullException>());
        _circleStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Never);
    }
    [Test]
    public void DeleteCircle_IdIsNotGuid_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _cirlceBuisnessLogicContract.DeleteCircle(_storekeeper.Id, "id"), Throws.TypeOf<ValidationException>());
        _circleStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Never);
    }
}
