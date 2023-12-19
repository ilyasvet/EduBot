using Simulator.BotControl;
using Simulator.Properties;
using Telegram.Bot;

namespace Simulator.Commands
{
    internal class AdminAnswersCommand : Command
    {
        public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            await CommandKeyboard.MakeGroupList("AnswersTypes", false);
            await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: Resources.ShowGroups,
                        replyMarkup: CommandKeyboard.GroupsList);
        }
    }
}
