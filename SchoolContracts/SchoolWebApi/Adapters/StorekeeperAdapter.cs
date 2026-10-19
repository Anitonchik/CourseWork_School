using AutoMapper;
using SchoolBusinessLogic.Implementations;
using SchoolContracts.AdapterContracts;
using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.BindingModels;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.ViewModels;
using SchoolDatabase.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolWebApi.Adapters;

public class StorekeeperAdapter : IStorekeeperAdapter
{
    private readonly IStorekeeperBuisnessLogicContract _storekeeperBuisnessLogicContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public StorekeeperAdapter(IStorekeeperBuisnessLogicContract storekeeperBuisnessLogicContract, ILogger<CircleAdapter> logger)
    {
        _storekeeperBuisnessLogicContract = storekeeperBuisnessLogicContract;
        _logger = logger;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<StorekeeperBindingModel, StorekeeperDataModel>();
            cfg.CreateMap<StorekeeperDataModel, StorekeeperViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public StorekeeperOperationResponse GetUserByLogin(string login)
    {
        try
        {
            return StorekeeperOperationResponse.OK(_mapper.Map<StorekeeperViewModel>(_storekeeperBuisnessLogicContract.GetStorekeeperByLogin(login)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return StorekeeperOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return StorekeeperOperationResponse.NotFound($"Not found element by data {login}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return StorekeeperOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return StorekeeperOperationResponse.InternalServerError(ex.Message);
        }
    }

    /*public StorekeeperOperationResponse ChangeStorekeeperInfo(StorekeeperBindingModel storekeeperModel)
    {
        throw new NotImplementedException();
    }

    public StorekeeperOperationResponse RegisterStorekeeper(StorekeeperBindingModel storekeeperModel)
    {
        try
        {
            _storekeeperBuisnessLogicContract.InsertStorekeeper(_mapper.Map<StorekeeperDataModel>(storekeeperModel));

            var registeredUserData = _storekeeperBuisnessLogicContract.GetStorekeeperByLogin(storekeeperModel.Login);
            var viewModel = _mapper.Map<StorekeeperViewModel>(registeredUserData);

            return StorekeeperOperationResponse.OK(viewModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return StorekeeperOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return StorekeeperOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return StorekeeperOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return StorekeeperOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return StorekeeperOperationResponse.InternalServerError(ex.Message);
        }
    }

    public StorekeeperOperationResponse RemoveStorekeeper(string id)
    {
        throw new NotImplementedException();
    }*/
}
