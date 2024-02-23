using Telegram.Bot;
using Telegram.Bot.Types;

namespace EduBot.Services
{
    public static class BotHandler
    {
        public async static Task<string> FileHandle(ITelegramBotClient botClient, Document messageDocument)
        {
            var file = await botClient.GetFileAsync(messageDocument.FileId);
            //все файлы, посланные боту, хранятся в одной папке

            string path = ControlSystem.tempDirectory + "/" + messageDocument.FileName;

            FileStream fs = new FileStream(path, FileMode.Create);
            await botClient.DownloadFileAsync(file.FilePath, fs);
            fs.Dispose();

            return path;
        }
    }
}
