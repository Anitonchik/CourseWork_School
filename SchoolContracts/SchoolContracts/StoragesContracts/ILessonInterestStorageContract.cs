using SchoolContracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.StoragesContracts;

public interface ILessonInterestStorageContract
{
    public LessonInterestDataModel? GetLessonInterestById(string lessonId, string interestId);
}
