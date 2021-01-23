using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using System.Drawing;
using System.Threading;

namespace LawLabBot
{
    class Program
    {
        static TelegramBotClient Bot;
        static void Main(string[] args)
        {
            Bot = new TelegramBotClient("1547518014:AAE0qPxvPM94UMD6PomY9IQCdwa0xdZEQkA");

            var me = Bot.GetMeAsync().Result;

            Bot.OnMessage += Bot_OnMessageReceived;
            Bot.OnCallbackQuery += Bot_OnCallbackQueryReceived;

            Console.WriteLine(me.FirstName);

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static async void Bot_OnCallbackQueryReceived(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            string messageText = e.CallbackQuery.Data;
            string name = $"{e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName}";
            Console.WriteLine($"{name} нажал кнопку {messageText}");

            try
            {
                await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы нажали кнопку {messageText}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async void Bot_OnMessageReceived(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            Command command = new Command();
            var message = e.Message;

            if (message == null || message.Type != MessageType.Text)
            {
                return;
            }

            string name = $"{message.From.FirstName} {message.From.LastName}";
            Console.WriteLine($"{name} отправил сообщение: {message.Text}");

            switch (message.Text)
            {
                // Inline buttons
                case "/start":
                    string text =
                        "\U0001F170 Telegram-бот юридической лаборатории приветствует нового посетителя.\n" +
                        "\U000025B6 С использованием бота Вы можете получить информацию о правилах проекта и другие сведения.\n" +
                        "\U000025B6 Кроме того, Вы можете найти информацию об основателе проекта.\n\n";
                    await Bot.SendTextMessageAsync(message.From.Id, text, parseMode: ParseMode.Html);
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Выберите пункт меню для получения интересующей Вас информации",
                        replyMarkup: command.KeyboardMarkup);
                    break;

                // Keyboard buttons
                case "Правила проекта":
                    string textProject =
"<b>Правила проекта \"Юридическая Лаборатория\" (LawLab):</b>\n" +
"1) Уважать собеседника. Участники проекта должны проявлять вежливость по отношению друг к другу.\n" +
"2) Проект является некоммерческой площадкой. За юридические консультации студенты не вправе получать вознаграждение.\n" +
"3) Правовая помощь студентов не является профессиональной. При этом студент прикладывает максимум усилий к достижению высокого качества юридической консультации.\n" +
"4) Основатель проекта не является участником отношений студентов и клиентов.\n";
                    await Bot.SendTextMessageAsync(message.From.Id, textProject, parseMode: ParseMode.Html);
                    break;

                case "Студентам":
                    string textToStudents =
"<b>Студенты проекта \"Юридическая Лаборатория\" (LawLab) -</b> это люди, которые на безвозмездной основе оказывают правовую помощь гражданам (клиентам). Студент обязан обучаться на любом курсе учебного заведения по юридической специальности.\n";
                    await Bot.SendTextMessageAsync(message.From.Id, textToStudents, parseMode: ParseMode.Html);
                    break;

                case "Клиентам":
                    string textToClients =
"<b>Клиенты проекта \"Юридическая Лаборатория\" (LawLab) -</b> это люди, которые могут обратиться за правовой помощью к студентам-юристам. Гражданство не имеет значения. Обратившиеся могут быть гражданами (поддаными) любого государства.\n";
                    await Bot.SendTextMessageAsync(message.From.Id, textToClients, parseMode: ParseMode.Html);
                    break;

                case "О создателе проекта":
                    string textContact =
"<b>Основатель проекта \"Юридическая Лаборатория\" (LawLab) - слушатель гр.90321-2 ИИТ БГУИР Клиндухов Артём Олегович:</b>\n" +
"Телефоны:\n\U0000260E A1 (Viber, Telegram): +375(29) 36-14-333\n \U0001F4DE МТС (WhatsApp): +375(29) 84-06-418\n";
                    await Bot.SendTextMessageAsync(message.From.Id, textContact, parseMode: ParseMode.Html);
                    break;

                default:
                    await Bot.SendTextMessageAsync(message.From.Id, "Пожалуйста, используйте кнопки для общения с Ботом");
                    break;
            }


        }
    }
}
