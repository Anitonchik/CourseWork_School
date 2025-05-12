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

public class MedalAdapter : IMedalAdapter
{
    private readonly IMedalBuisnessLogicContract _medalBuisnessLogicContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public MedalAdapter(IMedalBuisnessLogicContract MedalBuisnessLogicContract, ILogger<MedalAdapter> logger)
    {
        _medalBuisnessLogicContract = MedalBuisnessLogicContract;
        _logger = logger;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<MedalBindingModel, MedalDataModel>();
            cfg.CreateMap<MedalDataModel, MedalViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public MedalOperationResponse GetList(string storekeeperId)
    {
        try
        {
            return MedalOperationResponse.OK([.. _medalBuisnessLogicContract.GetAllMedals(storekeeperId).Select(x => _mapper.Map<MedalViewModel>(x))]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return MedalOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return MedalOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return MedalOperationResponse.InternalServerError(ex.Message);
        }
    }

    public MedalOperationResponse GetListByRange(string storekeeperId, int range)
    {
        try
        {
            return MedalOperationResponse.OK([.. _medalBuisnessLogicContract.GetMedalsByRange(storekeeperId, range).Select(x => _mapper.Map<MedalViewModel>(x))]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return MedalOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return MedalOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return MedalOperationResponse.InternalServerError(ex.Message);
        }
    }

    public MedalOperationResponse GetElement(string storekeeperId, string id)
    {
        try
        {
            return MedalOperationResponse.OK(_mapper.Map<MedalViewModel>(_medalBuisnessLogicContract.GetMedalById(storekeeperId, id)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return MedalOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return MedalOperationResponse.NotFound($"Not found element by data {id}");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return MedalOperationResponse.BadRequest($"Storekeeper by id: {storekeeperId} doesn't have access to data by data: {id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return MedalOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return MedalOperationResponse.InternalServerError(ex.Message);
        }
    }

    public MedalOperationResponse RegisterMedal(string storekeeperId, MedalBindingModel medalModel)
    {
        try
        {
            _medalBuisnessLogicContract.InsertMedal(storekeeperId, _mapper.Map<MedalDataModel>(medalModel));
            return MedalOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return MedalOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return MedalOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return MedalOperationResponse.BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return MedalOperationResponse.BadRequest($"Storekeeper by id: {storekeeperId} doesn't have access to data by id: {medalModel.Id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return MedalOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return MedalOperationResponse.InternalServerError(ex.Message);
        }
    }

    public MedalOperationResponse ChangeMedalInfo(string storekeeperId, MedalBindingModel medalModel)
    {
        try
        {
            _medalBuisnessLogicContract.InsertMedal(storekeeperId, _mapper.Map<MedalDataModel>(medalModel));
            return MedalOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return MedalOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return MedalOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return MedalOperationResponse.BadRequest($"Not found element by Id {medalModel.Id}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return MedalOperationResponse.BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return MedalOperationResponse.BadRequest($"Storekeeper by id: {storekeeperId} doesn't have access to data by id: {medalModel.Id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return MedalOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return MedalOperationResponse.InternalServerError(ex.Message);
        }
    }

    public MedalOperationResponse RemoveMedal(string storekeeperId, string id)
    {
        try
        {
            _medalBuisnessLogicContract.DeleteMedal(storekeeperId, id);
            return MedalOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return MedalOperationResponse.BadRequest("Id is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return MedalOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return MedalOperationResponse.BadRequest($"Not found element by id: {id}");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return MedalOperationResponse.BadRequest($"Storekeeper by id: {storekeeperId} doesn't have access to data by id: {id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return MedalOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return MedalOperationResponse.InternalServerError(ex.Message);
        }
    }
}
