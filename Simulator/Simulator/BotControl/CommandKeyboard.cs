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
        public static InlineKeyboardMarkup MainMenuUser = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Главное меню", "MainMenuUser")
            },
        });
    }
   
}
