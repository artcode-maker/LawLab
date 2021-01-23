using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LawLab.Infrastructure;
using LawLab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LawLab.Controllers
{
    public class HomeController : Controller
    {        
        public IActionResult Index()
        {
            ViewBag.Title = "Студенческая юридическая лаборатория";
            return View(GetData(nameof(Index), nameof(HomeController)));
        }

        private Dictionary<string, object> GetData(string actionName, string controllerName)
        {
            ISession session = HttpContext.Session;
            return new Dictionary<string, object>
            {
                ["Action"] = actionName,
                ["Controller"] = controllerName,
                ["User"] = HttpContext.User.Identity.Name,
                ["Authenticated"] = HttpContext.User.Identity.IsAuthenticated,
                ["Auth Type"] = HttpContext.User.Identity.AuthenticationType,
                ["IsStudent"] = session.Get<long>("student") != default ? true : false,
                ["IsClient"] = session.Get<long>("client") != default ? true : false
            };
        }
    }
}
