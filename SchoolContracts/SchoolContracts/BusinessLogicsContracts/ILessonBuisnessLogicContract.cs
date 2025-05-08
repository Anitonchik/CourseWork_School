using SchoolContracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.BusinessLogicsContracts;

public interface ILessonBuisnessLogicContract
{
    List<LessonDataModel> GetAllLessons();
    List<LessonDataModel> GetAllLessonsByWorker(string workerId);
    LessonDataModel GetLessonByData(string data);
    List<LessonInterestDataModel> GetInterestsByLessonId(string lessonId);
    void InsertLesson(LessonDataModel lessonDataModel);
    void UpdateLesson(LessonDataModel lessonDataModel);
    void DeleteLesson(string id);
}
