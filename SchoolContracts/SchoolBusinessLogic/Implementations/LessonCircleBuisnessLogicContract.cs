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

public class LessonCircleBuisnessLogicContract(ILessonCircleStorageContract lessonCircleStorageContract, 
    ICircleBuisnessLogicContract circleBuisnessLogicContract, ILessonStorageContract lessonStorageContract,
    ILogger logger) : ILessonCircleBuisnessLogicContract
{
    private readonly ILogger _logger = logger;
    private readonly ILessonCircleStorageContract _lessonCircleStorageContract = lessonCircleStorageContract;
    private readonly ICircleBuisnessLogicContract _circleBuisnessLogicContract = circleBuisnessLogicContract;
    private readonly ILessonStorageContract _lessonStorageContract = lessonStorageContract;

    public void CreateLessonCircle(string storekeeperId, LessonCircleDataModel lessonCircleDataModel)
    {
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(lessonCircleDataModel));
        ArgumentNullException.ThrowIfNull(lessonCircleDataModel);
        lessonCircleDataModel.Validate();
        try
        {
            circleBuisnessLogicContract.GetCircleByData(storekeeperId, lessonCircleDataModel.CircleId);
        }
        catch (UnauthorizedAccessException)
        {
            throw;
        }
        _lessonCircleStorageContract.AddElement(lessonCircleDataModel);
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
            _circleBuisnessLogicContract.GetCircleByData(storekeeperId, circleId);
            _lessonStorageContract.GetElementById(lessonId);

            _lessonCircleStorageContract.DeleteElement(lessonId, circleId);
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
