using Microsoft.Extensions.Logging;
using SchoolContracts.BusinessLogicsContracts;
using SchoolContracts.DataModels;
using SchoolContracts.Exceptions;
using SchoolContracts.Extensions;
using SchoolContracts.StoragesContracts;
using SchoolDatabase.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using UnauthorizedAccessException = SchoolContracts.Exceptions.UnauthorizedAccessException;

namespace SchoolBuisnessLogic.Implementations;

public class LessonBuisnessLogicContract(ILessonStorageContract lessonStorageContract, ILogger logger) : ILessonBuisnessLogicContract
{
    private readonly ILogger _logger = logger;
    private readonly ILessonStorageContract _lessonStorageContract = lessonStorageContract;

    public List<LessonDataModel> GetWholeLessons()
    {
        _logger.LogInformation("GetAllLessons");
        return _lessonStorageContract.GetWholeList() ?? throw new NullListException();
    }

    public List<LessonDataModel> GetAllLessons(string workerId)
    {
        _logger.LogInformation("GetAllLessons");
        return _lessonStorageContract.GetList(workerId) ?? throw new NullListException();
    }

    public LessonDataModel GetLessonByData(string workerId, string data)
    {
        _logger.LogInformation("Get element by data: {data}", data);
        LessonDataModel lesson;

        if (data.IsEmpty())
        {
            throw new ArgumentNullException(nameof(data));
        }
        if (data.IsGuid())
        {
            lesson = _lessonStorageContract.GetElementById(data) ?? throw new ElementNotFoundException(data);
        }
        else
        {
            lesson = _lessonStorageContract.GetElementByName(data) ?? throw new ElementNotFoundException(data);
        }

        if (lesson.WorkerId != workerId)
        {
            throw new UnauthorizedAccessException(data);
        }
        else return lesson;
    }

    public void InsertLesson(string workerId, LessonDataModel lessonDataModel)
    {
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(lessonDataModel));
        ArgumentNullException.ThrowIfNull(lessonDataModel);
        lessonDataModel.Validate();
        if (lessonDataModel.WorkerId != workerId)
        {
            throw new UnauthorizedAccessException(workerId);
        }
        _lessonStorageContract.AddElement(lessonDataModel);
    }

    public void UpdateLesson(string workerId, LessonDataModel lessonataModel)
    {
        _logger.LogInformation("Update data: {json}", JsonSerializer.Serialize(lessonataModel));
        ArgumentNullException.ThrowIfNull(lessonataModel);

        if (lessonataModel.WorkerId != workerId)
        {
            throw new UnauthorizedAccessException(lessonataModel.Id);
        }

        lessonataModel.Validate();
        _lessonStorageContract.UpdElement(lessonataModel);
    }

    public void DeleteLesson(string workerId, string id)
    {
        _logger.LogInformation("Delete by id: {id}", id);
        if (id.IsEmpty())
        {
            throw new ArgumentNullException(nameof(id));
        }
        if (!id.IsGuid())
        {
            throw new ValidationException("Id is not a unique identifier");
        }

        var lesson = _lessonStorageContract.GetElementById(id);
        if (lesson.WorkerId != workerId)
        {
            throw new UnauthorizedAccessException(id);
        }

        GetLessonByData(workerId, id);

        _lessonStorageContract.DelElement(id);
    }
}
