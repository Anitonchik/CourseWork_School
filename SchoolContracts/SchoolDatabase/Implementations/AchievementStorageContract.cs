using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NUnit.Framework.Constraints.Tolerance;

namespace SchoolDatabase.Implementations;

public class AchievementStorageContract : IAchievementStorageContract
{
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapper;
    //private readonly LessonStorageContract? _lessonStorageContract;

    public AchievementStorageContract(SchoolDbContext dbContext/*, LessonStorageContract? lessonStorageContract*/)
    {
        _dbContext = dbContext;

        var configuration = new MapperConfiguration(cfg => cfg.AddMaps(typeof(Achievement)));

        _mapper = new Mapper(configuration);
        //_lessonStorageContract = lessonStorageContract;
    }
     public List<AchievementDataModel> GetList(string workerId)
    {
        try
        {
            var query = _dbContext.Achievements.Where(x => x.WorkerId == workerId).AsQueryable();
            return [.. query.Select(x => _mapper.Map<AchievementDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public AchievementDataModel? GetElementById(string id)
    {
        try
        {
            var achievement = GetAchievementById(id) ?? throw new ElementNotFoundException(id);
            return _mapper.Map<AchievementDataModel>(achievement);
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
    /*public void CreateConnectWithLesson(string achievementId, string lessonId)
    {
        try
        {
            if (_lessonStorageContract == null)
            {
                throw new Exception("");
            }

            var lesson = _lessonStorageContract.GetElementById(lessonId);
            var achievement = GetElementById(achievementId);

            var newAchievement = new AchievementDataModel(achievement.Id, achievement.WorkerId, achievement.Id, achievement.AchievementName,
                achievement.Description);
            UpdElement(newAchievement);
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
    }*/
    public void AddElement(AchievementDataModel achievementDataModel)
    {
        try
        {
            _dbContext.Achievements.Add(_mapper.Map<Achievement>(achievementDataModel));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", achievementDataModel.Id);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public void UpdElement(AchievementDataModel achievementDataModel)
    {
        try
        {
            var element = GetAchievementById(achievementDataModel.Id) ?? throw new ElementNotFoundException(achievementDataModel.Id);
            _dbContext.Achievements.Update(_mapper.Map(achievementDataModel, element));
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
            var element = GetAchievementById(id) ?? throw new ElementNotFoundException(id);
            _dbContext.Achievements.Remove(element);
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
    private Achievement? GetAchievementById(string id) => _dbContext.Achievements.FirstOrDefault(x => x.Id == id);
}
