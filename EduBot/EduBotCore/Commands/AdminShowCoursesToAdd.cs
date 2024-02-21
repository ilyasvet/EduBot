using Simulator.BotControl;
using Simulator.Commands;
using EduBotCore.Properties;
using Telegram.Bot;

namespace EduBotCore.Commands
{
	internal class AdminShowCoursesToAdd : Command
	{
		public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
		{
			//param - номер группы
			await CommandKeyboard.MakeCourses("AddGroupToCourse", false, param);
			await botClient.SendTextMessageAsync(
				chatId: userId,
				text: Resources.ChoosingCourse,
				replyMarkup: CommandKeyboard.Courses
				);
		}
	}
}
