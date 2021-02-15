using LawLab.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using LawLab.Models;
using Microsoft.EntityFrameworkCore;

namespace LawLab.Hubs
{
    public class ChatHub : Hub<IMessageClient>
    {
        private UserManager<AppUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private AppDbContext context;

        public ChatHub(AppDbContext ctx,
            UserManager<AppUser> usrMgr,
            RoleManager<IdentityRole> rm)
        {
            context = ctx;
            roleManager = rm;
            userManager = usrMgr;
        }
        
        public Task SendTo(Message message)
        {
            string userId = Context.UserIdentifier;
            AppUser user = userManager.FindByIdAsync(userId).Result;
            if (userManager.IsInRoleAsync(user, "student").Result)
            {
                Client toClient = context.Clients
                    .Include(c => c.ClientUser)
                    .First(c => c.ClientId == Int64.Parse(message.Id));
                var messageForUser = NewMessage.Create(Context.Items["Name"] as string, message);
                return Clients.User(toClient.ClientUser.Id).Send(messageForUser);
            }
            else if (userManager.IsInRoleAsync(user, "client").Result)
            {
                Student toStudent = context.Students
                    .Include(s => s.StudentUser)
                    .First(c => c.StudentId == Int64.Parse(message.Id));

                var messageForUser = NewMessage.Create(Context.Items["Name"] as string, message);
                return Clients.User(toStudent.StudentUser.Id).Send(messageForUser);
            }

            return Task.CompletedTask;
        }

        public Task StopChat(Message message)
        {
            string userId = Context.UserIdentifier;
            AppUser user = userManager.FindByIdAsync(userId).Result;
            Student toStudent = context.Students
                    .Include(s => s.StudentUser)
                    .First(c => c.StudentId == Int64.Parse(message.Id));

            return Clients.User(toStudent.StudentUser.Id).StopChat();
        }

        public override async Task OnConnectedAsync()
        {
            string userId = Context.UserIdentifier;
            AppUser user = userManager.FindByIdAsync(userId).Result;

            if (Context.User.IsInRole("student"))
            {
                Context.Items["student"] = userId;
            }
            else if (Context.User.IsInRole("client"))
            {
                Context.Items.Add("client", userId);
            }

            await base.OnConnectedAsync();
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

            return Task.FromResult("Имя не установлено");
        }
    }
}
