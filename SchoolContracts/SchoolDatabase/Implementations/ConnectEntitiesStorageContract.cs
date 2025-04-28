using AutoMapper;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;

namespace SchoolDatabase.Implementations;

public class ConnectEntitiesStorageContract : IConnectEntitiesStorageContract
{
    private readonly SchoolDbContext _dbContext;
    private readonly Mapper _mapperLessonCircle;
    private readonly Mapper _mapperMaterialCircle;
    private readonly Mapper _mapperLessonInterest;
    private readonly Mapper _mapperInterestMaterial;
    private readonly CircleStorageContract _circleStorageContract;
    private readonly LessonStorageContract _lessonStorageContract;
    private readonly MaterialStorageContract _materialStorageContract;
    private readonly InterestStorageContract _interestStorageContract;


    public ConnectEntitiesStorageContract(SchoolDbContext dbContext, 
        CircleStorageContract? circleStorageContract, LessonStorageContract? lessonStorageContract,
        MaterialStorageContract materialStorageContract , InterestStorageContract? interestStorageContract)
    {
        _dbContext = dbContext;

        var configurationLessonCircle = new MapperConfiguration(cfg => cfg.AddMaps(typeof(LessonCircle)));
        var configurationMaterialCircle = new MapperConfiguration(cfg => cfg.AddMaps(typeof(CircleMaterial)));

        var configurationLessonInterest = new MapperConfiguration(cfg => cfg.AddMaps(typeof(LessonInterest)));
        var configurationInterestMaterial = new MapperConfiguration(cfg => cfg.AddMaps(typeof(InterestMaterial)));
       
        _mapperLessonCircle = new Mapper(configurationLessonCircle);
        _mapperMaterialCircle = new Mapper(configurationMaterialCircle);

        _mapperLessonInterest = new Mapper(configurationLessonInterest);
        _mapperInterestMaterial = new Mapper(configurationInterestMaterial);

        _circleStorageContract = circleStorageContract;
        _lessonStorageContract = lessonStorageContract;
        _materialStorageContract = materialStorageContract;
        _interestStorageContract = interestStorageContract;
    }

    public void CreateConnectLessonAndCircle(string lessonId, string circleId)
    {
        try
        {
            _circleStorageContract.GetElementById(circleId);
            _lessonStorageContract.GetElementById(lessonId);

            var lessonCircleDataModel = new LessonCircleDataModel(circleId, lessonId);
            _dbContext.LessonCircles.Add(_mapperLessonCircle.Map<LessonCircle>(lessonCircleDataModel));
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

    public void DeleteConnectLessonAndCircle(string lessonId, string circleId)
    {
        try
        {
            _circleStorageContract.GetElementById(circleId);
            _lessonStorageContract.GetElementById(lessonId);

            var element = GetLessonCircleById(lessonId, circleId) ?? throw new ConnectElementsNotFoundException(lessonId, circleId);
            _dbContext.LessonCircles.Remove(element);
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

    public void CreateConnectCircleAndMaterial(string circleId, string materialId, int count = 1)
    {
        try
        {
            _circleStorageContract.GetElementById(circleId);
            _materialStorageContract.GetElementById(materialId);

            var circleMaterialDataModel = new CircleMaterialDataModel(circleId, materialId, count);
            _dbContext.CircleMaterials.Add(_mapperMaterialCircle.Map<CircleMaterial>(circleMaterialDataModel));
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

    public void DeleteConnectCircleAndMaterial(string circleId, string materialId, int count = 1)
    {
        try
        {
            _circleStorageContract.GetElementById(circleId);
            _materialStorageContract.GetElementById(materialId);

            var element = GetCircleMaterialById(circleId, materialId, count) ?? throw new ConnectElementsNotFoundException(circleId, materialId);
            _dbContext.CircleMaterials.Remove(element);
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

    public void CreateConnectLessonAndInterest(string lessonId, string interestId)
    {
        try
        {
            _interestStorageContract.GetElementById(interestId);
            _lessonStorageContract.GetElementById(lessonId);

            var lessonInterestDataModel = new LessonInterestDataModel(interestId, lessonId);
            _dbContext.LessonInterests.Add(_mapperLessonInterest.Map<LessonInterest>(lessonInterestDataModel));
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

    public void DeleteConnectLessonAndInterest(string lessonId, string interestId)
    {
        try
        {
            _interestStorageContract.GetElementById(interestId);
            _lessonStorageContract.GetElementById(lessonId);

            var element = GetLessonInterestById(lessonId, interestId) ?? throw new ConnectElementsNotFoundException(lessonId, interestId);
            _dbContext.LessonInterests.Remove(element);
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

    public void CreateConnectInterestAndMaterial(string interestId, string materialId)
    {
        try
        {
            _interestStorageContract.GetElementById(interestId);
            _materialStorageContract.GetElementById(materialId);

            var interestMaterialDataModel = new InterestMaterialDataModel(interestId, materialId);
            _dbContext.InterestMaterials.Add(_mapperInterestMaterial.Map<InterestMaterial>(interestMaterialDataModel));
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

    public void DeleteConnectInterestAndMaterial(string interestId, string materialId)
    {
        try
        {
            _interestStorageContract.GetElementById(interestId);
            _materialStorageContract.GetElementById(materialId);

            var element = GetInterestMaterialById(interestId, materialId) ?? throw new ConnectElementsNotFoundException(interestId, materialId);
            _dbContext.InterestMaterials.Remove(element);
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

    private LessonCircle? GetLessonCircleById(string lessonId, string circleId) => 
        _dbContext.LessonCircles.FirstOrDefault(x => x.LessonId == lessonId && x.CircleId == circleId);

    private CircleMaterial? GetCircleMaterialById(string circleId, string materialId, int count) =>
        _dbContext.CircleMaterials.FirstOrDefault(x => x.CircleId == circleId && x.MaterialId == materialId && x.Count == count);

    private LessonInterest? GetLessonInterestById(string lessonId, string interestId) =>
        _dbContext.LessonInterests.FirstOrDefault(x => x.LessonId == lessonId && x.InterestId == interestId);
    private InterestMaterial? GetInterestMaterialById(string interestId, string materialId) =>
       _dbContext.InterestMaterials.FirstOrDefault(x => x.InterestId == interestId && x.MaterialId == materialId);
}
