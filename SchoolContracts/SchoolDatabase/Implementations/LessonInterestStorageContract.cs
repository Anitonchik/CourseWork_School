using AutoMapper;
using SchoolContracts.DataModels;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDatabase.Implementations;

public class LessonInterestStorageContract : ILessonInterestStorageContract
{
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapper;
    public LessonInterestStorageContract(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;

        var configuration = new MapperConfiguration(cfg => cfg.AddMaps(typeof(LessonInterest)));

        _mapper = new Mapper(configuration);
    }

    public LessonInterestDataModel? GetLessonInterestById(string lessonId, string interestId) => _mapper.Map<LessonInterestDataModel>(
        _dbContext.LessonInterests.FirstOrDefault(x => x.LessonId == lessonId && x.InterestId == interestId));
}
