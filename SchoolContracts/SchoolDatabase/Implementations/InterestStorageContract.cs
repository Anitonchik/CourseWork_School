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

public class InterestStorageContract:IInterestStorageContract
{
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapper;

    public InterestStorageContract(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<InterestMaterial, InterestMaterialDataModel>();
            cfg.CreateMap<Interest, InterestDataModel>();
            cfg.CreateMap<InterestDataModel, Interest>()
            .ForMember(x => x.InterestMaterials, x => x.MapFrom(src => src.Materials));
        });
        _mapper = new Mapper(config);
    }
    public List<InterestDataModel> GetList()
    {
        try
        {
            return [.. _dbContext.Interests.Select(x => _mapper.Map<InterestDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public InterestDataModel? GetElementById(string id)
    {
        try
        {
            return _mapper.Map<InterestDataModel>(GetInterestById(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public InterestDataModel? GetElementByName(string name)
    {
        try
        {
            return
            _mapper.Map<InterestDataModel>(_dbContext.Interests.FirstOrDefault(x => x.InterestName == name));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public void AddElement(InterestDataModel interestDataModel)
    {
        try
        {
            _dbContext.Interests.Add(_mapper.Map<Interest>(interestDataModel));
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    public void UpdElement(InterestDataModel interestDataModel)
    {
        try
        {
            var element = GetInterestById(interestDataModel.Id);
            _dbContext.Interests.Update(_mapper.Map(interestDataModel, element));
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
            var element = GetInterestById(id);
            _dbContext.Interests.Remove(element);
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    private Interest? GetInterestById(string id) => _dbContext.Interests.FirstOrDefault(x => x.Id == id);
}
