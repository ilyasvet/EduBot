using Simulator.BotControl.State;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;
using Simulator.TelegramBotLibrary;

namespace Simulator.Commands
{
    public class LogInCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            await Task.Run(() =>
            {
                UserTableCommand.SetDialogState(userId, DialogState.EnterPassword);
                botClient.SendTextMessageAsync(userId, Resources.EnterPassword);
            });
        }
    }
}
