using SchoolContracts;
using SchoolDatabase;

namespace Test.Tests;

public class BaseStorageContractTests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Structure", "NUnit1032:An IDisposable field/property should be Disposed in a TearDown method", Justification = "<Ожидание>")]
    protected SchoolDbContext SchoolDbContext { get; private set; }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        SchoolDbContext = new SchoolDbContext(new ConnectionString());
        SchoolDbContext.Database.EnsureDeleted();
        SchoolDbContext.Database.EnsureCreated();
    }
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        SchoolDbContext.Database.EnsureDeleted();
        SchoolDbContext.Dispose();
    }
}
