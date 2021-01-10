using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LawLab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LawLab.Controllers
{
    public class ChatController : Controller
    {
        private UserManager<AppUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private AppDbContext context;

        public ChatController(AppDbContext ctx,
            UserManager<AppUser> usrMgr,
            RoleManager<IdentityRole> rm)
        {
            context = ctx;
            roleManager = rm;
            userManager = usrMgr;
        }

        [Authorize]
        public IActionResult Index()
        {
            if ((bool)User?.IsInRole(""))
            {

            }
            return View();
        }
    }
}
