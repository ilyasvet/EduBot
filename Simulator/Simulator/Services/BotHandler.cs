using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Simulator.Services
{
    public static class BotHandler
    {
        public async static Task<string> FileHandle(ITelegramBotClient botClient, Document messageDocument)
        {
            var file = await botClient.GetFileAsync(messageDocument.FileId);
            //все файлы, посланные боту, хранятся в одной папке
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}" +
                $"{ConfigurationManager.AppSettings["DocumentsDir"]}" +
                $"\\{messageDocument.FileName}";
            FileStream fs = new FileStream(path, FileMode.Create);
            await botClient.DownloadFileAsync(file.FilePath, fs);
            fs.Dispose();
            return path;
        }
    }
}
