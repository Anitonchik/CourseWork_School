using Microsoft.Extensions.Logging;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.Extensions;
using SchoolContracts.StoragesContracts;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using UnauthorizedAccessException = SchoolContracts.Exceptions.UnauthorizedAccessException;

namespace SchoolBuisnessLogic.Implementations;

public class InterestBuisnessLogicContract(IInterestStorageContract interestStorageContract, ILogger logger) : IInterestBuisnessLogicContract
{
    private readonly ILogger _logger = logger;
    private readonly IInterestStorageContract _interestStorageContract = interestStorageContract;
    public List<InterestDataModel> GetAllInterests(string workerId)
    {
        _logger.LogInformation("GetAllInterests");
        return _interestStorageContract.GetList(workerId) ?? throw new NullListException();
    }

    public InterestDataModel GetInterestByData(string workerId, string data)
    {
        _logger.LogInformation("Get element by data: {data}", data);
        InterestDataModel interest;

        if (data.IsEmpty())
        {
            throw new ArgumentNullException(nameof(data));
        }
        if (data.IsGuid())
        {
            interest = _interestStorageContract.GetElementById(data) ?? throw new ElementNotFoundException(data);
        }
        else
        {
            interest = _interestStorageContract.GetElementByName(data) ?? throw new ElementNotFoundException(data);
        }

        if (interest.WorkerId != workerId)
        {
            throw new UnauthorizedAccessException(data);
        }
        else return interest;
    }

    public void InsertInterest(string workerId, InterestDataModel interestDataModel)
    {
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(interestDataModel));
        ArgumentNullException.ThrowIfNull(interestDataModel);
        interestDataModel.Validate();
        if (interestDataModel.WorkerId != workerId)
        {
            throw new UnauthorizedAccessException(workerId);
        }
        _interestStorageContract.AddElement(interestDataModel);
    }

    public void UpdateInterest(string workerId, InterestDataModel interestDataModel)
    {
        _logger.LogInformation("Update data: {json}", JsonSerializer.Serialize(interestDataModel));
        ArgumentNullException.ThrowIfNull(interestDataModel);

        if (interestDataModel.WorkerId != workerId)
        {
            throw new UnauthorizedAccessException(interestDataModel.Id);
        }

        interestDataModel.Validate();
        _interestStorageContract.UpdElement(interestDataModel);
    }

    public void DeleteInterest(string workerId, string id)
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

        var interest = _interestStorageContract.GetElementById(id);
        if (interest.WorkerId != workerId)
        {
            throw new UnauthorizedAccessException(id);
        }

        GetInterestByData(workerId, id);

        _interestStorageContract.DelElement(id);
    }
}
