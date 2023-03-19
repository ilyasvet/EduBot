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
                InlineKeyboardButton.WithCallbackData("Затычка", "AssPlug") //Тут будут кнопки для меню админа
            },
        });
        public static InlineKeyboardMarkup UserMenu = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Затычка", "AssPlug") //Тут будут кнопки для меню пользователя
            },
        });
    }
   
}
