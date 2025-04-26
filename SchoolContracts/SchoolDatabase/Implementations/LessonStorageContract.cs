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

public class LessonStorageContract: ILessonStorageContract
{

    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapper;

    public LessonStorageContract(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<LessonInterest, LessonInterestDataModel>();
            cfg.CreateMap<Lesson, LessonDataModel>();
            cfg.CreateMap<LessonDataModel, Lesson>()
            .ForMember(x => x.LessonInterests, x => x.MapFrom(src => src.Interests));
        });
        _mapper = new Mapper(config);
    }
    public List<LessonDataModel> GetList()
    {
        try
        {
            return [.. _dbContext.Lessons.Select(x => _mapper.Map<LessonDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public LessonDataModel? GetElementById(string id)
    {
        try
        {
            return _mapper.Map<LessonDataModel>(GetLessonById(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public LessonDataModel? GetElementByName(string name)
    {
        try
        {
            return
            _mapper.Map<LessonDataModel>(_dbContext.Lessons.FirstOrDefault(x => x.LessonName == name));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public void AddElement(LessonDataModel lessonDataModel)
    {
        try
        {
            _dbContext.Lessons.Add(_mapper.Map<Lesson>(lessonDataModel));
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public void UpdElement(LessonDataModel lessonDataModel)
    {
        try
        {
            var element = GetLessonById(lessonDataModel.Id);
            _dbContext.Lessons.Update(_mapper.Map(lessonDataModel, element));
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public void DelElement(string id)
    {
        try
        {
            var element = GetLessonById(id);
            _dbContext.Lessons.Remove(element);
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    private Lesson? GetLessonById(string id) => _dbContext.Lessons.FirstOrDefault(x => x.Id == id);
}
