using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.ModelsForReports;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;
using SchoolDatabase.Models.ModelsForReports;
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

        var configuration = new MapperConfiguration(cfg => cfg.AddMaps(typeof(Lesson)));

        _mapper = new Mapper(configuration);
    }
    public List<LessonDataModel> GetList(string workerId)
    {
        try
        {
            return [.. _dbContext.Lessons.Where(x => x.WorkerId == workerId).Select(x => _mapper.Map<LessonDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public List<LessonByMaterial> GetLessonsByMaterial(string workerId,string materialId)
    {
        /*try
        {
            var lessons = (from l in _dbContext.Lessons
                           join li in _dbContext.LessonInterests on l.Id equals li.LessonId
                           join i in _dbContext.Interests on li.InterestId equals i.Id
                           join im in _dbContext.InterestMaterials on i.Id equals im.InterestId
                           join m in _dbContext.Materials on im.MaterialId equals m.Id
                           where im.MaterialId == materialId
                           select l).ToList();
           return [.. lessons.Select(x => _mapper.Map<LessonDataModel>(x))];
        }*/
        try
        {
            var sql = $"SELECT   m.\"MaterialName\" as \"MaterialName\",l.\"LessonName\" as \"LessonName\", l.\"Description\" as \"LessonDescription\" " +
                       $"FROM \"Lessons\" l " +
                       $"JOIN \"Workers\" wo ON wo.\"Id\" = l.\"WorkerId\" " +
                       $"JOIN \"LessonInterests\" li ON l.\"Id\" = li.\"LessonId\" " +
                       $"JOIN \"Interests\" i ON li.\"InterestId\" = i.\"Id\" " +
                       $"JOIN \"InterestMaterials\" im ON i.\"Id\" = im.\"InterestId\" " +
                       $"JOIN \"Materials\" m ON im.\"MaterialId\" = m.\"Id\" " +
                       $"WHERE (wo.\"Id\" = '{workerId}' AND  m.\"Id\" = '{materialId}');";

            return _dbContext.Set<LessonByMaterial>().FromSqlRaw(sql).ToList();
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
            var lesson = GetLessonById(id) ?? throw new ElementNotFoundException(id);
            return _mapper.Map<LessonDataModel>(lesson);
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
        catch (InvalidOperationException ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", lessonDataModel.Id);
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
            var element = GetLessonById(lessonDataModel.Id) ?? throw new ElementNotFoundException(lessonDataModel.Id);
            _dbContext.Lessons.Update(_mapper.Map(lessonDataModel, element));
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
            var element = GetLessonById(id) ?? throw new ElementNotFoundException(id);
            _dbContext.Lessons.Remove(element);
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
    private Lesson? GetLessonById(string id) => _dbContext.Lessons.FirstOrDefault(x => x.Id == id);

    public List<LessonDataModel> GetList()
    {
        throw new NotImplementedException();
    }

    public List<LessonByMaterial> GetLessonsByMaterial(string materialId)
    {
        throw new NotImplementedException();
    }
}
