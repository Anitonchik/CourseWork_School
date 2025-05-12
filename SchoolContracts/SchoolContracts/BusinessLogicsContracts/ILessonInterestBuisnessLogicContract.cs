using SchoolContracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.BusinessLogicsContracts;

public interface ILessonInterestBuisnessLogicContract
{
    public void CreateLessonInterest(string workerId, LessonDataModel lessonDataModel, LessonInterestDataModel lessonInterestDataModel);
    public void DeleteLessonInterest(string workerId, string lessonId, string interestId);
}
