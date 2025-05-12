using Microsoft.Extensions.Logging;
using SchoolBusinessLogic.Implementations;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.Extensions;
using SchoolContracts.StoragesContracts;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using UnauthorizedAccessException = SchoolContracts.Exceptions.UnauthorizedAccessException;

namespace SchoolBuisnessLogic.Implementations;

public class LessonInterestBuisnessLogicContract(ILessonInterestStorageContract lessonInterestStorageContract, ILessonBuisnessLogicContract lessonBuisnessLogicContract,
    ILogger logger) : ILessonInterestBuisnessLogicContract
{
    private readonly ILogger _logger = logger;
    private readonly ILessonBuisnessLogicContract _lessonBuisnessLogicContract = lessonBuisnessLogicContract;
    private readonly ILessonInterestStorageContract _lessonInterestStorageContract = lessonInterestStorageContract;

    public void CreateLessonInterest(string workerId, LessonDataModel lessonDataModel, LessonInterestDataModel lessonInterestDataModel)
    {
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(lessonDataModel));
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(lessonInterestDataModel));
        ArgumentNullException.ThrowIfNull(lessonDataModel);
        ArgumentNullException.ThrowIfNull(lessonInterestDataModel);
        lessonDataModel.Validate();
        lessonInterestDataModel.Validate();
        if (lessonDataModel.Id != lessonInterestDataModel.LessonId)
        {
            throw new ArgumentException("Id are not equals");
        }
        if (lessonDataModel.WorkerId != workerId)
        {
            throw new UnauthorizedAccessException(workerId);
        }

        lessonDataModel.Interests.Add(lessonInterestDataModel);
        _lessonBuisnessLogicContract.UpdateLesson(workerId, lessonDataModel);


    }

    public void DeleteLessonInterest(string workerId, string lessonId, string interestId)
    {
        _logger.LogInformation($"Delete by id: {lessonId}, {interestId}");
        if (lessonId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(lessonId));
        }
        if (!lessonId.IsGuid())
        {
            throw new ValidationException("lessonId is not a unique identifier");
        }
        if (interestId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(interestId));
        }
        if (!interestId.IsGuid())
        {
            throw new ValidationException("interestId is not a unique identifier");
        }

        try
        {
            var lesson = _lessonBuisnessLogicContract.GetLessonByData(workerId, lessonId);
            var lessonInterest = _lessonInterestStorageContract.GetLessonInterestById(lessonId, interestId);
            lesson.Interests.Remove(lessonInterest);
            _lessonBuisnessLogicContract.UpdateLesson(workerId, lesson);
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
