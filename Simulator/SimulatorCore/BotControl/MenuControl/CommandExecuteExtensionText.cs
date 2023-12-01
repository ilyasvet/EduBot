using Simulator.BotControl.State;
using Simulator.Models;
using Simulator.Properties;
using Simulator.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Simulator.BotControl
{
    public static class CommandExecuteExtensionText
    {
        public static async Task Execute(long userId, ITelegramBotClient botClient, Message message)
        {
            await Task.Run(async () =>
            {
                DialogState state = await DataBaseControl.UserTableCommand.GetDialogState(userId);
                bool resultOperation = false;
                switch (state)
                {
                    case DialogState.None:
                        await botClient.DeleteMessageAsync(userId, message.MessageId);
                        break;
                    case DialogState.EnterPassword:
                        resultOperation = await CheckPassword(userId, botClient, message);
                        break;
                    case DialogState.AddingGroupLeader:
                        resultOperation = await AddClassLeader(userId, botClient, message);
                        break;
                    case DialogState.EditingUserInfo:
                        resultOperation = await EditUserInfo(userId, botClient, message);
                        break;
                    default:
                        await BotCallBack(userId, botClient, Resources.WrongArgumentMessage);
                        break;
                }
                if (resultOperation)
                {
                    await DataBaseControl.UserTableCommand.SetDialogState(userId, DialogState.None);
                }
            });
        }

        private static async Task<bool> EditUserInfo(long userId, ITelegramBotClient botClient, Message message)
        {
            string[] userProperties = message.Text.Split(' ');
            string callBackMessage = string.Empty;
            bool result = false;

            if (userProperties.Length != 2)
            {
                callBackMessage = Resources.WrongFormat;
            }
            else if (!UserHandler.IsCorrectName(userProperties[0]))
            {
                callBackMessage = Resources.WrongFormatName;
            }
            else if (!UserHandler.IsCorrectName(userProperties[1]))
            {
                callBackMessage = Resources.WrongFormatSurname;
            }
            else
            {
                callBackMessage += Resources.SuccessEditing;
                await DataBaseControl.UserTableCommand.SetName(userId, userProperties[0]);
                await DataBaseControl.UserTableCommand.SetSurname(userId, userProperties[1]);
                result = true;

            }
            await BotCallBack(userId, botClient, callBackMessage);
            return result;
        }

        private static async Task<bool> AddClassLeader(long userId, ITelegramBotClient botClient, Message message)
        {
            string[] userProperties = message.Text.Split(' ');
            string callBackMessage = string.Empty;
            bool result = false;
            try
            {

                if (!long.TryParse(userProperties[0], out long longId) || longId < 0)
                {
                    throw new ArgumentException(Resources.WrongFormatID);
                }
                else if (await DataBaseControl.UserTableCommand.HasUser(longId) && userProperties.Length == 1)
                {
                    await DataBaseControl.UserTableCommand.SetType(longId, UserType.ClassLeader);
                    callBackMessage = Resources.MadeGroupLeader;
                    result = true;
                }
                else if (userProperties.Length == 4)
                {
                    string name = userProperties[1];
                    string surname = userProperties[2];
                    string group = userProperties[3];

                    Models.User groupLeader = new(longId, name, surname);
                    groupLeader.GroupNumber = group;

                    await DataBaseControl.UserTableCommand.AddUser(groupLeader, UserType.ClassLeader);
                    callBackMessage = Resources.MadeNewGroupLeader;

                    await GroupHandler.AddGroup(group);
                    await DataBaseControl.UserTableCommand.SetGroup(longId, group);
                    result = true;
                }
            }
            catch(ArgumentException ex)
            {
                callBackMessage = ex.Message;
            }

            await BotCallBack(userId, botClient, callBackMessage);
            return result;
        }

        private static async Task<bool> CheckPassword(long userId, ITelegramBotClient botClient, Message message)
        {
            Models.User user = await DataBaseControl.UserTableCommand.GetUserById(userId);
            string groupPassword = await DataBaseControl.GroupTableCommand.GetPassword(user.GroupNumber);
            if(message.Text == groupPassword)
            {
                await DataBaseControl.UserTableCommand.SetLogedIn(userId, true);
                await DataBaseControl.UserTableCommand.SetDialogState(userId, DialogState.None);
                await BotCallBack(userId, botClient, Resources.RightPassword);
                return true;
            }
            else
            {
                await botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.WrongPassword);
                return false;
            }
        }

        private async static Task BotCallBack(long userId, ITelegramBotClient botClient, string message)
        {
            await botClient.SendTextMessageAsync(
                       chatId: userId,
                       text: message,
                       replyMarkup: CommandKeyboard.ToMainMenu);
        }
    }
}
