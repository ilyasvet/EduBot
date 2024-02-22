using Telegram.Bot;

namespace EduBot.Commands
{
  public abstract class Command
  {
    public abstract Task Execute(long userId, ITelegramBotClient botClient, string param = "");
  }
}
