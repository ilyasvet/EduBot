using EduBot.BotControl;
using EduBot.Services;
using EduBotCore.Models.DbModels;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EduBot.Commands
{
    internal class AdminGetStatisticsCommand : Command
    {
        public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            string[] properties = param.Split('-');
            string courseName = properties[0];
            if (properties[1].Equals("all"))
            {
				var groups = await DataBaseControl.GetCollection<Group>();
				foreach (var group in groups)
				{
					await GetStatistics(userId, botClient, courseName, group.GroupNumber);
				}
            }
            else
            {
                string groupNumber = properties[1] + '-' + properties[2];
                await GetStatistics(userId, botClient, courseName, groupNumber);
            }
        }

        private async Task GetStatistics(long userId, ITelegramBotClient botClient, string courseName, string groupNumber)
        {
			string statsFilePath = string.Empty;
			try
			{
				statsFilePath = await ExcelHandler.MakeStatistics(courseName, groupNumber);
				using (Stream fs = new FileStream(statsFilePath, FileMode.Open))
				{
					await botClient.SendDocumentAsync(
					chatId: userId,
						document: new InputFileStream(fs, $"Statistics-{courseName}-{groupNumber}.xlsx"),
						replyMarkup: CommandKeyboard.ToMainMenu
						);
				}
			}
			finally
			{
				if (System.IO.File.Exists(statsFilePath))
				{
					System.IO.File.Delete(statsFilePath);
				}
			}
		}
    }
}
