using Simulator.BotControl;
using Simulator.Properties;
using Simulator.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Simulator.Commands
{
    internal class AdminShowAnswersCommand : Command
    {
        public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            var paramsArray = param.Split('-');
            string groupNumber = paramsArray[0];
            string typeAnswer = paramsArray[1];

            List<Models.User> users = await DataBaseControl.UserTableCommand.GetGroupUsers(groupNumber);

            string[] files = Directory.GetFiles(ControlSystem.messageAnswersDirectory + typeAnswer);

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string[] fileProperties = fileName.Split('.')[0].Split('-');
                Models.User user = users.Find(u => u.UserID.ToString() == fileProperties[0]);
                if (user != null)
                {
                    using (Stream fs = new FileStream(file, FileMode.Open))
                    {
                        await botClient.SendDocumentAsync(
                            chatId: userId,
                            document: new InputFileStream(
                                fs, $"{user.Surname}{user.Name}Вопрос{fileProperties[1]}.{fileName.Split('.')[1]}"
                                ),
                            disableContentTypeDetection: true
                            );
                    }
                }
            }

            await botClient.SendTextMessageAsync(chatId: userId,
                        text: Resources.ToMenu,
                        replyMarkup: CommandKeyboard.ToMainMenu);
        }
    }
}
