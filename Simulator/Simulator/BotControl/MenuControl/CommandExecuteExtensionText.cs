using Simulator.BotControl.State;
using Simulator.Models;
using Simulator.Properties;
using Simulator.Services;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.BotControl
{
    public static class CommandExecuteExtensionText
    {
        public static async Task Execute(long userId, ITelegramBotClient botClient, string message)
        {
            await Task.Run(async () =>
            {
                DialogState state = await DataBaseControl.UserTableCommand.GetDialogState(userId);
                switch (state)
                {
                    case DialogState.None:
                        break;
                    case DialogState.EnterPassword:
                        await CheckPassword(userId, botClient, message);
                        break;
                    case DialogState.AddingGroupLeader:
                        await AddClassLeader(userId, botClient, message);
                        break;
                    case DialogState.EditingUserInfo:
                        await EditUserInfo(userId, botClient, message);
                        break;
                    default:
                        await BotCallBack(userId, botClient, Resources.WrongArgumentMessage);
                        break;
                }
            });
        }

        private static async Task EditUserInfo(long userId, ITelegramBotClient botClient, string message)
        {
            string[] userProperties = message.Split(' ');
            string callBackMessage = string.Empty;

            if (userProperties.Length != 2)
            {
                callBackMessage = Resources.WrongArgumentMessage;
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
            }
            await BotCallBack(userId, botClient, callBackMessage);
        }

        private static async Task AddClassLeader(long userId, ITelegramBotClient botClient, string message)
        {
            string[] userProperties = message.Split(' ');
            string callBackMessage = string.Empty;

            if (!long.TryParse(userProperties[0], out long longId) || longId < 0)
            {
                callBackMessage = Resources.WrongFormatID;
            }
            else if(await DataBaseControl.UserTableCommand.HasUser(longId))
            {
                await DataBaseControl.UserTableCommand.SetType(longId, UserType.ClassLeader);
                callBackMessage = Resources.MadeGroupLeader;
            }
            else if (userProperties.Length != 4)
            {
                callBackMessage = Resources.WrongFormat;  
            }
            else if (!UserHandler.IsCorrectName(userProperties[1]))
            {
                callBackMessage = Resources.WrongFormatName;
            }
            else if (!UserHandler.IsCorrectName(userProperties[2]))
            {
                callBackMessage = Resources.WrongFormatSurname;
            }
            else if (!GroupHandler.IsCorrectGroupNumber(userProperties[3]))
            {
                callBackMessage = Resources.WrongFormatGroup;
            }
            else
            {
                string name = userProperties[1];
                string surname = userProperties[2];
                string group = userProperties[3];

                User groupLeader = new(longId, name, surname);
                await DataBaseControl.UserTableCommand.AddUser(groupLeader, UserType.ClassLeader);
                callBackMessage = Resources.MadeNewGroupLeader;

                await GroupHandler.AddGroup(group);
                await DataBaseControl.UserTableCommand.SetGroup(longId, group);

            }
            await BotCallBack(userId, botClient, callBackMessage);
        }

        private static async Task CheckPassword(long userId, ITelegramBotClient botClient, string password)
        {
            User user = await DataBaseControl.UserTableCommand.GetUserById(userId);
            string groupPassword = await DataBaseControl.GroupTableCommand.GetPassword(user.GroupNumber);
            if(password == groupPassword)
            {
                await DataBaseControl.UserTableCommand.SetLogedIn(userId, true);
                await DataBaseControl.UserTableCommand.SetDialogState(userId, DialogState.None);
                await BotCallBack(userId, botClient, Resources.RightPassword);
            }
            else
            {
                await botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.WrongPassword);
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
