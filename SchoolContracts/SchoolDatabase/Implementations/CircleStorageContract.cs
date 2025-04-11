using AutoMapper;
using SchoolContracts.DataModels;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;

namespace SchoolDatabase.Implementations;

public class CircleStorageContract : ICircleStorageContract
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
            .ForMember(x => x.CircleMaterials, x => x.MapFrom(src => src.Materials))
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
        try
        {
            return
            _mapper.Map<CircleDataModel>(_dbContext.Circles.FirstOrDefault(x => x.CircleName == name));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }
    
    public void AddElement(CircleDataModel circleDataModel)
    {
        try
        {
            _dbContext.Circles.Add(_mapper.Map<Circle>(circleDataModel));
            _dbContext.SaveChanges();
        }
        /*catch (InvalidOperationException ex) when (ex.TargetSite?.Name == "ThrowIdentityConflict")
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", buyerDataModel.Id);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { ConstraintName: "IX_Buyers_PhoneNumber" })
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("PhoneNumber", buyerDataModel.PhoneNumber);
        }*/
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    public void UpdElement(CircleDataModel circleDataModel)
    {
        try
        {
            var element = GetCircleById(circleDataModel.Id) /*?? throw new ElementNotFoundException(buyerDataModel.Id)*/;
            _dbContext.Circles.Update(_mapper.Map(circleDataModel, element));
            _dbContext.SaveChanges();
        }
        /*catch (ElementNotFoundException)
        {
            _dbContext.ChangeTracker.Clear();
            throw;
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { ConstraintName: "IX_Buyers_PhoneNumber" })
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("PhoneNumber", buyerDataModel.PhoneNumber);
        }*/
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
            var element = GetCircleById(id) /*?? throw new ElementNotFoundException(id)*/;
            _dbContext.Circles.Remove(element);
            _dbContext.SaveChanges();
        }
        /*catch (ElementNotFoundException)
        {
            _dbContext.ChangeTracker.Clear();
            throw;
        }*/
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new Exception();
        }
    }

    private Circle? GetCircleById(string id) => _dbContext.Circles.FirstOrDefault(x => x.Id == id);
}
