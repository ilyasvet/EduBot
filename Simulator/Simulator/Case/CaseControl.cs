using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Simulator.Case
{
    internal static class CaseControl
    {
        private static long UserId;
        private static int UserPoint;
        private static Update Update;
        private static ITelegramBotClient BotClient;
        public static void SetParameters(long userId, Update update, ITelegramBotClient botClient)
        {
            UserId = userId;
            Update = update;
            BotClient = botClient;
            UserPoint = UserCaseTableCommand.GetPoint(userId);
        }
        public static async Task CaseCallback()
        {
            try
            {
                switch (Update.Type)
                {
                    case UpdateType.Message:
                        MessageHandle();
                        break;
                    case UpdateType.CallbackQuery:
                        CallbackQueryHandle();
                        break;
                    default:
                        break;
                }
            }
            catch(Exception ex)
            { }

        }
        private static async Task MessageHandle()
        {
            Message message = Update.Message;
            //TODO обработка сообщения от пользователя 
        }
        private static async Task CallbackQueryHandle()
        {
            CallbackQuery callbackQuery = Update.CallbackQuery;
        }
    }
}
