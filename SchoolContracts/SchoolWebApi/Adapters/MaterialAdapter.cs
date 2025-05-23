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

public class MaterialAdapter : IMaterialAdapter
{
    private readonly IMaterialBuisnessLogicContract _materialBuisnessLogicContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public MaterialAdapter(IMaterialBuisnessLogicContract materialBuisnessLogicContract, ILogger<MaterialAdapter> logger)
    {
        _materialBuisnessLogicContract = materialBuisnessLogicContract;
        _logger = logger;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<MaterialBindingModel, MaterialDataModel>();
            cfg.CreateMap<MaterialDataModel, MaterialViewModel>();
        });
        _mapper = new Mapper(config);
    }


    public MaterialOperationResponse GetList(string storekeeperId)
    {
        try
        {
            return MaterialOperationResponse.OK([.. _materialBuisnessLogicContract.GetAllMaterials(storekeeperId).Select(x => _mapper.Map<MaterialViewModel>(x))]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return MaterialOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return MaterialOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return MaterialOperationResponse.InternalServerError(ex.Message);
        }
    }
    
    public MaterialOperationResponse GetElement(string storekeeperId, string data)
    {
        try
        {
            return MaterialOperationResponse.OK(_mapper.Map<MaterialViewModel>(_materialBuisnessLogicContract.GetMaterialByData(storekeeperId, data)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return MaterialOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return MaterialOperationResponse.NotFound($"Not found element by data {data}");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return MaterialOperationResponse.BadRequest($"Storekeeper by id: {storekeeperId} doesn't have access to data by data: {data}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return MaterialOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return MaterialOperationResponse.InternalServerError(ex.Message);
        }
    }

    public MaterialOperationResponse RegisterMaterial(string storekeeperId, MaterialBindingModel materialModel)
    {
        try
        {
            _materialBuisnessLogicContract.InsertMaterial(storekeeperId, _mapper.Map<MaterialDataModel>(materialModel));
            return MaterialOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return MaterialOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return MaterialOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return MaterialOperationResponse.BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return MaterialOperationResponse.BadRequest($"Storekeeper by id: {storekeeperId} doesn't have access to data by id: {materialModel.Id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return MaterialOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return MaterialOperationResponse.InternalServerError(ex.Message);
        }
    }
    
    public MaterialOperationResponse ChangeMaterialInfo(string storekeeperId, MaterialBindingModel materialModel)
    {
        try
        {
            _materialBuisnessLogicContract.UpdateMaterial(storekeeperId, _mapper.Map<MaterialDataModel>(materialModel));
            return MaterialOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return MaterialOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return MaterialOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return MaterialOperationResponse.BadRequest($"Not found element by Id {materialModel.Id}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return MaterialOperationResponse.BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return MaterialOperationResponse.BadRequest($"Storekeeper by id: {storekeeperId} doesn't have access to data by id: {materialModel.Id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return MaterialOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return MaterialOperationResponse.InternalServerError(ex.Message);
        }
    }

    public MaterialOperationResponse RemoveMaterial(string storekeeperId, string id)
    {
        try
        {
            _materialBuisnessLogicContract.DeleteMaterial(storekeeperId, id);
            return MaterialOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return MaterialOperationResponse.BadRequest("Id is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return MaterialOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return MaterialOperationResponse.BadRequest($"Not found element by id: {id}");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "UnauthorizedAccessException");
            return MaterialOperationResponse.BadRequest($"Storekeeper by id: {storekeeperId} doesn't have access to data by id: {id}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return MaterialOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return MaterialOperationResponse.InternalServerError(ex.Message);
        }
    }
}
