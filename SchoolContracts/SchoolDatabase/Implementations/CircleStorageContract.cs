using AutoMapper;
using SchoolContracts.DataModels;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;

namespace SchoolDatabase.Implementations;

/*public class CircleStorageContract : ICircleStorageContract
{
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapper;

    public CircleStorageContract(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CircleMaterial, CircleMaterialDataModel>();
            cfg.CreateMap<LessonCircle, LessonCircleDataModel>();
            cfg.CreateMap<Circle, CircleDataModel>();
            cfg.CreateMap<CircleDataModel, Circle>()
            .ForMember(x => x.CircleMaterials, x => x.MapFrom(src => src.Materails))
            .ForMember(x => x.LessonCircles, x => x.MapFrom(src => src.Lessons));
        });
        _mapper = new Mapper(config);
    }

    public List<CircleDataModel> GetList()
    {
        try
        {
            return [.. _dbContext.Circles.Select(x => _mapper.Map<CircleDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    public CircleDataModel? GetElementById(string id)
    {
        try
        {
            return _mapper.Map<CircleDataModel>(GetCircleById(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    public CircleDataModel? GetElementByName(string name)
    {
        throw new NotImplementedException();
    }
    
    public void AddElement(CircleDataModel circleDataModel)
    {
        throw new NotImplementedException();
    }

    public void DelElement(string id)
    {
        throw new NotImplementedException();
    }

    
    public void UpdElement(CircleDataModel circleDataModel)
    {
        throw new NotImplementedException();
    }

    private Circle? GetCircleById(string id) => _dbContext.Circles.FirstOrDefault(x => x.Id == id);
}*/
