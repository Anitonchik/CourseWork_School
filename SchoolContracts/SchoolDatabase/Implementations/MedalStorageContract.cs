using AutoMapper;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;

namespace SchoolDatabase.Implementations;

public class MedalStorageContract : IMedalStorageContract
{
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapper;

    public MedalStorageContract(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;

        var configuration = new MapperConfiguration(cfg => cfg.AddMaps(typeof(Medal)));

        _mapper = new Mapper(configuration);
    }

    public List<MedalDataModel> GetList()
    {
        try
        {
            return [.. _dbContext.Medals.Select(x => _mapper.Map<MedalDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public MedalDataModel? GetElementById(string id)
    {
        try
        {
            var circle = GetMedalById(id) ?? throw new ElementNotFoundException(id);
            return _mapper.Map<MedalDataModel>(circle);
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

    public MedalDataModel? GetElementByName(string name)
    {
        try
        {
            return _mapper.Map<MedalDataModel>(_dbContext.Medals.FirstOrDefault(x => x.MedalName == name));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public void AddElement(MedalDataModel medalDataModel)
    {
        try
        {
            _dbContext.Medals.Add(_mapper.Map<Medal>(medalDataModel));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", medalDataModel.Id);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    public void UpdElement(MedalDataModel medalDataModel)
    {
        try
        {
            var element = GetMedalById(medalDataModel.Id) ?? throw new ElementNotFoundException(medalDataModel.Id);
            _dbContext.Medals.Update(_mapper.Map(medalDataModel, element));
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
            var element = GetMedalById(id) ?? throw new ElementNotFoundException(id);
            _dbContext.Medals.Remove(element);
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

    private Medal? GetMedalById(string id) => _dbContext.Medals.FirstOrDefault(x => x.Id == id);
}
