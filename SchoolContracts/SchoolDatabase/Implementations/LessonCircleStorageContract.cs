using AutoMapper;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;

namespace SchoolDatabase.Implementations;

public class LessonCircleStorageContract : ILessonCircleStorageContract
{
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapper;
    public LessonCircleStorageContract(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<LessonCircleDataModel, LessonCircle>();
            cfg.CreateMap<LessonCircle, LessonCircleDataModel>();
        });

        _mapper = new Mapper(configuration);
    }

    public void AddElement(LessonCircleDataModel lessonCircleDataModel)
    {
        try
        {
            var data = _mapper.Map<LessonCircle>(lessonCircleDataModel);
            _dbContext.LessonCircles.Add(data);
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", lessonCircleDataModel.LessonId);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    public LessonCircleDataModel? GetLessonCircleById(string lessonId, string circleId) => _mapper.Map<LessonCircleDataModel>(
        _dbContext.LessonCircles.FirstOrDefault(x => x.LessonId == lessonId && x.CircleId == circleId));
}
