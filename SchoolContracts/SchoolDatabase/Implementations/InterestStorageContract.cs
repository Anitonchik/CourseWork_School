using AutoMapper;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDatabase.Implementations;

public class InterestStorageContract:IInterestStorageContract
{
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapper;

    public InterestStorageContract(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;

        var configuration = new MapperConfiguration(cfg => cfg.AddMaps(typeof(Interest)));

        _mapper = new Mapper(configuration);
    }
    public List<InterestDataModel> GetList()
    {
        try
        {
            return [.. _dbContext.Interests.Select(x => _mapper.Map<InterestDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public List<InterestReportDataModel> GetInterestReport(DateTime startDate, DateTime endDate)
    {
        try
        {
            var reportData = from interest in _dbContext.Interests
                             join lessonInterest in _dbContext.LessonInterests on interest.Id equals lessonInterest.InterestId
                             join lesson in _dbContext.Lessons on lessonInterest.LessonId equals lesson.Id
                             join achievement in _dbContext.Achievements on lesson.AchievementId equals achievement.Id
                             join lessonCircle in _dbContext.LessonCircles on lesson.Id equals lessonCircle.LessonId
                             join circle in _dbContext.Circles on lessonCircle.CircleId equals circle.Id
                             where lesson.LessonDate >= startDate && lesson.LessonDate <= endDate
                             select new InterestReportDataModel
                             {
                                 InterestName = interest.InterestName,
                                 AchievementName = achievement.AchievementName,
                                 CircleName = circle.CircleName,
                                 Description = interest.Description,
                                 Date = lesson.LessonDate
                             };

            return reportData.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Error while generating interest report: " + ex.Message);
        }
    }

    public InterestDataModel? GetElementById(string id)
    {
        try
        {
            var interest = GetInterestById(id) ?? throw new ElementNotFoundException(id);
            return _mapper.Map<InterestDataModel>(interest);
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
    public InterestDataModel? GetElementByName(string name)
    {
        try
        {
            return
            _mapper.Map<InterestDataModel>(_dbContext.Interests.FirstOrDefault(x => x.InterestName == name));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public void AddElement(InterestDataModel interestDataModel)
    {
        try
        {
            _dbContext.Interests.Add(_mapper.Map<Interest>(interestDataModel));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", interestDataModel.Id);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public void UpdElement(InterestDataModel interestDataModel)
    {
        try
        {
            var element = GetInterestById(interestDataModel.Id) ?? throw new ElementNotFoundException(interestDataModel.Id);
            _dbContext.Interests.Update(_mapper.Map(interestDataModel, element));
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
    public void DelElement(string id)
    {
        try
        {
            var element = GetInterestById(id) ?? throw new ElementNotFoundException(id);
            _dbContext.Interests.Remove(element);
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
    private Interest? GetInterestById(string id) => _dbContext.Interests.FirstOrDefault(x => x.Id == id);
}
