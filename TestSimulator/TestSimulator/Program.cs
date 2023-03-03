using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TestSimulator.BotControl;

namespace TestSimulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TelegramBotControl telegramBotControl = new TelegramBotControl();
            telegramBotControl.managementTelegramBot();
            Console.ReadLine();
        }
    }
}
