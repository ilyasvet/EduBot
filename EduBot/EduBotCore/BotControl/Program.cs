using EduBot.BotControl;
using EduBot.Services;



TelegramBotControl telegramBotControl = new TelegramBotControl(
            ControlSystem.botToken
            );

ControlSystem.CreateDirectories();

await telegramBotControl.ManageTelegramBot();
Console.ReadLine();