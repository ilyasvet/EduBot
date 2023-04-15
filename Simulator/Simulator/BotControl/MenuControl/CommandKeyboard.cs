using Simulator.Models;
using Simulator.TelegramBotLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace Simulator.BotControl
{
    public static class CommandKeyboard
    {
        private static InlineKeyboardButton ListGroups = InlineKeyboardButton.WithCallbackData("Списки", "ListGroups");
        private static InlineKeyboardButton ToMenuAdmin = InlineKeyboardButton.WithCallbackData("Главное меню", "MainMenuAdmin");
        private static InlineKeyboardButton ToMenuUser = InlineKeyboardButton.WithCallbackData("Главное меню", "MainMenuUser");
        private static InlineKeyboardButton AddUsers = InlineKeyboardButton.WithCallbackData("Добавить пользователей", "AddUsersAdmin");
        private static InlineKeyboardButton GoToCase = InlineKeyboardButton.WithCallbackData("Перейти в курсу", "ToCase");
        private static InlineKeyboardButton UserCard = InlineKeyboardButton.WithCallbackData("Карточка пользователя", "UserCard");
        
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
                ToMenuUser
            },
        });
        public static InlineKeyboardMarkup ToMainMenuAdmin = new(new[]
        {
            new[]
            {
                ToMenuAdmin,
            },  
        });
        public static InlineKeyboardMarkup AdminMenu = new(new[]
        {
            new[]
            {
                ListGroups
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Добавить группу", "AddUsersAdmin"),
            },
        });
        public static InlineKeyboardMarkup GroupListUsers = new(new[]
        {
            new[]
            {
                AddUsers,
            },
            new[]
            {
                ListGroups,
            },
            new[]
            {
                ToMenuAdmin,
            },
        });
        public static InlineKeyboardMarkup UserMenu = new(new[]
        {
            new[]
            {
                UserCard,
            },
            new[]
            {
                GoToCase
            },
        });
        public static InlineKeyboardMarkup GroupsList;
        public static void MakeGroupList()
        {
            List<Group> groups = GroupTableCommand.GetAllGroups();
            InlineKeyboardButton[][] inlineKeyboardButtons = groups.Select(g => new[]
            {
                InlineKeyboardButton.WithCallbackData(g.GroupNumber, $"ShowUsersInfo|{g.GroupNumber}")
            }).ToArray();
            var temp = inlineKeyboardButtons.ToList();
            temp.Add(new[] { ToMenuAdmin });
            inlineKeyboardButtons = temp.ToArray();
            GroupsList = new InlineKeyboardMarkup(inlineKeyboardButtons);
        }
    }
}
