using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.EntityFrameworkCore;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.ModelsForReports;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;
using System;
using System.Collections.Generic;

namespace SchoolDatabase.Implementations;

public class CircleStorageContract : ICircleStorageContract
{
    private readonly ICircleMaterialStorageContract _circleMaterialStorageContract;
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapper;

    public CircleStorageContract(ICircleMaterialStorageContract circleMaterialStorageContract, SchoolDbContext dbContext)
    {
        _circleMaterialStorageContract = circleMaterialStorageContract;

        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Storekeeper, StorekeeperDataModel>();

            cfg.CreateMap<CircleMaterial, CircleMaterialDataModel>()
            .ConstructUsing(src => new CircleMaterialDataModel(
           src.CircleId,
           src.MaterialId,
           src.Count,
           src.Material != null ? new MaterialDataModel(src.Material.Id, src.Material.StorekeeperId, src.Material.MaterialName, src.Material.Description) : null));
            cfg.CreateMap<CircleMaterialDataModel, CircleMaterial>();

            cfg.CreateMap<LessonCircle, LessonCircleDataModel>();
            cfg.CreateMap<LessonCircleDataModel, LessonCircle>();

            cfg.CreateMap<Circle, CircleDataModel>();
            cfg.CreateMap<CircleDataModel, Circle>()
            .ForMember(x => x.CircleMaterials, x => x.MapFrom(src => src.Materials));
        });
        _mapper = new Mapper(config);
    }

    public List<CircleDataModel> GetList(string storekeeperId)
    {
        try
        {
            
            List<CircleDataModel> data1 = [.. _dbContext.Circles
                .Include(x => x.CircleMaterials)!.ThenInclude(x => x.Material)
                .Include(x => x.LessonCircles)!.ThenInclude(x => x.Lesson)
                .Where(x => x.StorekeeperId == storekeeperId).Select(x => _mapper.Map<CircleDataModel>(x))];

            return data1;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    public async Task<List<CirclesWithInterestsWithMedalsModel>> GetCirclesWithInterestsWithMedals(string storekeeperId, DateTime fromDate, DateTime toDate, CancellationToken ct)
    {
        try
        {
            var sql = $"SELECT c.\"CircleName\" as \"CircleName\", c.\"Description\" as \"CircleDescription\", " +
                $"i.\"InterestName\" as \"InterestName\", md.\"MedalName\" as \"MedalName\", l.\"LessonDate\" as \"Date\" " +
                $"FROM \"Circles\" c " +
                $"JOIN \"Storekeepers\" st ON st.\"Id\" = c.\"StorekeeperId\" " +
                $"JOIN \"CircleMaterials\" cm ON c.\"Id\" = cm.\"CircleId\" " +
                $"JOIN \"Materials\" mt ON cm.\"MaterialId\" = mt.\"Id\" " +
                $"JOIN \"Medals\" md ON md.\"MaterialId\" = cm.\"MaterialId\" " +
                $"JOIN \"InterestMaterials\" im ON im.\"MaterialId\" = mt.\"Id\" " +
                $"JOIN \"Interests\" i ON i.\"Id\" = im.\"InterestId\" " +
                $"JOIN \"LessonInterests\" li ON li.\"InterestId\" = i.\"Id\" " +
                $"JOIN \"Lessons\" l ON l.\"Id\" = li.\"LessonId\" " +
                $"WHERE(st.\"Id\" = '{storekeeperId}' AND l.\"LessonDate\" between '{fromDate}' and '{toDate}');";

            return await _dbContext.Set<CirclesWithInterestsWithMedalsModel>().FromSqlRaw(sql).ToListAsync(ct);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }

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
            var data = _mapper.Map<Circle>(circleDataModel);
            _dbContext.Circles.Add(data);
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

            if (element.CircleMaterials != null)
            {
                _dbContext.CircleMaterials.RemoveRange(element.CircleMaterials);
            }


            /*if (circleDataModel.Materials.Count > 0)
            {
                foreach (CircleMaterialDataModel circleMaterial in  circleDataModel.Materials)
                {
                    _circleMaterialStorageContract.UpdElement(circleMaterial);
                }
            }*/

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

    private Circle? GetCircleById(string id) => _dbContext.Circles
        .Include(x => x.CircleMaterials)!.ThenInclude(x => x.Material)
                .Include(x => x.LessonCircles)!.ThenInclude(x => x.Lesson).FirstOrDefault(x => x.Id == id);
}
