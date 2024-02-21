using Simulator.BotControl.State;
using EduBotCore.Properties;
using Simulator.Services;
using EduBotCore.Models.DbModels;
using Telegram.Bot;
using Telegram.Bot.Types;

using DbUser = EduBotCore.Models.DbModels.User;

namespace Simulator.BotControl
{
    public static class CommandExecuteExtensionText
    {
        public static async Task Execute(long userId, ITelegramBotClient botClient, Message message)
        {
            await Task.Run(async () =>
            {
				UserState? userState = await DataBaseControl.GetEntity<UserState>(userId);

				DialogState state = userState?.GetDialogState() ?? DialogState.None;
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
					userState.SetDialogState(DialogState.None);
					await DataBaseControl.UpdateEntity(userId, userState);
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
                DbUser user = await DataBaseControl.GetEntity<DbUser>(userId);
                user.Name = userProperties[0];
                user.Surname = userProperties[1];
                await DataBaseControl.UpdateEntity(userId, user);
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
                bool hasUser = true;
                UserState classLeader = null;
                try
                {
                    classLeader = await DataBaseControl.GetEntity<UserState>(longId);
                }
                catch (KeyNotFoundException)
                {
                    hasUser = false;
                }
                if (hasUser && userProperties.Length == 1)
                {
                    classLeader.SetUserType(UserType.ClassLeader);
                    await DataBaseControl.UpdateEntity(longId, classLeader);
                    callBackMessage = Resources.MadeGroupLeader;
                    result = true;
                }
                else if (userProperties.Length == 4)
                {
                    string name = userProperties[1];
                    string surname = userProperties[2];
                    string group = userProperties[3];

                    DbUser groupLeader = new()
                    {
                        UserID = longId,
                        Name = name,
                        Surname = surname,
                        GroupNumber = group
                    };
                    UserState groupLeaderState = new()
                    {
                        UserID = longId,

                    };
                    UserFlags groupLeaderFlags = new()
                    {
                        UserID = longId,
                    };

                    groupLeaderState.SetUserType(UserType.ClassLeader);

                    await DataBaseControl.AddEntity(groupLeader);
					await DataBaseControl.AddEntity(groupLeaderState);
					await DataBaseControl.AddEntity(groupLeaderFlags);

                    callBackMessage = Resources.MadeNewGroupLeader;


                    await GroupHandler.AddGroup(group);
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
            DbUser user = await DataBaseControl.GetEntity<DbUser>(userId);
            string groupPassword = (await DataBaseControl.GetEntity<Group>(user.GroupNumber)).Password;
            if(message.Text == groupPassword)
            {
                UserState currentState = await DataBaseControl.GetEntity<UserState>(userId);
                currentState.SetDialogState(DialogState.None);
                currentState.LogedIn = true;
                await DataBaseControl.UpdateEntity(userId, currentState);

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
