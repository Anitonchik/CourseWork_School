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
    public async Task<List<InterestsWithAchievementsWithCirclesModel>> GetInterestsWithAchievementsWithCircles(string workerId,DateTime startDate, DateTime endDate, CancellationToken ct)
    {
        try
        {
            var sql = $"SELECT i.\"InterestName\" as \"InterestName\", " +
          $"a.\"AchievementName\" as \"AchievementName\", " +
          $"c.\"CircleName\" as \"CircleName\", " +
          $"i.\"Description\" as \"InterestDescription\", " +
          $"l.\"LessonDate\" as \"Date\" " +
          $"FROM \"Interests\" i " +
          $"JOIN \"Workers\" wo ON wo.\"Id\" = i.\"WorkerId\" " +
          $"JOIN \"LessonInterests\" li ON i.\"Id\" = li.\"InterestId\" " +
          $"JOIN \"Lessons\" l ON li.\"LessonId\" = l.\"Id\" " +
          $"JOIN \"Achievements\" a ON a.\"LessonId\" = l.\"Id\" " +
          $"JOIN \"LessonCircles\" lc ON l.\"Id\" = lc.\"LessonId\" " +
          $"JOIN \"Circles\" c ON lc.\"CircleId\" = c.\"Id\" " +
          $"WHERE (wo.\"Id\" = '{workerId}' AND  l.\"LessonDate\" BETWEEN '{startDate}' AND '{endDate}');";

            return await _dbContext.Set<InterestsWithAchievementsWithCirclesModel>().FromSqlRaw(sql).ToListAsync(ct);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
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

    /*public List<InterestReportDataModel> GetInterestReport(string workerId, DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }*/
}
