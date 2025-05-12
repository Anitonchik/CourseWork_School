using AutoMapper;
using SchoolContracts.AdapterContracts;
using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.BindingModels;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.ViewModels;
using System.ComponentModel.DataAnnotations;
using UnauthorizedAccessException = SchoolContracts.Exceptions.UnauthorizedAccessException;

namespace SchoolWebApi.Adapters;

public class AchievementAdapter:IAchievementAdapter
{
    private readonly IAchievementBuisnessLogicContract _achievementBuisnessLogicContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public AchievementAdapter(IAchievementBuisnessLogicContract AchievementBuisnessLogicContract, ILogger<AchievementAdapter> logger)
    {
        _achievementBuisnessLogicContract = AchievementBuisnessLogicContract;
        _logger = logger;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AchievementBindingModel, AchievementDataModel>();
            cfg.CreateMap<AchievementDataModel, AchievementViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public AchievementOperationResponse GetList(string workerId)
    {
        try
        {
            return AchievementOperationResponse.OK([.. _achievementBuisnessLogicContract.GetAllAchievements(workerId).Select(x => _mapper.Map<AchievementViewModel>(x))]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return AchievementOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return AchievementOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return AchievementOperationResponse.InternalServerError(ex.Message);
        }
    }
    public AchievementOperationResponse GetElement(string workerId, string id)
    {
        try
        {
            return AchievementOperationResponse.OK(_mapper.Map<AchievementViewModel>(_achievementBuisnessLogicContract.GetAchievementById(workerId, id)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return AchievementOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return AchievementOperationResponse.NotFound($"Not found element by data {id}");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return AchievementOperationResponse.BadRequest($"Worker by id: {workerId} doesn't have access to data by data: {id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return AchievementOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return AchievementOperationResponse.InternalServerError(ex.Message);
        }
    }

    public AchievementOperationResponse RegisterAchievement(string workerId, AchievementBindingModel achievementModel)
    {
        try
        {
            _achievementBuisnessLogicContract.InsertAchievement(workerId, _mapper.Map<AchievementDataModel>(achievementModel));
            return AchievementOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return AchievementOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return AchievementOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return AchievementOperationResponse.BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return AchievementOperationResponse.BadRequest($"Worker by id: {workerId} doesn't have access to data by id: {achievementModel.Id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return AchievementOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return AchievementOperationResponse.InternalServerError(ex.Message);
        }
    }

    public AchievementOperationResponse ChangeAchievementInfo(string workerId, AchievementBindingModel achievementModel)
    {
        try
        {
            _achievementBuisnessLogicContract.InsertAchievement(workerId, _mapper.Map<AchievementDataModel>(achievementModel));
            return AchievementOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return AchievementOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return AchievementOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return AchievementOperationResponse.BadRequest($"Not found element by Id {achievementModel.Id}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return AchievementOperationResponse.BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return AchievementOperationResponse.BadRequest($"Worker by id: {workerId} doesn't have access to data by id: {achievementModel.Id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return AchievementOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return AchievementOperationResponse.InternalServerError(ex.Message);
        }
    }

    public AchievementOperationResponse RemoveAchievement(string workerId, string id)
    {
        try
        {
            _achievementBuisnessLogicContract.DeleteAchievement(workerId, id);
            return AchievementOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return AchievementOperationResponse.BadRequest("Id is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return AchievementOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return AchievementOperationResponse.BadRequest($"Not found element by id: {id}");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return AchievementOperationResponse.BadRequest($"Worker by id: {workerId} doesn't have access to data by id: {id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return AchievementOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return AchievementOperationResponse.InternalServerError(ex.Message);
        }
    }
}
