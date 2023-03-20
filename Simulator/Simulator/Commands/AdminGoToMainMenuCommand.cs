using Simulator.BotControl;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class AdminGoToMainMenuCommand : Command
    {
        public override Task Execute(long userId, ITelegramBotClient botClient)
        {
            return Task.Run(() =>
            {
                botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.AdminMenu,
                            replyMarkup: CommandKeyboard.AdminMenu);
            });
        }
    }
}
