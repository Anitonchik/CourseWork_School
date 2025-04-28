using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Internal;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.ModelsForReports;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;

namespace SchoolDatabase.Implementations;

public class CircleStorageContract : ICircleStorageContract
{
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapper;

    public CircleStorageContract(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;

        var configuration = new MapperConfiguration(cfg => cfg.AddMaps(typeof(Circle)));

        _mapper = new Mapper(configuration);
    }

    public List<CircleDataModel> GetList()
    {
        try
        {
            return [.. _dbContext.Circles.Select(x => _mapper.Map<CircleDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    public List<CirclesWithInterestsWithMedals> GetCirclesWithInterestsWithMedals(DateTime fromDate, DateTime toDate)
    {
        var sql = $"SELECT c.\"CircleName\" as \"CircleName\", c.\"Description\" as \"CircleDescription\", " +
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

        return _dbContext.Set<CirclesWithInterestsWithMedals>().FromSqlRaw(sql).ToList();

    }

    public CircleDataModel? GetElementById(string id)
    {
        try
        {
            var circle = GetCircleById(id) ?? throw new ElementNotFoundException(id);
            return _mapper.Map<CircleDataModel>(circle);
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

    public CircleDataModel? GetElementByName(string name)
    {
        try
        {
            return _mapper.Map<CircleDataModel>(_dbContext.Circles.FirstOrDefault(x => x.CircleName == name));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    
    public void AddElement(CircleDataModel circleDataModel)
    {
        try
        {
            _dbContext.Circles.Add(_mapper.Map<Circle>(circleDataModel));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", circleDataModel.Id);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    public void UpdElement(CircleDataModel circleDataModel)
    {
        try
        {
            var element = GetCircleById(circleDataModel.Id) ?? throw new ElementNotFoundException(circleDataModel.Id);
            _dbContext.Circles.Update(_mapper.Map(circleDataModel, element));
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
            var element = GetCircleById(id) ?? throw new ElementNotFoundException(id);
            _dbContext.Circles.Remove(element);
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

    private Circle? GetCircleById(string id) => _dbContext.Circles.FirstOrDefault(x => x.Id == id);
}
