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

internal class MaterialBuisnessLogicTests : BaseStorageContractTests
{
    private IMaterialBuisnessLogicContract _materialBuisnessLogicContract;
    private Mock<IMaterialStorageContract> _materialStorageContract;
    private Storekeeper _storekeeper;
    private Material _material;


    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _materialStorageContract = new Mock<IMaterialStorageContract>();
        _materialBuisnessLogicContract = new MaterialBuisnessLogicContract(_materialStorageContract.Object, new Mock<ILogger>().Object);

        _storekeeper = SchoolDbContext.InsertAndReturnStorekeeper();
        _material = SchoolDbContext.InsertAndReturnMaterial(storekeeperId: _storekeeper.Id);
    }

    [TearDown]
    public void TearDown()
    {
        _materialStorageContract.Reset();
    }

    [Test]
    public void GetAllMaterials_ReturnListOfRecords_Test()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var listOriginal = new List<MaterialDataModel>()
        {
            new(id, _storekeeper.Id, "name 1", "desc"),
            new(Guid.NewGuid().ToString(), _storekeeper.Id, "name 2", "desc"),
            new(Guid.NewGuid().ToString(), _storekeeper.Id, "name 3", "desc"),
        };

        _materialStorageContract.Setup(x => x.GetList(_storekeeper.Id)).Returns(listOriginal);
        //Act
        var list = _materialBuisnessLogicContract.GetAllMaterials(_storekeeper.Id);
        //Assert
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Is.EquivalentTo(listOriginal));
    }

    [Test]
    public void GetAllMaterials_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        SchoolDbContext.InsertAndReturnStorekeeper(storekeeperId, login: "login 1", password: "psw 1", mail: "mail 1");

        var id = Guid.NewGuid().ToString();
        var listOriginal = new List<MaterialDataModel>()
        {
            new(id, storekeeperId, "name 1", "desc"),
            new(Guid.NewGuid().ToString(), storekeeperId, "name 2", "desc"),
            new(Guid.NewGuid().ToString(), storekeeperId, "name 3", "desc"),
        };

        _materialStorageContract.Setup(x => x.GetList(storekeeperId)).Returns(listOriginal);
        //Act&Assert
        Assert.That(() => _materialBuisnessLogicContract.GetAllMaterials(_storekeeper.Id), Throws.TypeOf<NullListException>());
    }

    [Test]
    public void GetMaterialByData_GetById_ReturnRecord_Test()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var record = new MaterialDataModel(id, _storekeeper.Id, "name 2", "desc");
        _materialStorageContract.Setup(x => x.GetElementById(id)).Returns(record);
        //Act
        var element = _materialBuisnessLogicContract.GetMaterialByData(_storekeeper.Id, id);
        //Assert
        Assert.That(element, Is.Not.Null);
        Assert.That(element.Id, Is.EqualTo(id));
        _materialStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetMaterialByData_GetByName_ReturnRecord_Test()
    {
        //Arrange
        var name = "new name";
        var record = new MaterialDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, name, "desc");
        _materialStorageContract.Setup(x => x.GetElementByName(name)).Returns(record);
        //Act
        var element = _materialBuisnessLogicContract.GetMaterialByData(_storekeeper.Id, name);
        //Assert
        Assert.That(element, Is.Not.Null);
        Assert.That(element.MaterialName, Is.EqualTo(name));
        _materialStorageContract.Verify(x => x.GetElementByName(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetMaterialByData_EmptyData_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _materialBuisnessLogicContract.GetMaterialByData(_storekeeper.Id, null), Throws.TypeOf<ArgumentNullException>());
        Assert.That(() => _materialBuisnessLogicContract.GetMaterialByData(_storekeeper.Id, string.Empty), Throws.TypeOf<ArgumentNullException>());
        _materialStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Never);
        _materialStorageContract.Verify(x => x.GetElementByName(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetMaterialByData_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        var record = new MaterialDataModel(Guid.NewGuid().ToString(), storekeeperId, "name", "desc");
        _materialStorageContract.Setup(x => x.GetElementByName("name")).Returns(record);

        //Act&Assert
        Assert.That(() => _materialBuisnessLogicContract.GetMaterialByData(_storekeeper.Id, "name"), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void InsertMaterial_CorrectRecord_Test()
    {
        //Arrange
        var flag = false;
        var record = new MaterialDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "desc");
        _materialStorageContract.Setup(x => x.AddElement(It.IsAny<MaterialDataModel>()))
            .Callback((MaterialDataModel x) =>
            {
                flag = x.Id == record.Id && x.MaterialName == record.MaterialName && x.Description == record.Description;
            });
        //Act
        _materialBuisnessLogicContract.InsertMaterial(_storekeeper.Id, record);
        //Assert
        _materialStorageContract.Verify(x => x.AddElement(It.IsAny<MaterialDataModel>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void InsertMaterial_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        var record = new MaterialDataModel(Guid.NewGuid().ToString(), storekeeperId, "name", "desc");
        _materialStorageContract.Setup(x => x.AddElement(It.IsAny<MaterialDataModel>()));

        //Act&
        Assert.That(() => _materialBuisnessLogicContract.InsertMaterial(_storekeeper.Id, record), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void InsertMaterial_RecordWithExistsData_ThrowException_Test()
    {
        //Arrange
        _materialStorageContract.Setup(x => x.AddElement(It.IsAny<MaterialDataModel>())).Throws(new ElementExistsException("Data", "Data"));
        //Act&Assert
        Assert.That(() => _materialBuisnessLogicContract.InsertMaterial(_storekeeper.Id, new(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "desc")),
            Throws.TypeOf<ElementExistsException>());
        _materialStorageContract.Verify(x => x.AddElement(It.IsAny<MaterialDataModel>()), Times.Once);
    }
    [Test]
    public void InsertMaterial_NullRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _materialBuisnessLogicContract.InsertMaterial(_storekeeper.Id, null),
        Throws.TypeOf<ArgumentNullException>());
        _materialStorageContract.Verify(x => x.AddElement(It.IsAny<MaterialDataModel>()), Times.Never);
    }
    [Test]
    public void InsertMaterial_InvalidRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _materialBuisnessLogicContract.InsertMaterial(_storekeeper.Id, new MaterialDataModel("id", _storekeeper.Id, "name", "desc")), Throws.TypeOf<ValidationException>());
        _materialStorageContract.Verify(x => x.AddElement(It.IsAny<MaterialDataModel>()), Times.Never);
    }

    [Test]
    public void UpdateMaterial_CorrectRecord_Test()
    {
        //Arrange
        var flag = false;
        var record = new MaterialDataModel(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "desc");
        _materialStorageContract.Setup(x => x.UpdElement(It.IsAny<MaterialDataModel>()))
        .Callback((MaterialDataModel x) =>
        {
            flag = x.Id == record.Id && x.MaterialName == record.MaterialName && x.Description == record.Description;
        });
        //Act
        _materialBuisnessLogicContract.UpdateMaterial(_storekeeper.Id, record);
        //Assert
        _materialStorageContract.Verify(x => x.UpdElement(It.IsAny<MaterialDataModel>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void UpdateMaterial_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        var record = new MaterialDataModel(Guid.NewGuid().ToString(), storekeeperId, "name", "desc");
        _materialStorageContract.Setup(x => x.UpdElement(It.IsAny<MaterialDataModel>()));

        //Act&
        Assert.That(() => _materialBuisnessLogicContract.UpdateMaterial(_storekeeper.Id, record), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void UpdateMaterial_RecordWithIncorrectData_ThrowException_Test()
    {
        //Arrange
        _materialStorageContract.Setup(x => x.UpdElement(It.IsAny<MaterialDataModel>())).Throws(new ElementNotFoundException(""));
        //Act&Assert
        Assert.That(() => _materialBuisnessLogicContract.UpdateMaterial(_storekeeper.Id, new(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "desc")), Throws.TypeOf<ElementNotFoundException>());
        _materialStorageContract.Verify(x => x.UpdElement(It.IsAny<MaterialDataModel>()), Times.Once);
    }

    [Test]
    public void UpdateMaterial_RecordWithExistsData_ThrowException_Test()
    {
        //Arrange
        _materialStorageContract.Setup(x => x.UpdElement(It.IsAny<MaterialDataModel>())).Throws(new ElementExistsException("Data", "Data"));
        //Act&Assert
        Assert.That(() => _materialBuisnessLogicContract.UpdateMaterial(_storekeeper.Id, new(Guid.NewGuid().ToString(), _storekeeper.Id, "name", "desc")), Throws.TypeOf<ElementExistsException>());
        _materialStorageContract.Verify(x => x.UpdElement(It.IsAny<MaterialDataModel>()), Times.Once);
    }
    [Test]
    public void UpdateMaterial_NullRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _materialBuisnessLogicContract.UpdateMaterial(_storekeeper.Id, null), Throws.TypeOf<ArgumentNullException>());
        _materialStorageContract.Verify(x => x.UpdElement(It.IsAny<MaterialDataModel>()), Times.Never);
    }
    [Test]
    public void UpdateMaterial_InvalidRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _materialBuisnessLogicContract.UpdateMaterial(_storekeeper.Id, new MaterialDataModel("id", _storekeeper.Id, "name", "desc")),
        Throws.TypeOf<ValidationException>());
        _materialStorageContract.Verify(x => x.UpdElement(It.IsAny<MaterialDataModel>()), Times.Never);
    }

    [Test]
    public void DeleteMaterial_CorrectRecord_Test()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var flag = false;
        _materialStorageContract.Setup(x => x.DelElement(It.Is((string x) => x == id))).Callback(() => { flag = true; });
        //Act
        _materialBuisnessLogicContract.DeleteMaterial(_storekeeper.Id, id);
        //Assert
        _materialStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void DeleteMaterial_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        var id = Guid.NewGuid().ToString();
        var record = new MaterialDataModel(id, storekeeperId, "name", "desc");
        _materialStorageContract.Setup(x => x.GetElementById(id)).Returns(record);
        _materialStorageContract.Setup(x => x.UpdElement(It.IsAny<MaterialDataModel>()));

        //Act&
        Assert.That(() => _materialBuisnessLogicContract.DeleteMaterial(_storekeeper.Id, id), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void DeleteMaterial_RecordWithIncorrectId_ThrowException_Test()
    {
        //Arrange
        var storekeeperId = Guid.NewGuid().ToString();
        var id = Guid.NewGuid().ToString();
        var record = new MaterialDataModel(id, storekeeperId, "name", "desc");
        _materialStorageContract.Setup(x => x.GetElementById(id)).Returns(record);
        _materialStorageContract.Setup(x => x.DelElement(It.IsAny<string>())).Throws(new ElementNotFoundException(""));
        //Act&Assert
        Assert.That(() => _materialBuisnessLogicContract.DeleteMaterial(_storekeeper.Id, Guid.NewGuid().ToString()), Throws.TypeOf<ElementNotFoundException>());
        //_materialStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Once);
    }
    [Test]
    public void DeleteMaterial_IdIsNullOrEmpty_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _materialBuisnessLogicContract.DeleteMaterial(_storekeeper.Id, null), Throws.TypeOf<ArgumentNullException>());
        Assert.That(() => _materialBuisnessLogicContract.DeleteMaterial(_storekeeper.Id, string.Empty),
        Throws.TypeOf<ArgumentNullException>());
        _materialStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Never);
    }
    [Test]
    public void DeleteMaterial_IdIsNotGuid_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _materialBuisnessLogicContract.DeleteMaterial(_storekeeper.Id, "id"), Throws.TypeOf<ValidationException>());
        _materialStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Never);
    }
}
