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
            try
            {
                statsFilePath = await ExcelHandler.MakeStatistics(param);
                using (Stream fs = new FileStream(statsFilePath, FileMode.Open))
                {
                    await botClient.SendDocumentAsync(
                        chatId: userId,
                        document: new InputOnlineFile(fs, $"Statistics-{param}.xlsx"),
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
