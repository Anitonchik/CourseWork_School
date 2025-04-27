using AutoMapper;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;

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
    public MaterialDataModel? GetElementById(string id)
    {
        try
        {
            var circle = GetMaterialById(id) ?? throw new ElementNotFoundException(id);
            return _mapper.Map<MaterialDataModel>(circle);
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
