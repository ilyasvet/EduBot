using Simulator.BotControl;
using Simulator.Services;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;

namespace Simulator.Commands
{
    internal class AdminGetStatisticsCommand : Command
    {
        public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            string statsFilePath = string.Empty;

            string[] properties = param.Split('-');
            string courseName = properties[0];
            string groupNumber = properties[1];
            
            try
            {
                statsFilePath = await ExcelHandler.MakeStatistics(courseName, groupNumber);
                using (Stream fs = new FileStream(statsFilePath, FileMode.Open))
                {
                    await botClient.SendDocumentAsync(
                        chatId: userId,
                        document: new InputOnlineFile(fs, $"Statistics-{courseName}-{groupNumber}.xlsx"),
                        replyMarkup: CommandKeyboard.ToMainMenu
                        );
                }
            }
            finally
            {
                if (File.Exists(statsFilePath))
                {
                    File.Delete(statsFilePath);
                }
            }
        }
    }
}
