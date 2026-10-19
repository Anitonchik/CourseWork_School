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

public class LessonInterestAdapter : ILessonInterestAdapter
{
    private readonly ILessonInterestBuisnessLogicContract _lessonInterestBuisnessLogicContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public LessonInterestAdapter(ILessonInterestBuisnessLogicContract lessonInterestBuisnessLogicContract, ILogger<LessonInterestAdapter> logger)
    {
        _lessonInterestBuisnessLogicContract = lessonInterestBuisnessLogicContract;
        _logger = logger;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<LessonInterestBindingModel, LessonInterestDataModel>();
            cfg.CreateMap<LessonInterestDataModel, LessonInterestViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public LessonInterestOperationResponse RegisterLessonInterest(string workerId, LessonBindingModel lessonModel, LessonInterestBindingModel lessonInterestModel)
    {
        try
        {
            _lessonInterestBuisnessLogicContract.CreateLessonInterest(workerId, _mapper.Map<LessonDataModel>(lessonModel), _mapper.Map<LessonInterestDataModel>(lessonInterestModel));
            return LessonInterestOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return LessonInterestOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return LessonInterestOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return LessonInterestOperationResponse.BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return LessonInterestOperationResponse.BadRequest($"Worker by id: {workerId} doesn't have access to data by id: {lessonModel.Id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return LessonInterestOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return LessonInterestOperationResponse.InternalServerError(ex.Message);
        }
    }

    public LessonInterestOperationResponse RemoveLessonInterest(string workerId, string lessonId, string interestId)
    {
        try
        {
            _lessonInterestBuisnessLogicContract.DeleteLessonInterest(workerId, lessonId, interestId);
            return LessonInterestOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return LessonInterestOperationResponse.BadRequest("Id is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return LessonInterestOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return LessonInterestOperationResponse.BadRequest($"Not found element by id: {interestId}");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return LessonInterestOperationResponse.BadRequest($"Worker by id: {workerId} doesn't have access to data by id: {interestId}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return LessonInterestOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return LessonInterestOperationResponse.InternalServerError(ex.Message);
        }
    }
}
