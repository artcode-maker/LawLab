using Microsoft.AspNetCore.Mvc;
using LawLab.Models;
using Microsoft.AspNetCore.Identity;

namespace LawLab.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;

        public AdminController(UserManager<AppUser> usrMgr)
        {
            userManager = usrMgr;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
