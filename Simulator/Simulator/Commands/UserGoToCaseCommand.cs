using Simulator.Case;
using Simulator.Models;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    internal class UserGoToCaseCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            UserCaseTableCommand.SetOnCourse(userId, true);
            CaseStage currentStage = StagesControl.Stages[UserCaseTableCommand.GetPoint(userId)];
            await StagesControl.Move(userId, currentStage, botClient);
        }
    }
}
