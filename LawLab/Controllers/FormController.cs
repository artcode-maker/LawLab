using LawLab.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LawLab.Components
{
    [ViewComponent(Name = "MainForm")]
    public class FormController : Controller
    {
        private UserManager<AppUser> userManager;
        private AppDbContext context;
        public FormController(AppDbContext ctx, UserManager<AppUser> usrMgr)
        {
            context = ctx;
            userManager = usrMgr;
        }

        // Student's Ajax Actions for Form
        [HttpGet]
        public IActionResult CreateStudentForm()
        {
            ViewBag.User = "Student";
            (Student, Client) users = (new Student(), new Client());
            return PartialView("CreateForm", users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudentForm(Student student)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = student.FirstName + student.FamilyName,
                    Email = student.Email
                };
                IdentityResult result
                    = await userManager.CreateAsync(user, student.Password);
                if (result.Succeeded)
                {
                    context.Add(student);
                    context.SaveChanges();
                    return PartialView("SuccessRegistration");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
            }

            ViewBag.User = "Student";
            (Student, Client) users = (student, new Client());
            return PartialView("CreateForm", users);
        }

        // Client's Ajax Actions for Form
        [HttpGet]
        public IActionResult CreateClientForm() 
        {
            ViewBag.User = "Client";
            (Student, Client) users = (new Student(), new Client());
            return PartialView("CreateForm", users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClientForm(Client client)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = client.FirstName + client.FamilyName,
                    Email = client.Email
                };
                IdentityResult result
                    = await userManager.CreateAsync(user, client.Password);
                if (result.Succeeded)
                {
                    context.Add(client);
                    context.SaveChanges();
                    return PartialView("SuccessRegistration");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
            }

            ViewBag.User = "Client";
            (Student, Client) users = (new Student(), client);
            return PartialView("CreateForm", users);
        }

        // ViewComponent's Method
        public IViewComponentResult Invoke()
        {
            return new ViewViewComponentResult() { };
        }
    }
}
