using SchoolContracts.DataModels;
using SchoolDatabase.Models.ModelsForReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.StoragesContracts;

public interface ILessonStorageContract
{
    List<LessonDataModel> GetList();
    List<MaterialByLesson> GetLessonsByMaterial(string materialId);
    LessonDataModel? GetElementById(string id);
    LessonDataModel? GetElementByName(string name);
    void AddElement(LessonDataModel lessonDataModel);
    void UpdElement(LessonDataModel lessonDataModel);
    void DelElement(string id);
}
