using System.Threading.Tasks;
using Simulator.BotControl;
using Simulator.Properties;
using Telegram.Bot;
using TelegramBotLibrary;

namespace Simulator.Commands
{
  class LogInCommand : Command
  {
    public override Task Execute(long userId, ITelegramBotClient botClient)
    {
        return Task.Run(() =>
        {
            botClient.SendTextMessageAsync(
                chatId: userId,
                text: Resources.RequestPassword,
                replyMarkup: CommandKeyboard.EnterPassword);
        });
    }
  }
}
