using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.BusinessLogicsContracts;

public interface ILessonBuisnessLogicContract
{
    List<LessonDataModel> GetWholeLessons();
    List<LessonDataModel> GetAllLessons(string workerId);
    LessonDataModel GetLessonByData(string workerId, string data);
    //List<LessonInterestDataModel> GetInterestsByLessonId(string lessonId);
    void InsertLesson(string workerId, LessonDataModel lessonDataModel);
    void UpdateLesson(string workerId, LessonDataModel lessonDataModel);
    void DeleteLesson(string workerId, string id);
}
