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

public class AchievementBuisnessLogicContract(IAchievementStorageContract achievementStorageContract, ILessonBuisnessLogicContract lessonBuisnessLogucContract,
    ILogger logger):IAchievementBuisnessLogicContract
{
    private readonly ILogger _logger = logger;
    private readonly IAchievementStorageContract _achievementStorageContract = achievementStorageContract;
    private readonly ILessonBuisnessLogicContract _lessonBuisnessLogucContract = lessonBuisnessLogucContract;

    public List<AchievementDataModel> GetAllAchievements(string workerId)
    {
        _logger.LogInformation("GetAllAchievements");
        return _achievementStorageContract.GetList(workerId) ?? throw new NullListException();
    }
    public AchievementDataModel GetAchievementById(string workerId, string data)
    {
        _logger.LogInformation("Get element by data: {data}", data);
        AchievementDataModel achievement;

        if (data.IsEmpty())
        {
            throw new ArgumentNullException(nameof(data));
        }
        if (!data.IsGuid())
        {
            throw new ValidationException("Id is not a unique identifier");
        }
        else
        {
            achievement = _achievementStorageContract.GetElementById(data) ?? throw new ElementNotFoundException(data);
        }
        if (achievement.WorkerId != workerId)
        {
            throw new UnauthorizedAccessException(data);
        }
        else return achievement;
    }

    public void CreateConnectWithLesson(string workerId, string achievementId, string lessonId)
    {
        _logger.LogInformation("Create connection between medal and lesson");
        if (!lessonId.IsGuid() || !lessonId.IsGuid())
        {
            throw new ValidationException("Id is not a unique identifier");
        }

        _lessonBuisnessLogucContract.GetLessonByData(workerId, lessonId);
        GetAchievementById(workerId, lessonId);

        _achievementStorageContract.CreateConnectWithLesson(achievementId, lessonId);
    }

    public void InsertAchievement(string цщклукId, AchievementDataModel achievementDataModel)
    {
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(achievementDataModel));
        ArgumentNullException.ThrowIfNull(achievementDataModel);
        achievementDataModel.Validate();
        _achievementStorageContract.AddElement(achievementDataModel);
    }

    public void UpdateAchievement(string workerId, AchievementDataModel achievementDataModel)
    {
        _logger.LogInformation("Update data: {json}", JsonSerializer.Serialize(achievementDataModel));
        ArgumentNullException.ThrowIfNull(achievementDataModel);

        if (achievementDataModel.WorkerId != workerId)
        {
            throw new UnauthorizedAccessException(achievementDataModel.Id);
        }

        achievementDataModel.Validate();
        _achievementStorageContract.UpdElement(achievementDataModel);
    }

    public void DeleteAchievement(string workerId, string id)
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

        var achievement = _achievementStorageContract.GetElementById(id) ?? throw new ElementNotFoundException(id);
        if (achievement.WorkerId != workerId)
        {
            throw new UnauthorizedAccessException(id);
        }

        _achievementStorageContract.DelElement(id);
    }
}
