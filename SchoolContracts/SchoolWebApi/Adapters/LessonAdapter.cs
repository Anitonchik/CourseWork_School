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

public class LessonAdapter:ILessonAdapter
{
    private readonly ILessonBuisnessLogicContract _lessonBuisnessLogicContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public LessonAdapter(ILessonBuisnessLogicContract lessonBuisnessLogicContract, ILogger<LessonAdapter> logger)
    {
        _lessonBuisnessLogicContract = lessonBuisnessLogicContract;
        _logger = logger;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<LessonBindingModel, LessonDataModel>();
            cfg.CreateMap<LessonDataModel, LessonViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public LessonOperationResponse GetList(string workerId)
    {
        try
        {
            return LessonOperationResponse.OK([.. _lessonBuisnessLogicContract.GetAllLessons(workerId).Select(x => _mapper.Map<LessonViewModel>(x))]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return LessonOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return LessonOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return LessonOperationResponse.InternalServerError(ex.Message);
        }
    }

    public LessonOperationResponse GetElement(string workerId, string data)
    {
        try
        {
            return LessonOperationResponse.OK(_mapper.Map<LessonViewModel>(_lessonBuisnessLogicContract.GetLessonByData(workerId, data)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return LessonOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return LessonOperationResponse.NotFound($"Not found element by data {data}");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return LessonOperationResponse.BadRequest($"Worker by id: {workerId} doesn't have access to data by data: {data}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return LessonOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return LessonOperationResponse.InternalServerError(ex.Message);
        }
    }

    public LessonOperationResponse RegisterLesson(string workerId, LessonBindingModel lessonModel)
    {
        try
        {
            _lessonBuisnessLogicContract.InsertLesson(workerId, _mapper.Map<LessonDataModel>(lessonModel));
            return LessonOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return LessonOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return LessonOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return LessonOperationResponse.BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return LessonOperationResponse.BadRequest($"Worker by id: {workerId} doesn't have access to data by id: {lessonModel.Id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return LessonOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return LessonOperationResponse.InternalServerError(ex.Message);
        }
    }

    public LessonOperationResponse ChangeLessonInfo(string workerId, LessonBindingModel lessonModel)
    {
        try
        {
            _lessonBuisnessLogicContract.InsertLesson(workerId, _mapper.Map<LessonDataModel>(lessonModel));
            return LessonOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return LessonOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return LessonOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return LessonOperationResponse.BadRequest($"Not found element by Id {lessonModel.Id}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return LessonOperationResponse.BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return LessonOperationResponse.BadRequest($"Worker by id: {workerId} doesn't have access to data by id: {lessonModel.Id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return LessonOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return LessonOperationResponse.InternalServerError(ex.Message);
        }
    }

    public LessonOperationResponse RemoveLesson(string workerId, string id)
    {
        try
        {
            _lessonBuisnessLogicContract.DeleteLesson(workerId, id);
            return LessonOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return LessonOperationResponse.BadRequest("Id is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return LessonOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return LessonOperationResponse.BadRequest($"Not found element by id: {id}");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return LessonOperationResponse.BadRequest($"Worker by id: {workerId} doesn't have access to data by id: {id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return LessonOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return LessonOperationResponse.InternalServerError(ex.Message);
        }
    }
}
