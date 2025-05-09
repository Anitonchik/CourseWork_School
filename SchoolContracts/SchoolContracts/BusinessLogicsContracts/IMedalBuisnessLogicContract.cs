using SchoolContracts.DataModels;

namespace SchoolContracts.BusinessLogicsContracts;

public interface IMedalBuisnessLogicContract
{
    List<MedalDataModel> GetAllMedals(string storekeeperId);
    List<MedalDataModel> GetMedalsByRange(string storekeeperId, int range);
    MedalDataModel GetMedalById(string storekeeperId, string id);
    //void CreateConnectWithMaterial(string storekeeperId, string medalId, string materialId);
    void InsertMedal(string storekeeperId, MedalDataModel medalDataModel);
    void UpdateMedal(string storekeeperId, MedalDataModel medalDataModel);
    void DeleteMedal(string storekeeperId, string id);
}
