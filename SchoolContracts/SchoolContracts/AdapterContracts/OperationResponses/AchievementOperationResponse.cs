using SchoolContracts.Infrastructure;
using SchoolContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolContracts.AdapterContracts.OperationResponses;

public class AchievementOperationResponse : OperationResponse
{
    public static AchievementOperationResponse OK(List<AchievementViewModel> data) => OK<AchievementOperationResponse, List<AchievementViewModel>>(data);

    public static AchievementOperationResponse OK(AchievementViewModel data) => OK<AchievementOperationResponse, AchievementViewModel>(data);

    public static AchievementOperationResponse NoContent() => NoContent<AchievementOperationResponse>();

    public static AchievementOperationResponse BadRequest(string message) => BadRequest<AchievementOperationResponse>(message);

    public static AchievementOperationResponse NotFound(string message) => NotFound<AchievementOperationResponse>(message);

    public static AchievementOperationResponse InternalServerError(string message) => InternalServerError<AchievementOperationResponse>(message);
}
