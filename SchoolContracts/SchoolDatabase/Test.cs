using NUnit.Framework;

namespace SchoolDatabase;

[TestFixture]
internal class Test
{
    SchoolDbContext _schoolDbContext { get; set; }

    /*[SetUp]
    public void SetUp()
    {
        _schoolDbContext = new SchoolDbContext(new ConnectionString());
        _schoolDbContext.Database.EnsureDeleted();
        _schoolDbContext.Database.EnsureCreated();
    }*/

    [Test]
    public void test()
    {
        /*var circle = new CircleDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "name", "nnnn", [], []);
        Assert.That(circle, Is.Not.Null);*/
        Assert.Equals(1, 2);
    }
}
