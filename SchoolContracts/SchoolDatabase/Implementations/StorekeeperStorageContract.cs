using AutoMapper;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;

namespace SchoolDatabase.Implementations;

public class StorekeeperStorageContract : IStorekeeperStorageContract
{
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapper;

    public StorekeeperStorageContract(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Storekeeper, StorekeeperDataModel>();
            cfg.CreateMap<StorekeeperDataModel, Storekeeper>();
        });
        _mapper = new Mapper(config);
    }

    public List<StorekeeperDataModel> GetList()
    {
        try
        {
            return [.. _dbContext.Storekeepers.Select(x => _mapper.Map<StorekeeperDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    public StorekeeperDataModel? GetElementById(string id)
    {
        try
        {
            var Storekeeper = GetStorekeeperById(id) ?? throw new ElementNotFoundException(id);
            return _mapper.Map<StorekeeperDataModel>(Storekeeper);
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

    public StorekeeperDataModel? GetElementByFIO(string fio)
    {
        try
        {
            return _mapper.Map<StorekeeperDataModel>(_dbContext.Storekeepers.FirstOrDefault(x => x.FIO == fio));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    public StorekeeperDataModel? GetElementByLogin(string login)
    {
        try
        {
            return _mapper.Map<StorekeeperDataModel>(_dbContext.Storekeepers.FirstOrDefault(x => x.Login == login));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    public StorekeeperDataModel? GetElementByMail(string mail)
    {
        try
        {
            return _mapper.Map<StorekeeperDataModel>(_dbContext.Storekeepers.FirstOrDefault(x => x.Mail == mail));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    public void AddElement(StorekeeperDataModel StorekeeperDataModel)
    {
        try
        {
            _dbContext.Storekeepers.Add(_mapper.Map<Storekeeper>(StorekeeperDataModel));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", StorekeeperDataModel.Id);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    public void UpdElement(StorekeeperDataModel StorekeeperDataModel)
    {
        try
        {
            var element = GetStorekeeperById(StorekeeperDataModel.Id) ?? throw new ElementNotFoundException(StorekeeperDataModel.Id);
            _dbContext.Storekeepers.Update(_mapper.Map(StorekeeperDataModel, element));
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
            var element = GetStorekeeperById(id) ?? throw new ElementNotFoundException(id);
            _dbContext.Storekeepers.Remove(element);
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

    private Storekeeper? GetStorekeeperById(string id) => _dbContext.Storekeepers.FirstOrDefault(x => x.Id == id);
}
