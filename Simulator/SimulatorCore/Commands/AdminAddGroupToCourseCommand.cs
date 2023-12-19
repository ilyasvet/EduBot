using Simulator.BotControl;
using Simulator.Commands;
using Simulator.Services;
using SimulatorCore.Models.DbModels;
using Telegram.Bot;

namespace SimulatorCore.Commands
{
	internal class AdminAddGroupToCourseCommand : Command
	{
		public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
		{
			string[] properties = param.Split('-');
			string courseName = properties[0];
			string groupNumber = properties[1];

			GroupCourse groupCourse = new GroupCourse()
			{
				CourseName = courseName,
				GroupNumber = groupNumber
			};
			await DataBaseControl.AddEntity(groupCourse);
		}
	}
}
