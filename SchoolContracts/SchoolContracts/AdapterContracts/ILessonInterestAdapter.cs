using SchoolContracts.AdapterContracts.OperationResponses;
using SchoolContracts.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.AdapterContracts;

public interface ILessonInterestAdapter
{
    LessonInterestOperationResponse RegisterLessonInterest(string workerId, LessonBindingModel lessonModel, LessonInterestBindingModel LessonInterestModel);

    LessonInterestOperationResponse RemoveLessonInterest(string workerId, string lessonId, string interestId);
}
