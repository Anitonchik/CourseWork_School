using SchoolContracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.StoragesContracts;

public interface ILessonStorageContract
{
    List<LessonDataModel> GetList();
    LessonDataModel? GetElementById(string id);
    LessonDataModel? GetElementByName(string name);
    void AddElement(LessonDataModel lessonDataModel);
    void UpdElement(LessonDataModel lessonDataModel);
    void DelElement(string id);
}
