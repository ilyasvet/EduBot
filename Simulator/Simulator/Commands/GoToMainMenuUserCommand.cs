using Simulator.BotControl;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class GoToMainMenuUserCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            await Task.Run(() =>
            {
                botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.UserMenu,
                            replyMarkup: CommandKeyboard.UserMenu);
            });
        }
    }
}
