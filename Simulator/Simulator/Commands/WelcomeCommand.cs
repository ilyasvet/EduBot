using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
  class WelcomeCommand : Command
  {
    public override Task Execute(long userId, ITelegramBotClient botClient)
    {
        return null;
    }
  }
}
