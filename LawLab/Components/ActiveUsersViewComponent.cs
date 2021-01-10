using LawLab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using LawLab.Infrastructure;

namespace LawLab.Components
{
    public class ActiveUsersViewComponent : ViewComponent
    {
        private AppDbContext context;
        public ActiveUsersViewComponent(AppDbContext ctx)
        {
            context = ctx;
        }

        public IViewComponentResult Invoke(Dictionary<string, object> userData)
        {
            List<Student> students = context.Students.Include(s => s.StudentUser).Where(s => SecurityHelper.LoggedInStudents.Contains(s.StudentId)).ToList();
            List<Client> clients = context.Clients.Include(c => c.ClientUser).Where(c => SecurityHelper.LoggedInClients.Contains(c.ClientId)).ToList();
            (List<Student>, List<Client>, Dictionary<string, object>) activeUsers = (students, clients, userData);
            return View(activeUsers);
        }
    }
}
