using Simulator.BotControl;
using Simulator.Services;



TelegramBotControl telegramBotControl = new TelegramBotControl(
            ControlSystem.botToken
            );

ControlSystem.CreateDirectories();

await telegramBotControl.ManageTelegramBot();
Console.ReadLine();