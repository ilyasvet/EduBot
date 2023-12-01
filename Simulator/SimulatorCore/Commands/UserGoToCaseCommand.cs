using Simulator.Case;
using Simulator.Models;
using Telegram.Bot;
using Simulator.Properties;
using Simulator.BotControl;

namespace Simulator.Commands
{
    internal class UserGoToCaseCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            string courseName = StagesControl.Stages.CourseName;
            if (StagesControl.Stages == null)
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
                await DataBaseControl.UserFlagsTableCommand.SetOnCourse(userId, true);
                CaseStage currentStage = StagesControl.Stages[
                    await DataBaseControl.StatsStateTableCommand.GetPoint(courseName, userId)
                    ];
                await StagesControl.Move(userId, currentStage, botClient);
            }
        }
    }
}
