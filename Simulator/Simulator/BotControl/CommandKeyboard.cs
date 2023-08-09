using Simulator.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Simulator.BotControl
{
    public static class CommandKeyboard
    {
        private static InlineKeyboardButton ListGroups = InlineKeyboardButton.WithCallbackData("Списки", "ListGroups|ShowUsersInfo");
        private static InlineKeyboardButton ToMenu = InlineKeyboardButton.WithCallbackData("Главное меню", "MainMenu");
        private static InlineKeyboardButton AddUsers = InlineKeyboardButton.WithCallbackData("Добавить пользователей", "AddUsersAdmin");
        private static InlineKeyboardButton GoToCase = InlineKeyboardButton.WithCallbackData("Перейти к курсу", "ToCase");
        private static InlineKeyboardButton UserCard = InlineKeyboardButton.WithCallbackData("Карточка пользователя", "UserCard");
        private static InlineKeyboardButton AddCase = InlineKeyboardButton.WithCallbackData("Добавить кейс", "AddCase");
        private static InlineKeyboardButton GetStatistics = InlineKeyboardButton.WithCallbackData("Сатистика", "ListGroups|GetListCourses");
        private static InlineKeyboardButton CreateCase = InlineKeyboardButton.WithCallbackData("Создать кейс", "CreateCase");
        private static InlineKeyboardButton CheckTelegramId = InlineKeyboardButton.WithCallbackData("Узнать свой Telegram ID", "CheckTelegramId");
        private static InlineKeyboardButton AddGroupLider = InlineKeyboardButton.WithCallbackData("Добавить старосту на курс", "AddGroupLider");
        private static InlineKeyboardButton LogInButton = InlineKeyboardButton.WithCallbackData("Войти", "Login");
        private static InlineKeyboardButton AnswersButton = InlineKeyboardButton.WithCallbackData("Ответы пользователей", "AnswersFirst");
        private static InlineKeyboardButton MyGroup = InlineKeyboardButton.WithCallbackData("Группа", "ShowUsersInfo");
        private static InlineKeyboardButton Edit = InlineKeyboardButton.WithCallbackData("Изменить данные", "EditUserInfo");
        private static InlineKeyboardButton CheckCoursesEnrolled = InlineKeyboardButton.WithCallbackData("Узнать, на какие курсы я записан", "CheckCoursesEnrolled");

        public static InlineKeyboardButton ToFinishButton = InlineKeyboardButton.WithCallbackData("Выйти", "ToOut");
        public static InlineKeyboardButton NextButton = InlineKeyboardButton.WithCallbackData("Далее", "MoveNext");

        public static InlineKeyboardMarkup StageMenu = new(new List<InlineKeyboardButton[]>
        {
            new[] { ToFinishButton },
            new[] { NextButton },
        });

        public static InlineKeyboardMarkup NewUserMenu = new(new[]
        {
            new[] { CheckCoursesEnrolled },
            new[] { CheckTelegramId },
        });
        public static InlineKeyboardMarkup LogIn = new(new[]
        {
            new[] { LogInButton },
        });
        public static InlineKeyboardMarkup TelegramId = new(new[]
        {
            new[] { CheckTelegramId },
        });

        public static InlineKeyboardMarkup ToMainMenu = new(new[]
        {
            new[] { ToMenu },  
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
            new[] { CheckTelegramId },
            new[] { AddGroupLider },
            new[] { AnswersButton },
        });
        public static InlineKeyboardMarkup UserMenu = new(new[]
        {
            new[] { UserCard },
            new[] { GoToCase },
        });

        public static InlineKeyboardMarkup BackToUserCard = new(new[]
        {
            new[] { UserCard },
        });

        public static InlineKeyboardMarkup GroupLeaderMenu = new(new[]
        {
            new[] { UserCard },
            new[] { GoToCase },
            new[] { AddUsers },
            new[] { MyGroup },
        });

        public static InlineKeyboardMarkup UserCardMenu = new(new[]
        {
            new[] { Edit },
            new[] { ToMenu },
        });

        public static InlineKeyboardMarkup GroupsList;
        public static InlineKeyboardMarkup AnswersTypesList;
        public static InlineKeyboardMarkup Courses;
        public async static Task MakeGroupList(string command, bool all)
        {
            List<Group> groups = await DataBaseControl.GroupTableCommand.GetAllGroups();
            List<InlineKeyboardButton[]> inlineKeyboardButtons = groups.Select(g => new[]
            {
                InlineKeyboardButton.WithCallbackData(g.GroupNumber, $"{command}|{g.GroupNumber}")
            }).ToList();
            if (all)
            {
                inlineKeyboardButtons.Add(new[] { InlineKeyboardButton.WithCallbackData("Все группы", $"{command}|all") });
            }
            inlineKeyboardButtons.Add(new[] { ToMenu });
            GroupsList = new InlineKeyboardMarkup(inlineKeyboardButtons);
        }

        public static void MakeAnswersTypes(string groupNumber)
        {
            string commandName = "ShowAnswers";
            List<InlineKeyboardButton[]> inlineKeyboardButtons = new()
            {
                new[] { InlineKeyboardButton.WithCallbackData("Видео", $"{commandName}|{groupNumber}-/videos/") },
                new[] { InlineKeyboardButton.WithCallbackData("Аудио", $"{commandName}|{groupNumber}-/audios/") },
                new[] { InlineKeyboardButton.WithCallbackData("Текст", $"{commandName}|{groupNumber}-/texts/") },
                new[] { InlineKeyboardButton.WithCallbackData("Другой документ", $"{commandName}|{groupNumber}-/other/") },
                new[] { InlineKeyboardButton.WithCallbackData("К группам", "AnswersFirst") },
            };
            AnswersTypesList = new InlineKeyboardMarkup(inlineKeyboardButtons);
        }

        public static async Task MakeCourses(string command, string groupNumber = "")
        {
            List<string> courses = await DataBaseControl.CourseTableCommand.GetListCourses();
            List<InlineKeyboardButton[]> inlineKeyboardButtons = courses.Select(courseName => new[]
            {
                InlineKeyboardButton.WithCallbackData(courseName, $"{command}|{courseName}-{groupNumber}")
            }).ToList();

            inlineKeyboardButtons.Add(new[] { ToMenu });

            Courses = new InlineKeyboardMarkup(inlineKeyboardButtons);
        }
    }
}
