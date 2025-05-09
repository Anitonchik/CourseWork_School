using SchoolContracts.DataModels;

namespace SchoolContracts.BusinessLogicsContracts;

public interface IMaterialBuisnessLogicContract
{
    List<MaterialDataModel> GetAllMaterials(string storekeeperId);
    MaterialDataModel GetMaterialByData(string storekeeperId, string data);
    void InsertMaterial(string storekeeperId, MaterialDataModel materialDataModel);
    void UpdateMaterial(string storekeeperId, MaterialDataModel materialDataModel);
    void DeleteMaterial(string storekeeperId, string id);
}
