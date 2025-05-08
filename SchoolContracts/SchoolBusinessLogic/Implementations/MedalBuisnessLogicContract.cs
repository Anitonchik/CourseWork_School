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

internal class MedalBuisnessLogicContract(IMedalStorageContract medalStorageContract, IMaterialBuisnessLogicContract materialBuisnessLogucContract, 
    ILogger logger) : IMedalBuisnessLogicContract
{
    private readonly ILogger _logger = logger;
    private readonly IMedalStorageContract _medalStorageContract = medalStorageContract;
    private readonly IMaterialBuisnessLogicContract _materialBuisnessLogucContract = materialBuisnessLogucContract;

    public List<MedalDataModel> GetAllMedals(string storekeeperId)
    {
        _logger.LogInformation("GetAllMaterials");
        return _medalStorageContract.GetList(storekeeperId, null) ?? throw new NullListException();
    }

    public List<MedalDataModel> GetMedalsByRange(string storekeeperId, int range)
    {
        _logger.LogInformation("GetMaterialsByRange");
        return _medalStorageContract.GetList(storekeeperId, range) ?? throw new NullListException();
    }

    public MedalDataModel GetMedalById(string storekeeperId, string data)
    {
        _logger.LogInformation("Get element by data: {data}", data);
        MedalDataModel medal;

        if (data.IsEmpty())
        {
            throw new ArgumentNullException(nameof(data));
        }
        if (!data.IsGuid())
        {
            throw new ValidationException("Id is not a unique identifier");
        }

        medal = _medalStorageContract.GetElementById(data) ?? throw new ElementNotFoundException(data);

        if (medal.StorekeeperId != storekeeperId)
        {
            throw new UnauthorizedAccessException(data);
        }
        else return medal;
    }

    public void CreateConnectWithMaterial(string storekeeperId, string medalId, string materialId)
    {
        _logger.LogInformation("Create connection between medal and material");
        if (!medalId.IsGuid() || !materialId.IsGuid())
        {
            throw new ValidationException("Id is not a unique identifier");
        }

        _materialBuisnessLogucContract.GetMaterialByData(storekeeperId, materialId);
        GetMedalById(storekeeperId, medalId);

        _medalStorageContract.CreateConnectWithMaterial(medalId, materialId);
    }

    public void InsertMedal(string storekeeperId, MedalDataModel medalDataModel)
    {
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(medalDataModel));
        ArgumentNullException.ThrowIfNull(medalDataModel);
        medalDataModel.Validate();
        _medalStorageContract.AddElement(medalDataModel);
    }

    public void UpdateMedal(string storekeeperId, MedalDataModel medalDataModel)
    {
        _logger.LogInformation("Update data: {json}", JsonSerializer.Serialize(medalDataModel));
        ArgumentNullException.ThrowIfNull(medalDataModel);

        if (medalDataModel.StorekeeperId != storekeeperId)
        {
            throw new UnauthorizedAccessException(medalDataModel.Id);
        }

        medalDataModel.Validate();
        _medalStorageContract.UpdElement(medalDataModel);
    }

    public void DeleteMedal(string storekeeperId, string id)
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

        var material = _medalStorageContract.GetElementById(id) ?? throw new ElementNotFoundException(id);
        if (material.StorekeeperId != storekeeperId)
        {
            throw new UnauthorizedAccessException(id);
        }

        _medalStorageContract.DelElement(id);
    }
}
