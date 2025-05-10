using AutoMapper;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;
using UnauthorizedAccessException = SchoolContracts.Exceptions.UnauthorizedAccessException;

namespace SchoolDatabase.Implementations;

public class LessonCircleStorageContract : ILessonCircleStorageContract
{
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapper;

    private readonly CircleStorageContract _circleStorageContract;
    private readonly LessonStorageContract _lessonStorageContract;

    public LessonCircleStorageContract(SchoolDbContext dbContext,
        CircleStorageContract circleStorageContract, LessonStorageContract lessonStorageContract)
    {
        _dbContext = dbContext;

        var configuration = new MapperConfiguration(cfg => cfg.AddMaps(typeof(LessonCircle)));

        _mapper = new Mapper(configuration);

        _circleStorageContract = circleStorageContract;
        _lessonStorageContract = lessonStorageContract;
    }

    //CreateConnectLessonAndCircle
    public void AddElement(LessonCircleDataModel lessonCircleDataModel)
    {
        try
        {
            _circleStorageContract.GetElementById(lessonCircleDataModel.CircleId);
            _lessonStorageContract.GetElementById(lessonCircleDataModel.LessonId);
            
            _dbContext.LessonCircles.Add(_mapper.Map<LessonCircle>(lessonCircleDataModel));
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

    public void DeleteElement(string lessonId, string circleId)
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

    private LessonCircle? GetLessonCircleById(string lessonId, string circleId) =>
        _dbContext.LessonCircles.FirstOrDefault(x => x.LessonId == lessonId && x.CircleId == circleId);
}
