using AutoMapper;
using SchoolContracts.AdapterContracts;
using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.BindingModels;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.ViewModels;

namespace SchoolWebApi.Adapters;

public class UserStorekeeperAdapter : IStorekeeperAdapter
{
    private readonly IStorekeeperBuisnessLogicContract _storekeeperBuisnessLogicContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public UserStorekeeperAdapter(IStorekeeperBuisnessLogicContract storekeeperBuisnessLogicContract, ILogger<CircleAdapter> logger)
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

    public StorekeeperOperationResponse ChangeStorekeeperInfo(string storekeeperId, StorekeeperBindingModel storekeeperModel)
    {
        throw new NotImplementedException();
    }

    public StorekeeperOperationResponse RegisterStorekeeper(string storekeeperId, StorekeeperBindingModel storekeeperModel)
    {
        throw new NotImplementedException();
    }

    public StorekeeperOperationResponse RemoveStorekeeper(string storekeeperId, string id)
    {
        throw new NotImplementedException();
    }
}
