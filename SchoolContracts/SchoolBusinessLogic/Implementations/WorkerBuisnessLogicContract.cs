using Microsoft.Extensions.Logging;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.Extensions;
using SchoolContracts.StoragesContracts;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;


namespace SchoolBuisnessLogic.Implementations;

public class WorkerBuisnessLogicContract(IWorkerStorageContract workerStorageContract, ILogger logger) : IWorkerBuisnessLogicContract
{
    private readonly ILogger _logger = logger;
    private readonly IWorkerStorageContract _workerStorageContract = workerStorageContract;

    public WorkerDataModel GetWorkerByLogin(string login)
    {
        _logger.LogInformation("Get element by login: {data}", login);

        if (login.IsEmpty())
        {
            throw new ValidationException("Login is not a unique identifier");
        }
        return _workerStorageContract.GetElementByLogin(login) ?? throw new ElementNotFoundException(login);
    }

    public void InsertWorker(WorkerDataModel workerDataModel)
    {
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(workerDataModel));
        ArgumentNullException.ThrowIfNull(workerDataModel);
        workerDataModel.Validate();
        _workerStorageContract.AddElement(workerDataModel);
    }

    public void UpdateWorker(WorkerDataModel workerDataModel)
    {
        _logger.LogInformation("Update data: {json}", JsonSerializer.Serialize(workerDataModel));
        ArgumentNullException.ThrowIfNull(workerDataModel);

        workerDataModel.Validate();
        _workerStorageContract.UpdElement(workerDataModel);
    }

    public void DeleteWorker(string id)
    {
        _logger.LogInformation("Delete by id: {id}", id);
        if (id.IsEmpty())
        {
            throw new ArgumentNullException(nameof(id));
        }
        if (!id.IsGuid())
        {
            throw new ValidationException("Id is not a unique identifier");
        }

        _workerStorageContract.DelElement(id);
    }
}
