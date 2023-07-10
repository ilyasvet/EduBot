using Simulator.BotControl;
using Simulator.TelegramBotLibrary.JsonUserStats;
using System;
using System.Configuration;
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
            string statsDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}" +
                   $"{ConfigurationManager.AppSettings["PathStats"]}";
            string statsFilePath = statsDirectory + "\\" + ConfigurationManager.AppSettings["StatsFileName"];

            await UserCaseJsonExcelHandler.CreateAndEditExcelFile(statsFilePath, File.Exists(statsFilePath), statsDirectory);

            using (Stream fs = new FileStream(statsFilePath, FileMode.Open))
            {
                await botClient.SendDocumentAsync(
                    chatId: userId,
                    document: new InputOnlineFile(fs, "Statistics.xlsx"),
                    replyMarkup: CommandKeyboard.ToMainMenuAdmin
                    );
            }
            File.Delete(statsFilePath); // TODO Временно
        }
    }
}
