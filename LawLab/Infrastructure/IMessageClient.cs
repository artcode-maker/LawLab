using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LawLab.Infrastructure
{
    public interface IMessageClient
    {
        Task Send(NewMessage message);
        Task SendLink(/*MessageToConnect message*/ string message);
        Task SendAsync(string method, NewMessage message, string userName);
        Task SendId(string id);
        Task StopChat();
    }
}
