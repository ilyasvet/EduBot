using Simulator.BotControl;
using Simulator.BotControl.State;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    internal class AdminAddCaseCommand : Command
    {
        public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            await DataBaseControl.UserTableCommand.SetDialogState(userId, DialogState.AddingCase);
            await botClient.SendTextMessageAsync(
                  chatId: userId,
                  text: Resources.AddCase,
                  replyMarkup: CommandKeyboard.ToMainMenuAdmin);
        }
    }
}
