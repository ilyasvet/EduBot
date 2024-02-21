using Simulator.BotControl;
using Simulator.Commands;
using EduBotCore.Properties;
using EduBotCore.Models.DbModels;
using Telegram.Bot;

namespace EduBotCore.Commands
{
	internal class AdminAddGroupToCourseCommand : Command
	{
		public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
		{
			string[] properties = param.Split('-');
			string courseName = properties[0];
			string groupNumber = properties[1] + '-' + properties[2];

			GroupCourse groupCourse = new GroupCourse()
			{
				CourseName = courseName,
				GroupNumber = groupNumber
			};
			await DataBaseControl.AddEntity(groupCourse);

			int messageId = (await botClient.SendTextMessageAsync(
				 chatId: userId,
				 text: $"Группа {groupNumber} добавлена на курс {courseName}",
				 replyMarkup: CommandKeyboard.ToMainMenu)).MessageId;

			UserFlags userFlags = await DataBaseControl.GetEntity<UserFlags>(userId);
			userFlags.StartDialogId = messageId;
			await DataBaseControl.UpdateEntity(userId, userFlags);
		}
	}
}
