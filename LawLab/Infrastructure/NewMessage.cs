using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LawLab.Infrastructure
{
    public sealed class NewMessage : Message
    {
        public string Sender { get; set; }

        private NewMessage() { }

        public static NewMessage Create(string sender, Message message)
        {
            return new NewMessage
            {
                Sender = string.IsNullOrWhiteSpace(sender) ? "Имя не установлено" : sender,
                Text = message.Text,
                Id = message.Id
            };
        }
    }
}
