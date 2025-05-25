using Microsoft.Extensions.Logging;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.Extensions;
using SchoolContracts.StoragesContracts;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using UnauthorizedAccessException = SchoolContracts.Exceptions.UnauthorizedAccessException;

namespace SchoolBuisnessLogic.Implementations;

public class LessonCircleBuisnessLogicContract(ILessonCircleStorageContract lessonCircleStorageContract, ICircleBuisnessLogicContract circleBuisnessLogicContract, 
    ILogger logger) : ILessonCircleBuisnessLogicContract
{
    private readonly ILogger _logger = logger;
    private readonly ICircleBuisnessLogicContract _circleBuisnessLogicContract = circleBuisnessLogicContract;
    private readonly ILessonCircleStorageContract _lessonCircleStorageContract = lessonCircleStorageContract;

    public void CreateLessonCircle(string storekeeperId, string lessonId, string circleId, int count)
    {
        if (lessonId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(lessonId));
        }
        if (!lessonId.IsGuid())
        {
            throw new ValidationException("lessonId is not a unique identifier");
        }
        if (circleId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(circleId));
        }
        if (!circleId.IsGuid())
        {
            throw new ValidationException("circleId is not a unique identifier");
        }
        if (count < 0)
        {
            throw new ValidationException("count is less 0");
        }

        var lessonCircle = new LessonCircleDataModel(lessonId, circleId, count);

        /*var circle = _circleBuisnessLogicContract.GetCircleByData(storekeeperId, circleId);
        circle.Lessons.Add(lessonCircle);

        _circleBuisnessLogicContract.UpdateCircle(storekeeperId, circle);*/

        _lessonCircleStorageContract.AddElement(lessonCircle);

    }

    public void DeleteLessonCircle(string storekeeperId, string lessonId, string circleId)
    {
        _logger.LogInformation($"Delete by id: {lessonId}, {circleId}");
        if (lessonId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(lessonId));
        }
        if (!lessonId.IsGuid())
        {
            throw new ValidationException("lessonId is not a unique identifier");
        }
        if (circleId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(circleId));
        }
        if (!circleId.IsGuid())
        {
            throw new ValidationException("circleId is not a unique identifier");
        }

        try
        {
            var circle = _circleBuisnessLogicContract.GetCircleByData(storekeeperId, circleId);
            var lessonCircle = _lessonCircleStorageContract.GetLessonCircleById(lessonId, circleId);
            circle.Lessons.Remove(lessonCircle);
            _circleBuisnessLogicContract.UpdateCircle(storekeeperId , circle);
        }
        catch (ElementNotFoundException)
        {
            throw;
        }
        catch (UnauthorizedAccessException)
        {
            throw;
        }
    }
}
