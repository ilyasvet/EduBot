using EduBot.BotControl;
using EduBotCore.Properties;
using Telegram.Bot;

namespace EduBot.Commands
{
    internal class AdminShowListCoursesForStatistics : Command
    {
        public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            //param - номер группы
            await CommandKeyboard.MakeCourses("GetStatistics", true, param);
            await botClient.SendTextMessageAsync(
                chatId: userId,
                text: Resources.ChoosingCourse,
                replyMarkup: CommandKeyboard.Courses
                );
        }
    }
}
