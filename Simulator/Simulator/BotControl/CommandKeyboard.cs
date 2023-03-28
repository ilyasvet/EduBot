using Simulator.Models;
using Simulator.TelegramBotLibrary;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace Simulator.BotControl
{
    public static class CommandKeyboard
    {
        public static InlineKeyboardMarkup LogIn = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Войти", "Login")
            },
        });
        public static InlineKeyboardMarkup ToMainMenuUser = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Главное меню", "MainMenuUser")
            },
        });
        public static InlineKeyboardMarkup ToMainMenuAdmin = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Главное меню", "MainMenuAdmin")
            },
        });
        public static InlineKeyboardMarkup AdminMenu = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Списки", "ListGroups"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Добавить группу", "AddUsersAdmin"),
            },
        });
        public static InlineKeyboardMarkup AddUsers = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Добавить пользователей", "AddUsersAdmin")
            },
        });
        public static InlineKeyboardMarkup UserMenu = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Карточка пользователя", "UserCard")
            },
        });
        public static InlineKeyboardMarkup GroupsList;

        public static void MakeGroupList()
        {

        }
    }
   
}
