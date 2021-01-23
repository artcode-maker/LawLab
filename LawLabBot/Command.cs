using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace LawLabBot
{
    public class Command
    {
        public ReplyKeyboardMarkup KeyboardMarkup { get; set; }

        public Command()
        {
            KeyboardMarkup = new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                      new KeyboardButton("Правила проекта"),
                      new KeyboardButton("О создателе проекта")
                },
                new[]
                {
                      new KeyboardButton("Студентам"),
                      new KeyboardButton("Клиентам")
                }
            }, resizeKeyboard: true);
        }


    }
}
