using Simulator.BotControl;
using Simulator.Properties;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class FindOutCoursesEnrolledCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            string userGroupNumber = await DataBaseControl.UserTableCommand.GetGroupNumber(userId);
            List<string> groupCourses = await DataBaseControl.UserTableCommand.GetGroupCourses(userGroupNumber);

            string textAnswer = Resources.CoursesEnrolled;
            foreach (string course in groupCourses)
            {
                textAnswer += "\n" + course;
            }

            await botClient.SendTextMessageAsync(chatId: userId,
                text: textAnswer,
                replyMarkup: CommandKeyboard.ToMainMenu);
        }
    }
}
