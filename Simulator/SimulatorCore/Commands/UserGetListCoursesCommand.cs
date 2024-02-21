using Simulator.BotControl;
using Simulator.Commands;
using EduBotCore.Properties;
using EduBotCore.Models.DbModels;
using Telegram.Bot;

namespace EduBotCore.Commands
{
	internal class UserGetListCoursesCommand : Command
	{
		public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
		{
			User user = await DataBaseControl.GetEntity<User>(userId);
			await CommandKeyboard.MakeCourses("ToCase", true, user.GroupNumber);
			await botClient.SendTextMessageAsync(
				chatId: userId,
				text: Resources.ChoosingCourse,
				replyMarkup: CommandKeyboard.Courses
				);
		}
	}
}
