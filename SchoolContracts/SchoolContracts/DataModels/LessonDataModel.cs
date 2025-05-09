using SchoolContracts.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.DataModels;

public class LessonDataModel(string id, string workerId,  string lessonName, string description, List<LessonInterestDataModel> LessonInterests)
{
    public string Id { get; private set; } = id;
    public string WorkerId { get; private set; } = workerId;
    public string LessonName { get; private set; } = lessonName;
    public DateTime LessonDate { get; private set; } = DateTime.UtcNow;
    public string Description { get; private set; } = description;
    public List<LessonInterestDataModel> Interests { get; private set; } = LessonInterests;
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
