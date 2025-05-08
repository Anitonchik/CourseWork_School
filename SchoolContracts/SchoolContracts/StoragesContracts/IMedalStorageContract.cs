using SchoolContracts.DataModels;

namespace SchoolContracts.StoragesContracts;

public interface IMedalStorageContract
{
    List<MedalDataModel> GetList(string storekeeperId, int? range);
    MedalDataModel? GetElementById(string id);
    void CreateConnectWithMaterial(string medalId, string materialId);
    void AddElement(MedalDataModel medalDataModel);
    void UpdElement(MedalDataModel medalDataModel);
    void DelElement(string id);
}
