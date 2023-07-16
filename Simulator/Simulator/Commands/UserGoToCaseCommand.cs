using Simulator.Case;
using Simulator.Models;
using System.Threading.Tasks;
using Telegram.Bot;
using Simulator.Properties;
using Simulator.BotControl;

namespace Simulator.Commands
{
    internal class UserGoToCaseCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            if (StagesControl.Stages == null)
            {
                await botClient.SendTextMessageAsync(
                userId,
                Resources.ThereIsNotCase,
                replyMarkup: CommandKeyboard.UserMenu);

            }
            else if (await DataBaseControl.UserCaseTableCommand.GetHealthPoints(userId) == 0
                && await DataBaseControl.UserCaseTableCommand.GetPoint(userId) == StagesControl.Stages.StagesEnd.
                    Find(s => s.IsEndOfCase == true).Number)
            {
                await botClient.SendTextMessageAsync(
                userId,
                Resources.ThereIsNotAttempts,
                replyMarkup: CommandKeyboard.UserMenu);
            }
            else
            {
                await DataBaseControl.UserCaseTableCommand.SetOnCourse(userId, true);
                CaseStage currentStage = StagesControl.Stages[await DataBaseControl.UserCaseTableCommand.GetPoint(userId)];
                await StagesControl.Move(userId, currentStage, botClient);
            }
        }
    }
}
