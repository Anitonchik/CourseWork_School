using AutoMapper;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;

namespace SchoolDatabase.Implementations;

public class CircleMaterialStorageContract : ICircleMaterialStorageContract
{
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapper;

    public CircleMaterialStorageContract(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CircleMaterial, CircleMaterialDataModel>()
            .ConstructUsing(src => new CircleMaterialDataModel(
               src.CircleId,
               src.MaterialId,
               src.Count,
               src.Material != null ? new MaterialDataModel(src.Material.Id, src.Material.StorekeeperId, src.Material.MaterialName, src.Material.Description) : null));
            cfg.CreateMap<CircleMaterialDataModel, CircleMaterial>();
        });
        _mapper = new Mapper(config);
    }

    public void UpdElement(CircleMaterialDataModel circleMaterialDataModel)
    {
        try
        {
            var element = GetCircleMaterialById(circleMaterialDataModel.CircleId, circleMaterialDataModel.MaterialId) ?? throw new ElementNotFoundException(circleMaterialDataModel.CircleId);


            _dbContext.CircleMaterials.Update(_mapper.Map(circleMaterialDataModel, element));
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

    public CircleMaterial? GetCircleMaterialById(string circleId, string materialId) => _dbContext.CircleMaterials.FirstOrDefault(x => x.CircleId == circleId && x.MaterialId == materialId);
}
