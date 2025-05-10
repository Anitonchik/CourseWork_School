using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.BindingModels;

namespace SchoolContracts.AdapterContracts;

public interface IMaterialAdapter
{
    MaterialOperationResponse GetList();

    MaterialOperationResponse GetElement(string data);

    MaterialOperationResponse RegisterMaterial(MaterialBindingModel materialModel);

    MaterialOperationResponse ChangeMaterialInfo(MaterialBindingModel materialModel);

    MaterialOperationResponse RemoveMaterial(string id);
}
