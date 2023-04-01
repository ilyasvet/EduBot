using Simulator.BotControl;
using Simulator.TelegramBotLibrary;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class SkipCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            await Task.Run(() =>
            {     
                UserTableCommand.SetDialogState(userId, BotControl.State.DialogState.None);
                if (UserTableCommand.IsAdmin(userId))
                {
                    botClient.SendTextMessageAsync(
                                chatId: userId,
                                text: param,
                                replyMarkup: CommandKeyboard.AdminMenu);
                }
                else
                {
                    botClient.SendTextMessageAsync(
                                   chatId: userId,
                                   text: param,
                                   replyMarkup: CommandKeyboard.UserMenu);
                }
            });
        }
    }
}
