using Simulator.BotControl.State;
using Simulator.Models;
using Simulator.Properties;
using Simulator.Services;
using System;
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
                    default:
                        break;
                }
            });
        }

        private static async Task AddClassLeader(long userId, ITelegramBotClient botClient, string message)
        {
            string[] userProperties = message.Split(' ');
            string errorMessage = null;
            if (userProperties.Length == 4)
            {
                string telegramId = userProperties[0];
                if(long.TryParse(telegramId, out long longId) && longId >= 0)
                {
                    string name = userProperties[1];
                    if (UserHandler.IsCorrectName(name))
                    {
                        string surname = userProperties[2];
                        if (UserHandler.IsCorrectName(surname))
                        {
                            string group = userProperties[3];
                            if (GroupHandler.IsCorrectGroupNumber(group))
                            {
                                User groupLeader = new(longId, name, surname);
                                await DataBaseControl.UserTableCommand.AddUser(groupLeader, UserType.ClassLeader);
                                await GroupHandler.AddGroup(group);
                                await DataBaseControl.UserTableCommand.SetGroup(longId, group);
                                await botClient.SendTextMessageAsync(
                                    chatId: userId,
                                    text: Resources.SuccessAddGroup);
                            }
                            else
                            {
                                errorMessage = Resources.WrongFormatGroup;
                            }
                        }
                        else
                        {
                            errorMessage = Resources.WrongFormatSurname;
                        }
                    }
                    else
                    {
                        errorMessage = Resources.WrongFormatName;
                    }
                }
                else
                {
                    errorMessage = Resources.WrongFormatID;
                }
            }
            else
            {
                errorMessage = Resources.WrongFormat;
            }
            await botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: errorMessage);
        }

        private static async Task CheckPassword(long userId, ITelegramBotClient botClient, string password)
        {
            User user = await DataBaseControl.UserTableCommand.GetUserById(userId);
            string groupPassword = await DataBaseControl.GroupTableCommand.GetPassword(user.GroupNumber);
            if(password == groupPassword)
            {
                await DataBaseControl.UserTableCommand.SetLogedIn(userId, true);
                await DataBaseControl.UserTableCommand.SetDialogState(userId, DialogState.None);
                await botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.RightPassword,
                            replyMarkup: CommandKeyboard.ToMainMenu);
            }
            else
            {
                await botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.WrongPassword);
            }
        }
    }
}
