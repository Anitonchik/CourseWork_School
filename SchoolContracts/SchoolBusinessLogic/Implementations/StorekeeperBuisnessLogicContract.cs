using Microsoft.Extensions.Logging;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.DataModels;
using SchoolContracts.Extensions;
using SchoolContracts.StoragesContracts;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace SchoolBuisnessLogic.Implementations;

public class StorekeeperBuisnessLogicContract(IStorekeeperStorageContract storekeeperStorageContract, ILogger logger) : IStorekeeperBuisnessLogicContract
{
    private readonly ILogger _logger = logger;
    private readonly IStorekeeperStorageContract _storekeeperStorageContract = storekeeperStorageContract;

    public void InsertStorekeeper(StorekeeperDataModel storekeeperDataModel)
    {
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(storekeeperDataModel));
        ArgumentNullException.ThrowIfNull(storekeeperDataModel);
        storekeeperDataModel.Validate();
        _storekeeperStorageContract.AddElement(storekeeperDataModel);
    }
    
    public void UpdateStorekeeper(StorekeeperDataModel storekeeperDataModel)
    {
        _logger.LogInformation("Update data: {json}", JsonSerializer.Serialize(storekeeperDataModel));
        ArgumentNullException.ThrowIfNull(storekeeperDataModel);

        storekeeperDataModel.Validate();
        _storekeeperStorageContract.UpdElement(storekeeperDataModel);
    }

    public void DeleteStorekeeper(string id)
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

        _storekeeperStorageContract.DelElement(id);
    }
}
