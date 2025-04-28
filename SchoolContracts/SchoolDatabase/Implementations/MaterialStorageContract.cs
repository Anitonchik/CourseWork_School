using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;
using SchoolDatabase.Models.ModelsForReports;

namespace SchoolDatabase.Implementations;

public class MaterialStorageContract : IMaterialStorageContract
{
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapper;

    public MaterialStorageContract(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;

        var configuration = new MapperConfiguration(cfg => cfg.AddMaps(typeof(Material)));

        _mapper = new Mapper(configuration);
    }

    public List<MaterialDataModel> GetList()
    {
        try
        {
            return [.. _dbContext.Materials.Select(x => _mapper.Map<MaterialDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    public List<MaterialByLesson> GetMaterialsByLesson(string lessonId)
    {   
        try
        {
            var sql = $"SELECT l.\"LessonName\" as \"LessonName\", mt.\"MaterialName\" as \"MaterialName\", cm.\"Count\" as \"Count\", l.\"Description\" as \"LessonDescription\" " +
                $"FROM \"Materials\" mt " +
                $"JOIN \"CircleMaterials\" cm ON mt.\"Id\" = cm.\"MaterialId\" " +
                $"JOIN \"Circles\" c ON c.\"Id\" = cm.\"CircleId\" " +
                $"JOIN \"LessonCircles\" lc ON lc.\"CircleId\" = c.\"Id\" " +
                $"JOIN \"Lessons\" l ON l.\"Id\" = lc.\"LessonId\" " +
                $"WHERE (l.\"Id\" = '{lessonId}');";

            return _dbContext.Set<MaterialByLesson>().FromSqlRaw(sql).ToList();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    public MaterialDataModel? GetElementById(string id)
    {
        try
        {
            var material = GetMaterialById(id) ?? throw new ElementNotFoundException(id);
            return _mapper.Map<MaterialDataModel>(material);
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

    public MaterialDataModel? GetElementByName(string name)
    {
        try
        {
            return _mapper.Map<MaterialDataModel>(_dbContext.Materials.FirstOrDefault(x => x.MaterialName == name));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public void AddElement(MaterialDataModel materialDataModel)
    {
        try
        {
            _dbContext.Materials.Add(_mapper.Map<Material>(materialDataModel));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", materialDataModel.Id);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    
    public void UpdElement(MaterialDataModel materialDataModel)
    {
        try
        {
            var element = GetMaterialById(materialDataModel.Id) ?? throw new ElementNotFoundException(materialDataModel.Id);
            _dbContext.Materials.Update(_mapper.Map(materialDataModel, element));
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
            var element = GetMaterialById(id) ?? throw new ElementNotFoundException(id);
            _dbContext.Materials.Remove(element);
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

    private Material? GetMaterialById(string id) => _dbContext.Materials.FirstOrDefault(x => x.Id == id);
}
