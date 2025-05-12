using Microsoft.EntityFrameworkCore;
using SchoolContracts.BindingModels;
using SchoolContracts.ViewModels;
using SchoolDatabase.Models;
using SchoolTests.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SchoolTests.WebApiTests;

[TestFixture]
internal class LessonControllerTests : BaseWebApiControllerTest
{
   
    private string _workerId;


    [SetUp]
    public void SetUp()
    {
        _workerId = SchoolDbContext.InsertAndReturnWorker().Id;
    }
    [TearDown]
    public void TearDown()
    {
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Lessons\" CASCADE; ");
        SchoolDbContext.Database.ExecuteSqlRaw("TRUNCATE \"Workers\" CASCADE; ");
    }
    [Test]
    public async Task GetList_WhenHaveRecords_ShouldSuccess_Test()
    {
        //Arrange
        var lesson = SchoolDbContext.InsertAndReturnLesson(_workerId, lessonName:"name 1");
        SchoolDbContext.InsertAndReturnLesson(_workerId, lessonName: "name 2");
        SchoolDbContext.InsertAndReturnLesson(_workerId, lessonName: "name 3");
        //Act
        var response = await HttpClient.GetAsync($"/api/lessons/getrecords");
        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var data = await GetModelFromResponseAsync<List<LessonViewModel>>(response);
        Assert.Multiple(() =>
        {
            Assert.That(data, Is.Not.Null);
            Assert.That(data, Has.Count.EqualTo(3));
        });
        AssertElement(data.First(x => x.Id == lesson.Id), lesson);
    }
    private static void AssertElement(LessonViewModel? actual, Lesson expected)
    {
        Assert.That(actual, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.WorkerId, Is.EqualTo(expected.WorkerId));
            Assert.That(actual.LessonName, Is.EqualTo(expected.LessonName));
            Assert.That(actual.LessonDate, Is.EqualTo(expected.LessonDate));
            Assert.That(actual.Description, Is.EqualTo(expected.Description));
        });
    }
    private static void AssertElement(Lesson? actual, LessonBindingModel expected)
    {
        Assert.That(actual, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.WorkerId, Is.EqualTo(expected.WorkerId));
            Assert.That(actual.LessonName, Is.EqualTo(expected.LessonName));
            Assert.That(actual.LessonDate, Is.EqualTo(expected.LessonDate));
            Assert.That(actual.Description, Is.EqualTo(expected.Description));
        });
    }
}
