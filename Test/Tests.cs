using Microsoft.EntityFrameworkCore;
using SchoolContracts;
using SchoolContracts.DataModels;
using SchoolDatabase;
using SchoolDatabase.Implementations;

namespace Test;

[TestFixture]
public class Tests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Structure", "NUnit1032:An IDisposable field/property should be Disposed in a TearDown method", Justification = "<Ожидание>")]
    SchoolDbContext _schoolDbContext;
    private CircleStorageContract _circleStorageContract;


    [SetUp]
    public void Setup()
    {
        _schoolDbContext = new SchoolDbContext(new ConnectionString());
        _circleStorageContract = new CircleStorageContract(_schoolDbContext);
        /*_schoolDbContext.Database.EnsureDeleted();
        _schoolDbContext.Database.EnsureCreated();*/
    }

    /*[TearDown]
    public void TearDown()
    {
        _schoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Orders\" CASCADE; ");
        _schoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Buyers\" CASCADE; ");
        _schoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Workers\" CASCADE; ");
    }*/


    [Test]
    public void Test1()
    {
        var circle = new CircleDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "name", "nnnn", [], []);
        _circleStorageContract.AddElement(circle);
        Assert.Equals(1, 2);
    }
}
