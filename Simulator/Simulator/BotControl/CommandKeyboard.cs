using Telegram.Bot.Types.ReplyMarkups;

namespace Simulator.BotControl
{
    public static class CommandKeyboard
    {
        public static InlineKeyboardMarkup Registration = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Регистрация", "Registration")
            },
        });
        public static InlineKeyboardMarkup LogIn = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Войти", "Login")
            },
        });
        public static InlineKeyboardMarkup EnterName = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Ввести имя", "EnterName")
            },
        });
        public static InlineKeyboardMarkup EnterSurname = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Ввести фамилию", "EnterSurname")
            },
        });
    }
   
}
