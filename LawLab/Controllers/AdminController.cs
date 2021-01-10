using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        //public ViewResult Create() => View();

        //[HttpPost]
        //public async Task<IActionResult> Create(Student student)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        AppUser user = new AppUser
        //        {
        //            UserName = student.FirstName + student.FamilyName,
        //            Email = student.Email
        //        };
        //        IdentityResult result
        //            = await userManager.CreateAsync(user, student.Password);
        //        if (result.Succeeded)
        //        {
        //            context.Add(student);
        //            context.SaveChanges();
        //            return PartialView("SuccessRegistration");
        //        }
        //        else
        //        {
        //            foreach (IdentityError error in result.Errors)
        //            {
        //                ModelState.AddModelError(error.Code, error.Description);
        //            }
        //        }
        //    }

        //    ViewBag.User = "Student";
        //    (Student, Client) users = (student, new Client());
        //    return PartialView("CreateForm", users);
        //}
    }
}
