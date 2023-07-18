using Simulator.BotControl;
using Simulator.Services;
using Simulator.TelegramBotLibrary.JsonUserStats;
using System;
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
            string statsFilePath = AppDomain.CurrentDomain.BaseDirectory +
                ControlSystem.statsDirectory +
                "\\" + ControlSystem.statsFileName;

            try
            {
                await UserCaseJsonExcelHandler.CreateAndEditExcelFile(statsFilePath);

                using (Stream fs = new FileStream(statsFilePath, FileMode.Open))
                {
                    await botClient.SendDocumentAsync(
                        chatId: userId,
                        document: new InputOnlineFile(fs, "Statistics.xlsx"),
                        replyMarkup: CommandKeyboard.ToMainMenu
                        );
                }
            }
            finally
            {
                File.Delete(statsFilePath);
            }
        }
    }
}
