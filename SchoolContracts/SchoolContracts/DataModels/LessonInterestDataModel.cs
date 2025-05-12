using SchoolContracts.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.DataModels;

public class LessonInterestDataModel(string lessonId, string interesId,string category)
{
    public string LessonId { get; private set; } = lessonId;
    public string InterestId { get; private set; } = interesId;
    public string Category { get; private set; } = category;
    public void Validate()
    {
        if (LessonId.IsEmpty())
            throw new ValidationException("Field LessonId is empty");
        if (!LessonId.IsGuid())
            throw new ValidationException("The value in the field OrderId is not a unique identifier");
        if (InterestId.IsEmpty())
            throw new ValidationException("Field InterestId is empty");
        if (!InterestId.IsGuid())
            throw new ValidationException("The value in the field Id is not a unique identifier");
    }
}
