using Simulator.BotControl;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class AdminGoToMainMenuCommand : Command
    {
        public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            await DataBaseControl.UserTableCommand.SetDialogState(userId, BotControl.State.DialogState.None);
            await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: Resources.AdminMenu,
                        replyMarkup: CommandKeyboard.AdminMenu);
        }
    }
}
