using Simulator.BotControl;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    internal class AdminShowListCoursesForStatistics : Command
    {
        public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            await CommandKeyboard.MakeCourses("GetStatistics");
            await botClient.SendTextMessageAsync(
                chatId: userId,
                text: Resources.ChoosingCourse,
                replyMarkup: CommandKeyboard.Courses
                );
        }
    }
}
