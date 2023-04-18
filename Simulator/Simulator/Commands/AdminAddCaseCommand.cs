using Simulator.BotControl.State;
using Simulator.Properties;
using Simulator.TelegramBotLibrary;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    internal class AdminAddCaseCommand : Command
    {
        public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            await Task.Run(() =>
            {
                UserTableCommand.SetDialogState(userId, DialogState.AddingCase);
                botClient.SendTextMessageAsync(userId, Resources.AddCase);
            });
        }
    }
}
