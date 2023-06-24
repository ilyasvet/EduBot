using Simulator.Models;
using Simulator.TelegramBotLibrary;
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
        private static InlineKeyboardButton GoToCase = InlineKeyboardButton.WithCallbackData("Перейти к курсу", "ToCase");
        private static InlineKeyboardButton UserCard = InlineKeyboardButton.WithCallbackData("Карточка пользователя", "UserCard");
        private static InlineKeyboardButton AddCase = InlineKeyboardButton.WithCallbackData("Добавить кейс", "AddCase");
        private static InlineKeyboardButton GetStatistics = InlineKeyboardButton.WithCallbackData("Сатистика", "GetStatistics");
        private static InlineKeyboardButton CreateCase = InlineKeyboardButton.WithCallbackData("Создать кейс", "CreateCase");


        public static InlineKeyboardButton ToFinishButton = InlineKeyboardButton.WithCallbackData("Выйти", "ToOut");
        public static InlineKeyboardButton NextButton = InlineKeyboardButton.WithCallbackData("Далее", "MoveNext");
        public static InlineKeyboardButton ToBeginButton = InlineKeyboardButton.WithCallbackData("Сначала", "ToBegin");
        public static InlineKeyboardButton LogInButton = InlineKeyboardButton.WithCallbackData("Войти", "Login");

        public static InlineKeyboardMarkup StageMenu = new(new List<InlineKeyboardButton[]>
        {
            new[] { ToFinishButton },
            new[] { NextButton }
        });

        public static InlineKeyboardMarkup LogIn = new(new[]
        {
            new[] { LogInButton },
        });
        public static InlineKeyboardMarkup ToMainMenuUser = new(new[]
        {
            new[] { ToMenuUser },
        });
        public static InlineKeyboardMarkup ToMainMenuAdmin = new(new[]
        {
            new[] { ToMenuAdmin },  
        });
        public static InlineKeyboardMarkup ToGroups = new(new[]
        {
            new[] { ListGroups },
        });
        public static InlineKeyboardMarkup AdminMenu = new(new[]
        {
            new[] { ListGroups },
            new[] { AddUsers },
            new[] { AddCase },
            new[] { GetStatistics },
            new[] { CreateCase },
        });
        public static InlineKeyboardMarkup UserMenu = new(new[]
        {
            new[] { UserCard },
            new[] { GoToCase }
        });
        public static InlineKeyboardMarkup GroupsList;
        public static void MakeGroupList()
        {
            List<Group> groups = GroupTableCommand.GetAllGroups();
            List<InlineKeyboardButton[]> inlineKeyboardButtons = groups.Select(g => new[]
            {
                InlineKeyboardButton.WithCallbackData(g.GroupNumber, $"ShowUsersInfo|{g.GroupNumber}")
            }).ToList();
            inlineKeyboardButtons.Add(new[] { ToMenuAdmin });
            GroupsList = new InlineKeyboardMarkup(inlineKeyboardButtons);
        }
    }
}
