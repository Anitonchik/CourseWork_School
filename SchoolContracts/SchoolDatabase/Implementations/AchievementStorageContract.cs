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

public class AchievementStorageContract : IAchievementStorageContract
{
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapper;

    public AchievementStorageContract(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Achievement, AchievementDataModel>();
            cfg.CreateMap<AchievementDataModel, Achievement>();
        });
        _mapper = new Mapper(config);
    }
     public List<AchievementDataModel> GetList()
    {
        try
        {
            return [.._dbContext.Achievements.Select(x => _mapper.Map<AchievementDataModel>(x))];
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
            return _mapper.Map<AchievementDataModel>(GetAchievementById(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
     public AchievementDataModel? GetElementByName(string name)
    {
        try
        {
            return
            _mapper.Map<AchievementDataModel>(_dbContext.Achievements.FirstOrDefault(x => x.AchievementName == name));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public void AddElement(AchievementDataModel achievementDataModel)
    {
        throw new NotImplementedException();
    }
    public void UpdElement(AchievementDataModel achievementDataModel)
    {
        throw new NotImplementedException();
    }
    public void DelElement(string id)
    {
        throw new NotImplementedException();
    }
    private Achievement? GetAchievementById(string id) => _dbContext.Achievements.FirstOrDefault(x => x.Id == id);
}
