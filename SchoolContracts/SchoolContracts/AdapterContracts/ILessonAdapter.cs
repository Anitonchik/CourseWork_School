using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.AdapterContracts;

public interface ILessonAdapter
{
    LessonOperationResponse GetList(string workerId);

    LessonOperationResponse GetElement(string workerId, string data);

    LessonOperationResponse RegisterLesson(string workerId, LessonBindingModel lessonModel);

    LessonOperationResponse ChangeLessonInfo(string workerId, LessonBindingModel lessonModel);

    LessonOperationResponse RemoveLesson(string workerId, string id);
}
