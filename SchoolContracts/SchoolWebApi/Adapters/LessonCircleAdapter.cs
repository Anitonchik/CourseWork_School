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

public class LessonCircleAdapter : ILessonCircleAdapter
{
    private readonly ILessonCircleBuisnessLogicContract _lessonCircleBuisnessLogicContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public LessonCircleAdapter(ILessonCircleBuisnessLogicContract lessonCircleBuisnessLogicContract, ILogger<LessonCircleAdapter> logger)
    {
        _lessonCircleBuisnessLogicContract = lessonCircleBuisnessLogicContract;
        _logger = logger;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<LessonCircleBindingModel, LessonCircleDataModel>();
            cfg.CreateMap<LessonCircleDataModel, LessonCircleViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public LessonCircleOperationResponse RegisterLessonCircle(string storekeeperId, string circleId, string lessonId, int count)
    {
        try
        {
            _lessonCircleBuisnessLogicContract.CreateLessonCircle(storekeeperId, circleId, lessonId, count);
            return LessonCircleOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return LessonCircleOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return LessonCircleOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return LessonCircleOperationResponse.BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return LessonCircleOperationResponse.BadRequest($"Storekeeper by id: {storekeeperId} doesn't have access to data by id: {circleModel.Id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return LessonCircleOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return LessonCircleOperationResponse.InternalServerError(ex.Message);
        }
    }

    public LessonCircleOperationResponse RemoveLessonCircle(string storekeeperId, string lessonId, string circleId)
    {
        try
        {
            _lessonCircleBuisnessLogicContract.DeleteLessonCircle(storekeeperId, lessonId, circleId);
            return LessonCircleOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return LessonCircleOperationResponse.BadRequest("Id is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return LessonCircleOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return LessonCircleOperationResponse.BadRequest($"Not found element by id: {circleId}");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return LessonCircleOperationResponse.BadRequest($"Storekeeper by id: {storekeeperId} doesn't have access to data by id: {circleId}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return LessonCircleOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return LessonCircleOperationResponse.InternalServerError(ex.Message);
        }
    }
}
