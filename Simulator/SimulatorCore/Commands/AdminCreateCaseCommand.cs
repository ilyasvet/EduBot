using Simulator.BotControl;
using Simulator.BotControl.State;
using Simulator.Properties;
using SimulatorCore.Models.DbModels;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class AdminCreateCaseCommand : Command
    {
        public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
			UserState userState = await DataBaseControl.GetEntity<UserState>(userId);
			userState.SetDialogState(DialogState.CreatingCase);
			await DataBaseControl.UpdateEntity(userId, userState);

			int messageId = (await botClient.SendTextMessageAsync(
                chatId: userId,
                text: Resources.CreateCase,
                replyMarkup: CommandKeyboard.ToMainMenu)).MessageId;

			UserFlags userFlags = await DataBaseControl.GetEntity<UserFlags>(userId);
			userFlags.StartDialogId = messageId;
			await DataBaseControl.UpdateEntity(userId, userFlags);
		}
    }
}
