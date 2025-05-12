using AutoMapper;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;

namespace SchoolDatabase.Implementations;

public class ConnectEntitiesStorageContract : IConnectEntitiesStorageContract
{
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapperLessonCircle;
    private readonly Mapper _mapperLessonInterest;

    private readonly CircleStorageContract _circleStorageContract;
    private readonly LessonStorageContract _lessonStorageContract;
    private readonly InterestStorageContract _interestStorageContract;


    public ConnectEntitiesStorageContract(SchoolDbContext dbContext, 
        CircleStorageContract? circleStorageContract, LessonStorageContract? lessonStorageContract,
        InterestStorageContract? interestStorageContract)
    {
        _dbContext = dbContext;

        var configurationLessonCircle = new MapperConfiguration(cfg => cfg.AddMaps(typeof(LessonCircle)));
        var configurationLessonInterest = new MapperConfiguration(cfg => cfg.AddMaps(typeof(LessonInterest)));
       
        _mapperLessonCircle = new Mapper(configurationLessonCircle);
        _mapperLessonInterest = new Mapper(configurationLessonInterest);

        _circleStorageContract = circleStorageContract;
        _lessonStorageContract = lessonStorageContract;
        _interestStorageContract = interestStorageContract;
    }

    public void CreateConnectLessonAndCircle(string lessonId, string circleId, int count)
    {
        try
        {
            _circleStorageContract.GetElementById(circleId);
            _lessonStorageContract.GetElementById(lessonId);

            var lessonCircleDataModel = new LessonCircleDataModel(circleId, lessonId, count);
            _dbContext.LessonCircles.Add(_mapperLessonCircle.Map<LessonCircle>(lessonCircleDataModel));
            _dbContext.SaveChanges();
        }
        catch (ElementNotFoundException)
        {
            _dbContext.ChangeTracker.Clear();
            throw;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    public void DeleteConnectLessonAndCircle(string lessonId, string circleId)
    {
        try
        {
            _circleStorageContract.GetElementById(circleId);
            _lessonStorageContract.GetElementById(lessonId);

            var element = GetLessonCircleById(lessonId, circleId) ?? throw new ConnectElementsNotFoundException(lessonId, circleId);
            _dbContext.LessonCircles.Remove(element);
            _dbContext.SaveChanges();
        }
        catch (ElementNotFoundException)
        {
            _dbContext.ChangeTracker.Clear();
            throw;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    public void CreateConnectLessonAndInterest(string lessonId, string interestId,string category)
    {
        try
        {
            _interestStorageContract.GetElementById(interestId);
            _lessonStorageContract.GetElementById(lessonId);

            var lessonInterestDataModel = new LessonInterestDataModel(interestId, lessonId, category);
            _dbContext.LessonInterests.Add(_mapperLessonInterest.Map<LessonInterest>(lessonInterestDataModel));
            _dbContext.SaveChanges();
        }
        catch (ElementNotFoundException)
        {
            _dbContext.ChangeTracker.Clear();
            throw;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    public void DeleteConnectLessonAndInterest(string lessonId, string interestId)
    {
        try
        {
            _interestStorageContract.GetElementById(interestId);
            _lessonStorageContract.GetElementById(lessonId);

            var element = GetLessonInterestById(lessonId, interestId) ?? throw new ConnectElementsNotFoundException(lessonId, interestId);
            _dbContext.LessonInterests.Remove(element);
            _dbContext.SaveChanges();
        }
        catch (ElementNotFoundException)
        {
            _dbContext.ChangeTracker.Clear();
            throw;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    private LessonCircle? GetLessonCircleById(string lessonId, string circleId) => 
        _dbContext.LessonCircles.FirstOrDefault(x => x.LessonId == lessonId && x.CircleId == circleId);

    private LessonInterest? GetLessonInterestById(string lessonId, string interestId) =>
        _dbContext.LessonInterests.FirstOrDefault(x => x.LessonId == lessonId && x.InterestId == interestId);
}
