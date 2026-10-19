using SchoolContracts.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.DataModels;

public class LessonDataModel
{
    public string Id { get; private set; } 
    public string WorkerId { get; private set; } 
    public string LessonName { get; private set; }
    public DateTime LessonDate { get; private set; } 
    public string Description { get; private set; }
    public List<LessonInterestDataModel> Interests { get; private set; }
    public LessonDataModel() { }

    public LessonDataModel(string id, string workerId, string lessonName,DateTime lessonDate,
        string description, List<LessonInterestDataModel> lessonInterests)
    {
        Id = id;
        WorkerId = workerId;
        LessonName = lessonName;
        LessonDate = lessonDate;
        Description = description;
        Interests = lessonInterests;
    }
    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");

        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a unique identifier");

        if (WorkerId.IsEmpty())
            throw new ValidationException("Field WorkerId is empty");

        if (!WorkerId.IsGuid())
            throw new ValidationException("The value in the field WorkerId is not a unique identifier");

        if (LessonName.IsEmpty())
            throw new ValidationException("Field LessonName is empty");
    }
}
