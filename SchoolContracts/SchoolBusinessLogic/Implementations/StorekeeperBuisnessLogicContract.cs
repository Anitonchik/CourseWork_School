using Microsoft.Extensions.Logging;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.Extensions;
using SchoolContracts.StoragesContracts;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SchoolBuisnessLogic.Implementations;

public class StorekeeperBuisnessLogicContract(IStorekeeperStorageContract storekeeperStorageContract, ILogger logger) : IStorekeeperBuisnessLogicContract
{
    private readonly ILogger _logger = logger;
    private readonly IStorekeeperStorageContract _storekeeperStorageContract = storekeeperStorageContract;

    public StorekeeperDataModel GetStorekeeperByLogin(string login)
    {
        _logger.LogInformation("Get element by login: {data}", login);

        if (login.IsEmpty())
        {
            throw new ValidationException("Login is not a unique identifier");
        }
        return  _storekeeperStorageContract.GetElementByLogin(login) ?? throw new ElementNotFoundException(login);
    }

    public StorekeeperDataModel GetStorekeeperByMail(string mail)
    {
        _logger.LogInformation("Get element by login: {data}", mail);

        if (mail.IsEmpty())
        {
            throw new ValidationException("Mail is not a unique identifier");
        }
        return _storekeeperStorageContract.GetElementByLogin(mail) ?? throw new ElementNotFoundException(mail);
    }

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
