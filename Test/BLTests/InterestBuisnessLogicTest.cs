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
using SchoolTests.Infrastructure;

namespace Test.BLTests;
[TestFixture]
internal class InterestBuisnessLogicTest : BaseStorageContractTests
{
    private IInterestBuisnessLogicContract _interestBuisnessLogicContract;
    private Mock<IInterestStorageContract> _interestStorageContract;
    private Worker _worker;
    private Interest _interest;


    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _interestStorageContract = new Mock<IInterestStorageContract>();
        _interestBuisnessLogicContract = new InterestBuisnessLogicContract(_interestStorageContract.Object, new Mock<ILogger>().Object);

        _worker = SchoolDbContext.InsertAndReturnWorker();
        _interest = SchoolDbContext.InsertAndReturnInterest(workerId: _worker.Id);
    }

    [TearDown]
    public void TearDown()
    {
        _interestStorageContract.Reset();
    }

    [Test]
    public void GetAllInterests_ReturnListOfRecords_Test()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var listOriginal = new List<InterestDataModel>()
        {
            new(id, _worker.Id, "name 1", "desc", [(new InterestMaterialDataModel(id, _interest.Id))]),
            new(Guid.NewGuid().ToString(), _worker.Id, "name 2", "desc", []),
            new(Guid.NewGuid().ToString(), _worker.Id, "name 3", "desc", []),
        };

        _interestStorageContract.Setup(x => x.GetList(_worker.Id)).Returns(listOriginal);
        //Act
        var list = _interestBuisnessLogicContract.GetAllInterests(_worker.Id);
        //Assert
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Is.EquivalentTo(listOriginal));
    }

    [Test]
    public void GetAllInterests_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var workerId = Guid.NewGuid().ToString();
        SchoolDbContext.InsertAndReturnWorker(workerId, login: "login 1", password: "psw 1", mail: "mail 1");

        var id = Guid.NewGuid().ToString();
        var listOriginal = new List<InterestDataModel>()
        {
            new(id, workerId, "name 1", "desc", [(new InterestMaterialDataModel(id, _interest.Id))]),
            new(Guid.NewGuid().ToString(), workerId, "name 2", "desc", []),
            new(Guid.NewGuid().ToString(), workerId, "name 3", "desc", []),
        };

        _interestStorageContract.Setup(x => x.GetList(workerId)).Returns(listOriginal);
        //Act&Assert
        Assert.That(() => _interestBuisnessLogicContract.GetAllInterests(_worker.Id), Throws.TypeOf<NullListException>());
    }

    [Test]
    public void GetInterestByData_GetById_ReturnRecord_Test()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var record = new InterestDataModel(id, _worker.Id, "name 2", "desc", []);
        _interestStorageContract.Setup(x => x.GetElementById(id)).Returns(record);
        //Act
        var element = _interestBuisnessLogicContract.GetInterestByData(_worker.Id, id);
        //Assert
        Assert.That(element, Is.Not.Null);
        Assert.That(element.Id, Is.EqualTo(id));
        _interestStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetInterestByData_GetByName_ReturnRecord_Test()
    {
        //Arrange
        var name = "new name";
        var record = new InterestDataModel(Guid.NewGuid().ToString(), _worker.Id, name, "desc", []);
        _interestStorageContract.Setup(x => x.GetElementByName(name)).Returns(record);
        //Act
        var element = _interestBuisnessLogicContract.GetInterestByData(_worker.Id, name);
        //Assert
        Assert.That(element, Is.Not.Null);
        Assert.That(element.InterestName, Is.EqualTo(name));
        _interestStorageContract.Verify(x => x.GetElementByName(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetInterestByData_EmptyData_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _interestBuisnessLogicContract.GetInterestByData(_worker.Id, null), Throws.TypeOf<ArgumentNullException>());
        Assert.That(() => _interestBuisnessLogicContract.GetInterestByData(_worker.Id, string.Empty), Throws.TypeOf<ArgumentNullException>());
        _interestStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Never);
        _interestStorageContract.Verify(x => x.GetElementByName(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetInterestByData_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var workerId = Guid.NewGuid().ToString();
        var record = new InterestDataModel(Guid.NewGuid().ToString(), workerId, "name", "desc", []);
        _interestStorageContract.Setup(x => x.GetElementByName("name")).Returns(record);

        //Act&Assert
        Assert.That(() => _interestBuisnessLogicContract.GetInterestByData(_worker.Id, "name"), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void InsertInterest_CorrectRecord_Test()
    {
        //Arrange
        var flag = false;
        var record = new InterestDataModel(Guid.NewGuid().ToString(), _worker.Id, "name", "desc", []);
        _interestStorageContract.Setup(x => x.AddElement(It.IsAny<InterestDataModel>()))
            .Callback((InterestDataModel x) =>
            {
                flag = x.Id == record.Id && x.InterestName == record.InterestName && x.Description == record.Description;
            });
        //Act
        _interestBuisnessLogicContract.InsertInterest(_worker.Id, record);
        //Assert
        _interestStorageContract.Verify(x => x.AddElement(It.IsAny<InterestDataModel>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void InsertInterest_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var workerId = Guid.NewGuid().ToString();
        var record = new InterestDataModel(Guid.NewGuid().ToString(), workerId, "name", "desc", []);
        _interestStorageContract.Setup(x => x.AddElement(It.IsAny<InterestDataModel>()));

        //Act&
        Assert.That(() => _interestBuisnessLogicContract.InsertInterest(_worker.Id, record), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void InsertInterest_RecordWithExistsData_ThrowException_Test()
    {
        //Arrange
        _interestStorageContract.Setup(x => x.AddElement(It.IsAny<InterestDataModel>())).Throws(new ElementExistsException("Data", "Data"));
        //Act&Assert
        Assert.That(() => _interestBuisnessLogicContract.InsertInterest(_worker.Id, new(Guid.NewGuid().ToString(), _worker.Id, "name", "desc", [])),
            Throws.TypeOf<ElementExistsException>());
        _interestStorageContract.Verify(x => x.AddElement(It.IsAny<InterestDataModel>()), Times.Once);
    }
    [Test]
    public void InsertInterest_NullRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _interestBuisnessLogicContract.InsertInterest(_worker.Id, null),
        Throws.TypeOf<ArgumentNullException>());
        _interestStorageContract.Verify(x => x.AddElement(It.IsAny<InterestDataModel>()), Times.Never);
    }
    [Test]
    public void InsertInterest_InvalidRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _interestBuisnessLogicContract.InsertInterest(_worker.Id, new InterestDataModel("id", _worker.Id, "name", "desc", [])), Throws.TypeOf<ValidationException>());
        _interestStorageContract.Verify(x => x.AddElement(It.IsAny<InterestDataModel>()), Times.Never);
    }

    [Test]
    public void UpdateInterest_CorrectRecord_Test()
    {
        //Arrange
        var flag = false;
        var record = new InterestDataModel(Guid.NewGuid().ToString(), _worker.Id, "name", "desc", []);
        _interestStorageContract.Setup(x => x.UpdElement(It.IsAny<InterestDataModel>()))
        .Callback((InterestDataModel x) =>
        {
            flag = x.Id == record.Id && x.InterestName == record.InterestName && x.Description == record.Description;
        });
        //Act
        _interestBuisnessLogicContract.UpdateInterest(_worker.Id, record);
        //Assert
        _interestStorageContract.Verify(x => x.UpdElement(It.IsAny<InterestDataModel>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void UpdateInterest_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var workerId = Guid.NewGuid().ToString();
        var record = new InterestDataModel(Guid.NewGuid().ToString(), workerId, "name", "desc", []);
        _interestStorageContract.Setup(x => x.UpdElement(It.IsAny<InterestDataModel>()));

        //Act&
        Assert.That(() => _interestBuisnessLogicContract.UpdateInterest(_worker.Id, record), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void UpdateInterest_RecordWithIncorrectData_ThrowException_Test()
    {
        //Arrange
        _interestStorageContract.Setup(x => x.UpdElement(It.IsAny<InterestDataModel>())).Throws(new ElementNotFoundException(""));
        //Act&Assert
        Assert.That(() => _interestBuisnessLogicContract.UpdateInterest(_worker.Id, new(Guid.NewGuid().ToString(), _worker.Id, "name", "desc", [])), Throws.TypeOf<ElementNotFoundException>());
        _interestStorageContract.Verify(x => x.UpdElement(It.IsAny<InterestDataModel>()), Times.Once);
    }

    [Test]
    public void UpdateInterest_RecordWithExistsData_ThrowException_Test()
    {
        //Arrange
        _interestStorageContract.Setup(x => x.UpdElement(It.IsAny<InterestDataModel>())).Throws(new ElementExistsException("Data", "Data"));
        //Act&Assert
        Assert.That(() => _interestBuisnessLogicContract.UpdateInterest(_worker.Id, new(Guid.NewGuid().ToString(), _worker.Id, "name", "desc", [])), Throws.TypeOf<ElementExistsException>());
        _interestStorageContract.Verify(x => x.UpdElement(It.IsAny<InterestDataModel>()), Times.Once);
    }
    [Test]
    public void UpdateInterest_NullRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _interestBuisnessLogicContract.UpdateInterest(_worker.Id, null), Throws.TypeOf<ArgumentNullException>());
        _interestStorageContract.Verify(x => x.UpdElement(It.IsAny<InterestDataModel>()), Times.Never);
    }
    [Test]
    public void UpdateInterest_InvalidRecord_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _interestBuisnessLogicContract.UpdateInterest(_worker.Id, new InterestDataModel("id", _worker.Id, "name", "desc", [])),
        Throws.TypeOf<ValidationException>());
        _interestStorageContract.Verify(x => x.UpdElement(It.IsAny<InterestDataModel>()), Times.Never);
    }

    [Test]
    public void DeleteInterest_CorrectRecord_Test()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var flag = false;
        _interestStorageContract.Setup(x => x.DelElement(It.Is((string x) => x == id))).Callback(() => { flag = true; });
        //Act
        _interestBuisnessLogicContract.DeleteInterest(_worker.Id, id);
        //Assert
        _interestStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void DeleteInterest_WithoutAuth_ThrowException_Test()
    {
        //Arrange
        var workerId = Guid.NewGuid().ToString();
        var id = Guid.NewGuid().ToString();
        var record = new InterestDataModel(id, workerId, "name", "desc", []);
        _interestStorageContract.Setup(x => x.GetElementById(id)).Returns(record);
        _interestStorageContract.Setup(x => x.UpdElement(It.IsAny<InterestDataModel>()));

        //Act&
        Assert.That(() => _interestBuisnessLogicContract.DeleteInterest(_worker.Id, id), Throws.TypeOf<UnauthorizedAccessException>());
    }

    [Test]
    public void DeleteInterest_RecordWithIncorrectId_ThrowException_Test()
    {
        //Arrange
        _interestStorageContract.Setup(x => x.DelElement(It.IsAny<string>())).Throws(new ElementNotFoundException(""));
        //Act&Assert
        Assert.That(() => _interestBuisnessLogicContract.DeleteInterest(_worker.Id, Guid.NewGuid().ToString()), Throws.TypeOf<ElementNotFoundException>());
        _interestStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Once);
    }
    [Test]
    public void DeleteCircle_IdIsNullOrEmpty_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _interestBuisnessLogicContract.DeleteInterest(_worker.Id, null), Throws.TypeOf<ArgumentNullException>());
        Assert.That(() => _interestBuisnessLogicContract.DeleteInterest(_worker.Id, string.Empty),
        Throws.TypeOf<ArgumentNullException>());
        _interestStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Never);
    }
    [Test]
    public void DeleteCircle_IdIsNotGuid_ThrowException_Test()
    {
        //Act&Assert
        Assert.That(() => _interestBuisnessLogicContract.DeleteInterest(_worker.Id, "id"), Throws.TypeOf<ValidationException>());
        _interestStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Never);
    }
}
