using System;
using System.Configuration;
using System.IO;

namespace Simulator.BotControl
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string docDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}" +
                $"{ConfigurationManager.AppSettings["DocumentsDir"]}";
            // TODO сделать поуниверсальней
            
            TelegramBotControl telegramBotControl = new TelegramBotControl(
                ConfigurationManager.AppSettings["BotToken"]
                );

            if(!Directory.Exists(docDirectory))
            {
                Directory.CreateDirectory(docDirectory);
            }

            telegramBotControl.ManageTelegramBot();
            Console.ReadLine();
        }
    }
}
