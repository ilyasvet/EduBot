using Simulator.BotControl;
using Simulator.Models;
using Simulator.Properties;
using Simulator.TelegramBotLibrary;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Simulator.Commands
{
    public class TelegramIDCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            await Task.Run(() =>
            {
                UserTableCommand.SetDialogState(userId, BotControl.State.DialogState.None);
                string userTelegramIdString = Resources.TelegramId;
                userTelegramIdString += $"\n{userId}";
                if (UserTableCommand.IsAdmin(userId))
                {
                    botClient.SendTextMessageAsync(chatId: userId,
                        text: userTelegramIdString,
                        replyMarkup: CommandKeyboard.AdminMenu);
                }
                else
                {
                    botClient.SendTextMessageAsync(
                                   chatId: userId,
                                   text: userTelegramIdString,
                                   replyMarkup: CommandKeyboard.UserMenu);
                }
            });
        }
    }
}
