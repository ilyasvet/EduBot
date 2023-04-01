using System;
using System.Configuration;


namespace Simulator.BotControl
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TelegramBotControl telegramBotControl = new TelegramBotControl(
                ConfigurationManager.AppSettings["BotToken"]
                );
            telegramBotControl.ManageTelegramBot();
            Console.ReadLine();
        }
    }
}
