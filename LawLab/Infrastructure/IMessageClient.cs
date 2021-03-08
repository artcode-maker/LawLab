using System.Threading.Tasks;

namespace LawLab.Infrastructure
{
    public interface IMessageClient
    {
        Task Send(NewMessage message);
        Task SendLink(string message);
        Task SendAsync(string method, NewMessage message, string userName);
        Task SendId(string id);
        Task StopChat();
    }
}
