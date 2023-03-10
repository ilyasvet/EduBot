using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
  public abstract class Command
  {
    public abstract Task Execute(long userId, ITelegramBotClient botClient);
  }
}
