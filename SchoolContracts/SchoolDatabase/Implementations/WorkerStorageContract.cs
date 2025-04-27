using AutoMapper;
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

public class WorkerStorageContract: IWorkerStorageContract
{
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapper;

    public WorkerStorageContract(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;

        var configuration = new MapperConfiguration(cfg => cfg.AddMaps(typeof(Worker)));

        _mapper = new Mapper(configuration);
    }
    public List<WorkerDataModel> GetList()
    {
        try
        {
            return [.. _dbContext.Workers.Select(x => _mapper.Map<WorkerDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public WorkerDataModel? GetElementById(string id)
    {
        try
        {
            var Worker = GetWorkerById(id) ?? throw new ElementNotFoundException(id);
            return _mapper.Map<WorkerDataModel>(Worker);
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
    public WorkerDataModel? GetElementByFIO(string fio)
    {
        try
        {
            return
            _mapper.Map<WorkerDataModel>(_dbContext.Workers.FirstOrDefault(x => x.FIO == fio));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public WorkerDataModel? GetElementByLogin(string login)
    {
        try
        {
            return
            _mapper.Map<WorkerDataModel>(_dbContext.Workers.FirstOrDefault(x => x.Login == login));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public WorkerDataModel? GetElementByMail(string mail)
    {
        try
        {
            return
            _mapper.Map<WorkerDataModel>(_dbContext.Workers.FirstOrDefault(x => x.Mail == mail));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public void AddElement(WorkerDataModel workerDataModel)
    {
        try
        {
            _dbContext.Workers.Add(_mapper.Map<Worker>(workerDataModel));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", workerDataModel.Id);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public void UpdElement(WorkerDataModel workerDataModel)
    {
        try
        {
            var element = GetWorkerById(workerDataModel.Id) ?? throw new ElementNotFoundException(workerDataModel.Id);
            _dbContext.Workers.Update(_mapper.Map(workerDataModel, element));
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
            var element = GetWorkerById(id) ?? throw new ElementNotFoundException(id);
            _dbContext.Workers.Remove(element);
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
    private Worker? GetWorkerById(string id) => _dbContext.Workers.FirstOrDefault(x => x.Id == id);
}
