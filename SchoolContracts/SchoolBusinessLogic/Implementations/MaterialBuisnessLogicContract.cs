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

public class MaterialBuisnessLogicContract(IMaterialStorageContract materialStorageContract, ILogger logger) : IMaterialBuisnessLogicContract
{
    private readonly ILogger _logger = logger;
    private readonly IMaterialStorageContract _materialStorageContract = materialStorageContract;

    public List<MaterialDataModel> GetAllMaterials(string storekeeperId)
    {
        _logger.LogInformation("GetAllMaterials");
        return _materialStorageContract.GetList(storekeeperId) ?? throw new NullListException();
    }

    public MaterialDataModel GetMaterialByData(string storekeeperId, string data)
    {
        _logger.LogInformation("Get element by data: {data}", data);
        MaterialDataModel material;

        if (data.IsEmpty())
        {
            throw new ArgumentNullException(nameof(data));
        }
        if (data.IsGuid())
        {
            material = _materialStorageContract.GetElementById(data) ?? throw new ElementNotFoundException(data);
        }
        else
        {
            material = _materialStorageContract.GetElementByName(data) ?? throw new ElementNotFoundException(data);
        }

        if (material.StorekeeperId != storekeeperId)
        {
            throw new UnauthorizedAccessException(data);
        }
        else return material;
    }

    public void InsertMaterial(string storekeeperId, MaterialDataModel materialDataModel)
    {
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(materialDataModel));
        ArgumentNullException.ThrowIfNull(materialDataModel);
        materialDataModel.Validate();
        _materialStorageContract.AddElement(materialDataModel);
    }

    public void UpdateMaterial(string storekeeperId, MaterialDataModel materialDataModel)
    {
        _logger.LogInformation("Update data: {json}", JsonSerializer.Serialize(materialDataModel));
        ArgumentNullException.ThrowIfNull(materialDataModel);

        if (materialDataModel.StorekeeperId != storekeeperId)
        {
            throw new UnauthorizedAccessException(materialDataModel.Id);
        }

        materialDataModel.Validate();
        _materialStorageContract.UpdElement(materialDataModel);
    }
    
    public void DeleteMaterial(string storekeeperId, string id)
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

        var material = _materialStorageContract.GetElementById(id) ?? throw new ElementNotFoundException(id);
        if (material.StorekeeperId != storekeeperId)
        {
            throw new UnauthorizedAccessException(id);
        }

        _materialStorageContract.DelElement(id);
    }
}
