using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
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
    public List<LessonDataModel> GetList()
    {
        try
        {
            return [.. _dbContext.Lessons.Select(x => _mapper.Map<LessonDataModel>(x))];
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
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { ConstraintName: "IX_Lessons_LessonName" })
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("LessonName", lessonDataModel.LessonName);
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
}
