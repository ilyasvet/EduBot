using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class AdminAddNewUsers : Command
    {
        public override Task Execute(long userId, ITelegramBotClient botClient)
        {
            return Task.Run(() =>
            {
                
            });
        }
    }
}