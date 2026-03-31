using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ConsoleBot;

public partial class BotService
{
    TelegramBotClient bot;
    CancellationTokenSource cts = new ();
    // This class can be used to implement the bot logic.
    public BotService()
    {
        // todo : move Token to separate file and ignore it in git 
        // for example, create Token.cs with content:
        // namespace ConsoleBot;
        // public partial class BotService
        // {
        //     readonly string Token = "BOT_TOKEN";
        // }
        bot = new TelegramBotClient(Token, cancellationToken: cts.Token);

        bot.OnMessage += OnMessage;
    }

    private async Task OnMessage(Message message, UpdateType type)
    {
        if (message.Text is null) return;
        Console.WriteLine($"Received message from {message.Chat.FirstName}: {message.Text}");
        await bot.SendMessage(message.Chat.Id, $"You said: {message.Text}");
    }

    public async Task Run()
    {
        //bot.StartReceiving();

        var commands = new List<BotCommand>
        {
            new Telegram.Bot.Types.BotCommand { Command = "start", Description = "Start the bot" },
            new Telegram.Bot.Types.BotCommand { Command = "help", Description = "Show help information" },
            new Telegram.Bot.Types.BotCommand { Command = "test", Description = "Show information" }
        };

        await bot.SetMyCommands(commands);

        var me = await bot.GetMe();

        Console.WriteLine($"Bot {me.Username} is starting...");
        Console.WriteLine("Bot is running. Press any key to stop...");
        Console.ReadKey();
        cts.Cancel();
        Console.WriteLine("Bot is stoped...");

    }
}

internal class Program
{
    static void Main(string[] args)
    {
        new BotService().Run().Wait();
    }
}
