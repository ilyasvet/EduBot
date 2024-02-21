using Simulator.Case;
using Simulator.Models;
using Telegram.Bot;
using EduBotCore.Properties;
using Simulator.BotControl;
using EduBotCore.Case;
using EduBotCore.Models.DbModels;

namespace Simulator.Commands
{
    internal class UserGoToCaseCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            //param - courseName-groupNumber
            var parameters = param.Split('-');
            string courseName = parameters[0];
            if (!CoursesControl.Courses.Contains(courseName))
            {
                await botClient.SendTextMessageAsync(
                userId,
                Resources.ThereIsNotCase,
                replyMarkup: CommandKeyboard.UserMenu);
            }
            else if (await DataBaseControl.StatsStateTableCommand.GetPoint(courseName ,userId) == -1)
            {
                await botClient.SendTextMessageAsync(
                userId,
                Resources.ThereIsNotAttempts,
                replyMarkup: CommandKeyboard.UserMenu);
            }
            else
            {
                UserFlags currentUserFlags = await DataBaseControl.GetEntity<UserFlags>(userId);
                currentUserFlags.CurrentCourse = courseName;
                await DataBaseControl.UpdateEntity(userId, currentUserFlags);
                CaseStage currentStage = CoursesControl.Courses[courseName][
                    await DataBaseControl.StatsStateTableCommand.GetPoint(courseName, userId)
                    ];
                await StagesControl.Move(userId, currentStage, botClient);
            }
        }
    }
}
