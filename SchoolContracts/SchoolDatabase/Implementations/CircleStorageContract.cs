using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Internal;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
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

    public List<CircleDataModel> GetCirclesWithInterestsWithMedals(DateTime fromDate, DateTime toDate)
    {
        /*SELECT c."Id", c."StorekeeperId", c."CircleName", c."Description",
md."MedalName", i."InterestName", l."LessonDate"
FROM "Circles" c
JOIN "CircleMaterials" cm ON c."Id" = cm."CircleId"
JOIN "Materials" mt ON cm."MaterialId" = mt."Id"
JOIN "Medals" md ON md."MaterialId" = cm."MaterialId"
JOIN "InterestMaterials" im ON im."MaterialId" = mt."Id"
JOIN "Interests" i ON i."Id" = im."InterestId"
JOIN "LessonInterests" li ON li."InterestId" = i."Id"

JOIN "Lessons" l ON l."Id" = li."LessonId"
WHERE(l."LessonDate" = 'date');*/

        var circles = (from c in _dbContext.Circles
                      join cm in _dbContext.CircleMaterials on c.Id equals cm.MaterialId
                      join mt in _dbContext.Materials on cm.MaterialId equals mt.Id
                      join md in _dbContext.Medals on cm.MaterialId equals md.MaterialId
                      join im in _dbContext.InterestMaterials on mt.Id equals im.MaterialId
                      join i in _dbContext.Interests on im.InterestId equals i.Id
                      join li in _dbContext.LessonInterests on i.Id equals li.InterestId
                      join l in _dbContext.Lessons on li.LessonId equals l.Id
                      where l.LessonDate >= fromDate && l.LessonDate <= toDate
                      select c).ToList();

        return [.. circles.Select(x => _mapper.Map<CircleDataModel>(x))];

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
