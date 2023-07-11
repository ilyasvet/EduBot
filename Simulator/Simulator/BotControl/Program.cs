using Simulator.Services;
using System;

namespace Simulator.BotControl
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TelegramBotControl telegramBotControl = new TelegramBotControl(
                ControlSystem.botToken
                );

            ControlSystem.CreateDirectories();

            telegramBotControl.ManageTelegramBot();
            Console.ReadLine();
        }
    }
}
