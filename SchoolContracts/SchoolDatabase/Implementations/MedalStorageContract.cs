    using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;

namespace SchoolDatabase.Implementations;

public class MedalStorageContract : IMedalStorageContract
{
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapper;
    private readonly MaterialStorageContract? _materialStorageContract;

    public MedalStorageContract(SchoolDbContext dbContext, MaterialStorageContract? materialStorageContract)
    {
        _dbContext = dbContext;

        var configuration = new MapperConfiguration(cfg => cfg.AddMaps(typeof(Medal)));

        _mapper = new Mapper(configuration);
        _materialStorageContract = materialStorageContract;
    }

    public List<MedalDataModel> GetList(string storekeeperId, int? range)
    {
        try
        {
            var query = _dbContext.Medals.Where(x => x.StorekeeperId == storekeeperId).AsQueryable();
            if (range != null)
            {
                query = query.Where(x => x.Range == range);
            }
            return [.. query.Select(x => _mapper.Map<MedalDataModel>(x))];
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

    public void CreateConnectWithMaterial(string medalId, string materialId)
    {
        try
        {
            if (_materialStorageContract == null)
            {
                throw new Exception("");
            }

            var material = _materialStorageContract.GetElementById(materialId);
            var medal = GetElementById(medalId);

            var newMedal = new MedalDataModel(medal.Id, medal.StorekeeperId, medal.Id, medal.MedalName,
                medal.Range, medal.Description);
            UpdElement(newMedal);
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
