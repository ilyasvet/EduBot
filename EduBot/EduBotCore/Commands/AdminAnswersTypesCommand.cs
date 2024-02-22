using EduBot.BotControl;
using EduBotCore.Properties;
using Telegram.Bot;

namespace EduBot.Commands
{
    internal class AdminAnswersTypesCommand : Command
    {
        public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            CommandKeyboard.MakeAnswersTypes(param);
            await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: Resources.ShowAnswersTypes,
                        replyMarkup: CommandKeyboard.AnswersTypesList);
        }
    }
}
