using Simulator.Case;
using Simulator.Models;
using System.Threading.Tasks;
using Telegram.Bot;
using Simulator.TelegramBotLibrary;
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
            else if (UserCaseTableCommand.GetHealthPoints(userId) == 0)
            {
                await botClient.SendTextMessageAsync(
                userId,
                Resources.ThereIsNotAttempts,
                replyMarkup: CommandKeyboard.UserMenu);
            }
            else
            {
                UserCaseTableCommand.SetOnCourse(userId, true);
                CaseStage currentStage = StagesControl.Stages[UserCaseTableCommand.GetPoint(userId)];
                await StagesControl.Move(userId, currentStage, botClient);
            }
        }
    }
}
