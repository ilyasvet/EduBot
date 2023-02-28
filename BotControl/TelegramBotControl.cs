using System;
using Telegram.Bot;

namespace botControl
{
	class Program
	{
		static void Main(string[] args)
		{
			var botClient = new TelegramBotClient("{6250154721:AAHNyMELX0pyxhX3KbCpWFI8nSCUFdDtiv8}");
			botClient.StartReceiving(
				updateHandler: HandleUpdateAsync,
				pollingErrorHandler: HandlePollingErrorAsync,
			);
			Console.ReadLine();
		}

		async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update)
		{
			// Only process Message updates: https://core.telegram.org/bots/api#message
			if (update.Message is not { } message)
				return;
			// Only process text messages
			if (message.Text is not { } messageText)
				return;

			var chatId = message.Chat.Id;

			Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

			// Echo received message text
			Message sentMessage = await botClient.SendTextMessageAsync(
				chatId: chatId,
				text: "You said:\n" + messageText);
		}

		Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception)
		{
			var ErrorMessage = exception switch
			{
				ApiRequestException apiRequestException
					=> $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
				_ => exception.ToString()
			};

			Console.WriteLine(ErrorMessage);
			return Task.CompletedTask;
		}

	}
}