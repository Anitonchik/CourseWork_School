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

        var configuration = new MapperConfiguration(cfg => cfg.AddMaps(typeof(LessonCircle)));

        _mapper = new Mapper(configuration);
    }

    public LessonCircleDataModel? GetLessonCircleById(string lessonId, string circleId) => _mapper.Map<LessonCircleDataModel>(
        _dbContext.LessonCircles.FirstOrDefault(x => x.LessonId == lessonId && x.CircleId == circleId));
}
