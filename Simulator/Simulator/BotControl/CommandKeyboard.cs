using Telegram.Bot.Types.ReplyMarkups;

namespace Simulator.BotControl
{
    public static class CommandKeyboard
    {
        public static ReplyKeyboardMarkup Registration = new(new[]
            {
            new KeyboardButton[] { "/registration" },
            })
        { ResizeKeyboard = true };
        public static InlineKeyboardMarkup StartRegistration = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Имя", "UserName")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Фамилия", "UserSurname")
            },
        });
    }
   
}
