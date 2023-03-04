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
            TelegramBotControl telegramBotControl = new TelegramBotControl("", "", "");
            telegramBotControl.ManagementTelegramBot();
            Console.ReadLine();
        }
    }
}
