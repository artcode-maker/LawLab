using LawLab.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LawLab.Components
{
    public class ActiveUsersViewComponent : ViewComponent
    {
        private AppDbContext context;
        public ActiveUsersViewComponent(AppDbContext ctx)
        {
            context = ctx;
        }

        public IViewComponentResult Invoke()
        {
            (List<Student>, List<Client>) activeUsers = (context.Students.ToList(), context.Clients.ToList());
            return View(activeUsers);
        }
    }
}
