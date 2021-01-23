using LawLab.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace LawLab.Hubs
{
    public class ChatHub : Hub<IMessageClient>
    {
        List<string> groupIds = new List<string>();
        string groupName = "default";
        public Task SendToOthers(Message message)
        {
            var messageForClient = NewMessage.Create(Context.Items["Name"] as string, message);
            return Clients.Others.Send(messageForClient);
        }

        public Task Send(Message message, string to)
        {
            var messageForClient = NewMessage.Create(Context.Items["Name"] as string, message);

            if (Context.UserIdentifier != to)
                return Clients.User(Context.UserIdentifier).Send(messageForClient);
            return Clients.User(to).Send(messageForClient);
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await base.OnConnectedAsync();
        }

        public Task NewChatSession()
        {
            string str = Task.FromResult($"{GetIdAndRole().Result}").Result;
            return Clients.Others.SendLink(str);
        }

        public Task<string> GetIdAndRole()
        {
            ISession session = GetHttpContextExtensions.GetHttpContext(Context).Session;

            long studentId = session.Get<long>("student");
            long clientId = session.Get<long>("client");

            if (studentId != default)
            {
                //string link = $"<a href=\"/Chat?idUser={studentId}&amp;role=student\">Подключиться</a>";
                string link = $"Chat?idUser={studentId}&role=student";
                return Task.FromResult($"{link}");
            }
            else
            {
                //string link = $"<a href=\"/Chat?idUser={clientId}&amp;role=client\">Подключиться</a>";
                string link = $"Chat?idUser={clientId}&role=client";
                return Task.FromResult($"{link}");
            }
        }

        public Task<string> SendIdentifier()
        {
            ISession session = GetHttpContextExtensions.GetHttpContext(Context).Session;

            long studentId = session.Get<long>("student");
            long clientId = session.Get<long>("client");

            if (studentId != default)
            {
                return Task.FromResult($"student{studentId}");
            }
            else
            {
                return Task.FromResult($"client{clientId}");
            }
        }

        public Task SetGroupId(string id)
        {
            groupIds.Add(id);

            return Task.CompletedTask;
        }

        public Task SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Task.CompletedTask;

            if (Context.Items.ContainsKey("Name"))
                Context.Items["Name"] = name;
            else
                Context.Items.Add("Name", name);

            return Task.CompletedTask;
        }

        public Task<string> GetName()
        {
            if (Context.Items.ContainsKey("Name"))
                return Task.FromResult(Context.Items["Name"] as string);

            return Task.FromResult("Anonymous");
        }
    }
}
