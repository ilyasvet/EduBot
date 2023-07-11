using Simulator.BotControl;
using Simulator.Services;
using Simulator.TelegramBotLibrary.JsonUserStats;
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

            string statsFilePath = ControlSystem.statsDirectory + "\\" + ControlSystem.statsFileName;

            await UserCaseJsonExcelHandler.CreateAndEditExcelFile(statsFilePath, ControlSystem.statsDirectory);

            using (Stream fs = new FileStream(statsFilePath, FileMode.Open))
            {
                await botClient.SendDocumentAsync(
                    chatId: userId,
                    document: new InputOnlineFile(fs, "Statistics.xlsx"),
                    replyMarkup: CommandKeyboard.ToMainMenuAdmin
                    );
            }
            File.Delete(statsFilePath);
        }
    }
}
