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

public class CircleAdapter : ICircleAdapter
{
    private readonly ICircleBuisnessLogicContract _circleBuisnessLogicContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public CircleAdapter(ICircleBuisnessLogicContract circleBuisnessLogicContract, ILogger<CircleAdapter> logger)
    {
        _circleBuisnessLogicContract = circleBuisnessLogicContract;
        _logger = logger;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CircleBindingModel, CircleDataModel>();
            cfg.CreateMap<CircleDataModel, CircleViewModel>();

            cfg.CreateMap<CircleMaterialBindingModel, CircleMaterialDataModel>();
            cfg.CreateMap<CircleMaterialDataModel, CircleMaterialViewModel>();
            
            cfg.CreateMap<LessonCircleBindingModel, LessonCircleDataModel>();
            cfg.CreateMap<LessonCircleDataModel, LessonCircleViewModel>();

        });
        _mapper = new Mapper(config);
    }

    public CircleOperationResponse GetList(string storekeeperId)
    {
        try
        {
            return CircleOperationResponse.OK([.. _circleBuisnessLogicContract.GetAllCircles(storekeeperId).Select(x => _mapper.Map<CircleViewModel>(x))]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return CircleOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return CircleOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return CircleOperationResponse.InternalServerError(ex.Message);
        }
    }
    
    public CircleOperationResponse GetElement(string storekeeperId, string data)
    {
        try
        {
            return CircleOperationResponse.OK(_mapper.Map<CircleViewModel>(_circleBuisnessLogicContract.GetCircleByData(storekeeperId, data)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return CircleOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return CircleOperationResponse.NotFound($"Not found element by data {data}");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return CircleOperationResponse.BadRequest($"Storekeeper by id: {storekeeperId} doesn't have access to data by data: {data}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return CircleOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return CircleOperationResponse.InternalServerError(ex.Message);
        }
    }

    public CircleOperationResponse RegisterCircle(string storekeeperId, CircleBindingModel circleModel)
    {
        try
        {
            var data = _mapper.Map<CircleDataModel>(circleModel);
            _circleBuisnessLogicContract.InsertCircle(storekeeperId, data);
            return CircleOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return CircleOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return CircleOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return CircleOperationResponse.BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return CircleOperationResponse.BadRequest($"Storekeeper by id: {storekeeperId} doesn't have access to data by id: {circleModel.Id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return CircleOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return CircleOperationResponse.InternalServerError(ex.Message);
        }
    }

    public CircleOperationResponse ChangeCircleInfo(string storekeeperId, CircleBindingModel circleModel)
    {
        try
        {
            _circleBuisnessLogicContract.UpdateCircle(storekeeperId, _mapper.Map<CircleDataModel>(circleModel));
            return CircleOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return CircleOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return CircleOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return CircleOperationResponse.BadRequest($"Not found element by Id {circleModel.Id}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return CircleOperationResponse.BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return CircleOperationResponse.BadRequest($"Storekeeper by id: {storekeeperId} doesn't have access to data by id: {circleModel.Id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return CircleOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return CircleOperationResponse.InternalServerError(ex.Message);
        }
    }

    public CircleOperationResponse RemoveCircle(string storekeeperId, string id)
    {
        try
        {
            _circleBuisnessLogicContract.DeleteCircle(storekeeperId, id);
            return CircleOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return CircleOperationResponse.BadRequest("Id is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return CircleOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return CircleOperationResponse.BadRequest($"Not found element by id: {id}");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return CircleOperationResponse.BadRequest($"Storekeeper by id: {storekeeperId} doesn't have access to data by id: {id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return CircleOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return CircleOperationResponse.InternalServerError(ex.Message);
        }
    }
}
