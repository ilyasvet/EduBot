using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Simulator.BotControl
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TelegramBotControl telegramBotControl = new TelegramBotControl("6071291263:AAHDfQXU8LbyuUUc_p7Qx-qyObp25Q9ttxg",
                "", "");
            telegramBotControl.ManagementTelegramBot();
            Console.ReadLine();
        }
    }
}
