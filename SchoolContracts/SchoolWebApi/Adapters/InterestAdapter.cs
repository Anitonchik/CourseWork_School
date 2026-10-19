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

public class InterestAdapter:IInterestAdapter
{
    private readonly IInterestBuisnessLogicContract _interestBuisnessLogicContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public InterestAdapter(IInterestBuisnessLogicContract interestBuisnessLogicContract, ILogger<LessonAdapter> logger)
    {
        _interestBuisnessLogicContract = interestBuisnessLogicContract;
        _logger = logger;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<InterestBindingModel, InterestDataModel>();
            cfg.CreateMap<InterestDataModel, InterestViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public InterestOperationResponse GetList(string workerId)
    {
        try
        {
            return InterestOperationResponse.OK([.. _interestBuisnessLogicContract.GetAllInterests(workerId).Select(x => _mapper.Map<InterestViewModel>(x))]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return InterestOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return InterestOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return InterestOperationResponse.InternalServerError(ex.Message);
        }
    }

    public InterestOperationResponse GetElement(string workerId, string data)
    {
        try
        {
            return InterestOperationResponse.OK(_mapper.Map<InterestViewModel>(_interestBuisnessLogicContract.GetInterestByData(workerId, data)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return InterestOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return InterestOperationResponse.NotFound($"Not found element by data {data}");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return InterestOperationResponse.BadRequest($"Worker by id: {workerId} doesn't have access to data by data: {data}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return InterestOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return InterestOperationResponse.InternalServerError(ex.Message);
        }
    }

    public InterestOperationResponse RegisterInterest(string workerId, InterestBindingModel interestModel)
    {
        try
        {
            _interestBuisnessLogicContract.InsertInterest(workerId, _mapper.Map<InterestDataModel>(interestModel));
            return InterestOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return InterestOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return InterestOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return InterestOperationResponse.BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return InterestOperationResponse.BadRequest($"Worker by id: {workerId} doesn't have access to data by id: {interestModel.Id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return InterestOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return InterestOperationResponse.InternalServerError(ex.Message);
        }
    }

    public InterestOperationResponse ChangeInterestInfo(string workerId, InterestBindingModel interestModel)
    {
        try
        {
            _interestBuisnessLogicContract.UpdateInterest(workerId, _mapper.Map<InterestDataModel>(interestModel));
            return InterestOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return InterestOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return InterestOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return InterestOperationResponse.BadRequest($"Not found element by Id {interestModel.Id}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return InterestOperationResponse.BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return InterestOperationResponse.BadRequest($"Worker by id: {workerId} doesn't have access to data by id: {interestModel.Id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return InterestOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return InterestOperationResponse.InternalServerError(ex.Message);
        }
    }

    public InterestOperationResponse RemoveInterest(string workerId, string id)
    {
        try
        {
            _interestBuisnessLogicContract.DeleteInterest(workerId, id);
            return InterestOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return InterestOperationResponse.BadRequest("Id is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return InterestOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return InterestOperationResponse.BadRequest($"Not found element by id: {id}");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return InterestOperationResponse.BadRequest($"Worker by id: {workerId} doesn't have access to data by id: {id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return InterestOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return InterestOperationResponse.InternalServerError(ex.Message);
        }
    }
}
