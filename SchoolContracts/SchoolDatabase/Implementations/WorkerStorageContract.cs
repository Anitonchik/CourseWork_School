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

public class WorkerStorageContract: IWorkerStorageContract
{
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapper;

    public WorkerStorageContract(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Worker, WorkerDataModel>();
            cfg.CreateMap<WorkerDataModel, Worker>();
        });
        _mapper = new Mapper(config);
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
            return _mapper.Map<WorkerDataModel>(GetWorkerById(id));
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
            var element = GetWorkerById(workerDataModel.Id);
            _dbContext.Workers.Update(_mapper.Map(workerDataModel, element));
            _dbContext.SaveChanges();
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
            var element = GetWorkerById(id);
            _dbContext.Workers.Remove(element);
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    private Worker? GetWorkerById(string id) => _dbContext.Workers.FirstOrDefault(x => x.Id == id);
}
