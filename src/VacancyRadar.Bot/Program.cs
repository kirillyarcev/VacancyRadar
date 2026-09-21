using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

// 1. Читаем токен из appsettings.json
var json = File.ReadAllText("appsettings.json");
var config = JsonSerializer.Deserialize<Dictionary<string, string>>(json)!;
var token = config["BotToken"];

// 2. Подключаемся к Telegram
var bot = new TelegramBotClient(token);

// 3. Проверяем, что подключились
var me = await bot.GetMe();
Console.WriteLine($"✅ Бот @{me.Username} запущен!");
Console.WriteLine("Открой Telegram, найди бота и напиши /start");
Console.WriteLine("Чтобы остановить — нажми Enter в этом окне.");

// 4. Подписываемся на событие получения сообщений
bot.OnMessage += async (message, type) =>
{
    // Проверяем, что пришло именно текстовое сообщение
    if (message.Text is not { } text) return;

    var chatId = message.Chat.Id;

    if (text == "/start")
    {
        await bot.SendMessage(
            chatId: chatId,
            text: "👋 Привет! Я VacancyRadar.\nИщу вакансии с трёх сайтов.");
    }
    else if (text == "/vacancies")
    {
        await bot.SendMessage(
            chatId: chatId,
            text: "📋 Тут скоро будут вакансии...");
    }
    else
    {
        await bot.SendMessage(
            chatId: chatId,
            text: "Не понимаю. Напиши /start");
    }
};

// 5. Держим программу запущенной
Console.ReadLine();