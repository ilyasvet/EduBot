using Simulator.BotControl.State;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;
using Simulator.BotControl;

namespace Simulator.Commands
{
    public class LogInCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            await DataBaseControl.UserTableCommand.SetDialogState(userId, DialogState.EnterPassword);
            await botClient.SendTextMessageAsync(userId, Resources.EnterPassword);
        }
    }
}
