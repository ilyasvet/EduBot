using Simulator.BotControl;
using Simulator.BotControl.State;
using EduBotCore.Properties;
using EduBotCore.Models.DbModels;
using Telegram.Bot;

namespace Simulator.Commands
{
    internal class AdminAddCaseCommand : Command
    {
        public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            UserState userState = await DataBaseControl.GetEntity<UserState>(userId);
            userState.SetDialogState(DialogState.AddingCase);
            await DataBaseControl.UpdateEntity(userId, userState);

            int messageId = (await botClient.SendTextMessageAsync(
                  chatId: userId,
                  text: Resources.AddCase,
                  replyMarkup: CommandKeyboard.ToMainMenu)).MessageId;

            UserFlags userFlags = await DataBaseControl.GetEntity<UserFlags>(userId);
            userFlags.StartDialogId = messageId;
            await DataBaseControl.UpdateEntity(userId, userFlags);
        }
    }
}
