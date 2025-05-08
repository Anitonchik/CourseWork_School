using Microsoft.Extensions.Logging;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.Extensions;
using SchoolContracts.StoragesContracts;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using UnauthorizedAccessException = SchoolContracts.Exceptions.UnauthorizedAccessException;

namespace SchoolBusinessLogic.Implementations;

public class CircleBuisnessLogicContract(ICircleStorageContract circleStorageContract, ILogger logger) : ICircleBuisnessLogicContract
{
    private readonly ILogger _logger = logger;
    private readonly ICircleStorageContract _circleStorageContract = circleStorageContract;

    public List<CircleDataModel> GetAllCircles(string storekeeperId)
    {
        _logger.LogInformation("GetAllCircles");
        return _circleStorageContract.GetList(storekeeperId) ?? throw new NullListException();
    }

    public CircleDataModel GetCircleByData(string storekeeperId, string data)
    {
        _logger.LogInformation("Get element by data: {data}", data);
        CircleDataModel circle;

        if (data.IsEmpty())
        {
            throw new ArgumentNullException(nameof(data));
        }
        if (data.IsGuid())
        {
            circle = _circleStorageContract.GetElementById(data) ?? throw new ElementNotFoundException(data);
        }
        else
        {
            circle = _circleStorageContract.GetElementByName(data) ?? throw new ElementNotFoundException(data);
        }

        if (circle.StorekeeperId != storekeeperId)
        {
            throw new UnauthorizedAccessException(data);
        }
        else return circle;
    }

    public void InsertCircle(string storekeeperId, CircleDataModel circleDataModel)
    {
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(circleDataModel));
        ArgumentNullException.ThrowIfNull(circleDataModel);
        circleDataModel.Validate();
        if (circleDataModel.StorekeeperId != storekeeperId)
        {
            throw new UnauthorizedAccessException(storekeeperId);
        }
        _circleStorageContract.AddElement(circleDataModel);
    }

    public void UpdateCircle(string storekeeperId, CircleDataModel circleDataModel)
    {
        _logger.LogInformation("Update data: {json}", JsonSerializer.Serialize(circleDataModel));
        ArgumentNullException.ThrowIfNull(circleDataModel);

        if (circleDataModel.StorekeeperId != storekeeperId)
        {
            throw new UnauthorizedAccessException(circleDataModel.Id);
        }

        circleDataModel.Validate();
        _circleStorageContract.UpdElement(circleDataModel);
    }

    public void DeleteCircle(string storekeeperId, string id)
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

        var circle = _circleStorageContract.GetElementById(id);
        if (circle.StorekeeperId != storekeeperId)
        {
            throw new UnauthorizedAccessException(id);
        }

        GetCircleByData(storekeeperId, id);

        _circleStorageContract.DelElement(id);
    }
}
