using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.BindingModels;

namespace SchoolContracts.AdapterContracts;

public interface IMaterialAdapter
{
    MaterialOperationResponse GetList(string storekeeperId);

    MaterialOperationResponse GetElement(string storekeeperId, string data);

    MaterialOperationResponse RegisterMaterial(string storekeeperId, MaterialBindingModel materialModel);

    MaterialOperationResponse ChangeMaterialInfo(string storekeeperId, MaterialBindingModel materialModel);

    MaterialOperationResponse RemoveMaterial(string storekeeperId, string id);
}
