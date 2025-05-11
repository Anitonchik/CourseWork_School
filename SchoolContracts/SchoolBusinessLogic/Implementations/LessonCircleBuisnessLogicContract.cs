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

    public void CreateLessonCircle(string storekeeperId, CircleDataModel circleDataModel,  LessonCircleDataModel lessonCircleDataModel)
    {
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(circleDataModel));
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(lessonCircleDataModel));
        ArgumentNullException.ThrowIfNull(circleDataModel);
        ArgumentNullException.ThrowIfNull(lessonCircleDataModel);
        circleDataModel.Validate();
        lessonCircleDataModel.Validate();
        if (circleDataModel.Id != lessonCircleDataModel.CircleId)
        {
            throw new ArgumentException("Id are not equals");
        }
        if (circleDataModel.StorekeeperId != storekeeperId)
        {
            throw new UnauthorizedAccessException(storekeeperId);
        }

        circleDataModel.Lessons.Add(lessonCircleDataModel);
        _circleBuisnessLogicContract.UpdateCircle(storekeeperId, circleDataModel);


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
