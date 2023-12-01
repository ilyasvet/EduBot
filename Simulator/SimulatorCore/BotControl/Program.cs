using Simulator.BotControl;
using Simulator.Services;



TelegramBotControl telegramBotControl = new TelegramBotControl(
            ControlSystem.botToken
            );

ControlSystem.CreateDirectories();

telegramBotControl.ManageTelegramBot();
Console.ReadLine();