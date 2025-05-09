using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.ModelsForReports;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDatabase.Implementations;

public class InterestStorageContract: IInterestStorageContract
{
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapper;

    public InterestStorageContract(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;

        var configuration = new MapperConfiguration(cfg => cfg.AddMaps(typeof(Interest)));

        _mapper = new Mapper(configuration);
    }
    public List<InterestDataModel> GetList(string workerId)
    {
        try
        {
            return [.. _dbContext.Interests.Where(x => x.WorkerId == workerId).Select(x => _mapper.Map<InterestDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public List<InterestsWithAchievementsWithCircles> GetInterestsWithAchievementsWithCircles(string workerId,DateTime startDate, DateTime endDate)
    {
       /* var sql = $"SELECT c.\"CircleName\" as \"CircleName\", c.\"Description\" as \"CircleDescription\", " +
    $"i.\"InterestName\" as \"InterestName\", md.\"MedalName\" as \"MedalName\" " +
    $"FROM \"Circles\" c" +
    $"JOIN \"CircleMaterials\" cm ON c.\"Id\" = cm.\"CircleId\" " +
    $"JOIN \"Materials\" mt ON cm.\"MaterialId\" = mt.\"Id\" " +
    $"JOIN \"Medals\" md ON md.\"MaterialId\" = cm.\"MaterialId\" " +
    $"JOIN \"InterestMaterials\" im ON im.\"MaterialId\" = mt.\"Id\" " +
    $"JOIN \"Interests\" i ON i.\"Id\" = im.\"InterestId\" " +
    $"JOIN \"LessonInterests\" li ON li.\"InterestId\" = i.\"Id\" " +
    $"JOIN \"Lessons\" l ON l.\"Id\" = li.\"LessonId\" " +
    $"WHERE(l.\"LessonDate\" between {fromDate} and {toDate});";

        return _dbContext.Set<CirclesWithInterestsWithMedals>().FromSqlRaw(sql).ToList();*/
        try
        {
            /*var reportData = from interest in _dbContext.Interests
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

             return reportData.ToList();*/
            var sql = $"SELECT i.\"InterestName\" as \"InterestName\", " +
          $"a.\"AchievementName\" as \"AchievementName\", " +
          $"c.\"CircleName\" as \"CircleName\", " +
          $"i.\"Description\" as \"Description\", " +
          $"l.\"LessonDate\" as \"Date\" " +
          $"FROM \"Interests\" i " +
          $"JOIN \"Workers\" wo ON wo.\"Id\" = i.\"WorkerId\" " +
          $"JOIN \"LessonInterests\" li ON i.\"Id\" = li.\"InterestId\" " +
          $"JOIN \"Lessons\" l ON li.\"LessonId\" = l.\"Id\" " +
          $"JOIN \"Achievements\" a ON l.\"AchievementId\" = a.\"Id\" " +
          $"JOIN \"LessonCircles\" lc ON l.\"Id\" = lc.\"LessonId\" " +
          $"JOIN \"Circles\" c ON lc.\"CircleId\" = c.\"Id\" " +
          $"WHERE wo.\"Id\" = '{workerId}' AND  l.\"LessonDate\" BETWEEN {startDate} AND {endDate};";

            return _dbContext.Set<InterestsWithAchievementsWithCircles>().FromSqlRaw(sql).ToList();
            /*return reportData.Select(x => _mapper.Map<InterestReportDataModel>(new
            {
                x.InterestName,
                x.AchievementName,
                x.CircleName,
                x.Description,
                x.Date
            })).ToList();*/
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

    public List<InterestDataModel> GetList()
    {
        throw new NotImplementedException();
    }

    public List<InterestReportDataModel> GetInterestReport(DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }
}
