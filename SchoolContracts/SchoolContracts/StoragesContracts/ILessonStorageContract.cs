using SchoolContracts.DataModels;
using SchoolContracts.ModelsForReports;
using SchoolDatabase.Models.ModelsForReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.StoragesContracts;

public interface ILessonStorageContract
{
    List<LessonDataModel> GetList(string workerId);
    Task<List<LessonByMaterialModel>> GetLessonsByMaterial(string storekeeperId, string materialId, CancellationToken ct);
    LessonDataModel? GetElementById(string id);
    LessonDataModel? GetElementByName(string name);
    void AddElement(LessonDataModel lessonDataModel);
    void UpdElement(LessonDataModel lessonDataModel);
    void DelElement(string id);
}
