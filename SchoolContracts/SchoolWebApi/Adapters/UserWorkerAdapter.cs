using AutoMapper;
using SchoolContracts.AdapterContracts;
using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.BindingModels;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace SchoolWebApi.Adapters;

public class UserWorkerAdapter : IWorkerAdapter
{
    private readonly IWorkerBuisnessLogicContract _workerBuisnessLogicContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public UserWorkerAdapter(IWorkerBuisnessLogicContract workerBuisnessLogicContract, ILogger<UserWorkerAdapter> logger)
    {
        _workerBuisnessLogicContract = workerBuisnessLogicContract;
        _logger = logger;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<WorkerBindingModel, WorkerDataModel>();
            cfg.CreateMap<WorkerDataModel, WorkerViewModel>();
        });
        _mapper = new Mapper(config);
    }

    public WorkerOperationResponse GetUserByLogin(string login)
    {
        try
        {
            return WorkerOperationResponse.OK(_mapper.Map<WorkerViewModel>(_workerBuisnessLogicContract.GetWorkerByLogin(login)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkerOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return WorkerOperationResponse.NotFound($"Not found element by data {login}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkerOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkerOperationResponse.InternalServerError(ex.Message);
        }
    }

    public WorkerOperationResponse ChangeWorkerInfo(WorkerBindingModel workerModel)
    {
        throw new NotImplementedException();
    }

    public WorkerOperationResponse RegisterWorker(WorkerBindingModel workerModel)
    {
        try
        {
            _workerBuisnessLogicContract.InsertWorker(_mapper.Map<WorkerDataModel>(workerModel));

            var registeredUserData = _workerBuisnessLogicContract.GetWorkerByLogin(workerModel.Login);
            var viewModel = _mapper.Map<WorkerViewModel>(registeredUserData);

            return WorkerOperationResponse.OK(viewModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkerOperationResponse.BadRequest("Data is empty");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return WorkerOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return WorkerOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkerOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkerOperationResponse.InternalServerError(ex.Message);
        }
    }

    public WorkerOperationResponse RemoveWorker(string id)
    {
        throw new NotImplementedException();
    }
}
