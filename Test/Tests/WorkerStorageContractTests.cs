using Microsoft.EntityFrameworkCore;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolDatabase.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Tests;

namespace Test.Tests;
[TestFixture]
internal class WorkerStorageContractTests : BaseStorageContractTests
{
    private WorkerStorageContract _workerStorageContract;


    [SetUp]
    public void Setup()
    {
        _workerStorageContract = new WorkerStorageContract(SchoolDbContext);
    }

    [TearDown]
    public void TearDown()
    {
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Workers\" CASCADE; ");
    }


    [Test]
    public void TestAddWorker()
    {
        var worker = new WorkerDataModel(Guid.NewGuid().ToString(), "fio", "login", "password", "mail");
        _workerStorageContract.AddElement(worker);

        var dbWorker = _workerStorageContract.GetElementById(worker.Id);
        AssertElement(dbWorker, worker);
    }

    [Test]
    public void TestDeleteWorker()
    {
        var Worker = new WorkerDataModel(Guid.NewGuid().ToString(), "fio", "login", "password", "mail");
        _workerStorageContract.AddElement(Worker);
        _workerStorageContract.DelElement(Worker.Id);

        Assert.That(() => _workerStorageContract.GetElementById(Worker.Id), Throws.TypeOf<ElementNotFoundException>());
    }

    [Test]
    public void UpdateWorker()
    {
        var Worker = new WorkerDataModel(Guid.NewGuid().ToString(), "fio", "login", "password", "mail");
        _workerStorageContract.AddElement(Worker);

        var newWorker = new WorkerDataModel(Worker.Id, "new fio", "login", "password", "mail");
        _workerStorageContract.UpdElement(newWorker);

        AssertElement(_workerStorageContract.GetElementById(Worker.Id), newWorker);
    }

    [Test]
    public void GetListWorkers()
    {
        var Worker1 = new WorkerDataModel(Guid.NewGuid().ToString(), "fio 1", "login 1", "password", "mail 1");
        var Worker2 = new WorkerDataModel(Guid.NewGuid().ToString(), "fio 2", "login 2", "password", "mail 2");
        var Worker3 = new WorkerDataModel(Guid.NewGuid().ToString(), "fio 3", "login 3", "password", "mail 3");

        _workerStorageContract.AddElement(Worker1);
        _workerStorageContract.AddElement(Worker2);
        _workerStorageContract.AddElement(Worker3);

        var list = _workerStorageContract.GetList();
        Assert.That(list.Count, Is.EqualTo(3));
    }

    private void AssertElement(WorkerDataModel actual, WorkerDataModel expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.Login, Is.EqualTo(expected.Login));
        Assert.That(actual.Mail, Is.EqualTo(expected.Mail));
        Assert.That(actual.Password, Is.EqualTo(expected.Password));
    }
}
