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
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private AppDbContext context;
        private SignInManager<AppUser> signInManager;
        public AccountController(AppDbContext ctx,
            UserManager<AppUser> usrMgr,
            SignInManager<AppUser> siM)
        {
            context = ctx;
            userManager = usrMgr;
            signInManager = siM;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ISession session = HttpContext.Session;
                AppUser user = await userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    if (context.Students
                        .Include(s => s.StudentUser)
                        .ToArray()
                        .Select(s => s.StudentUserId)
                        .Contains(user.Id))
                    {
                        long studentId = context.Students
                        .Include(s => s.StudentUser)
                        .ToArray()
                        .First(s => s.StudentUserId.Contains(user.Id)).StudentId;
                        SecurityHelper.LoggedInStudents.Add(studentId);

                        await signInManager.SignOutAsync();
                        Microsoft.AspNetCore.Identity.SignInResult result =
                            await signInManager.PasswordSignInAsync(
                                user, details.Password, false, false);
                        if (result.Succeeded)
                        {
                            session.Set<long>("student", studentId);
                            return Redirect(returnUrl ?? "/");
                        }
                    }
                    else if (context.Clients
                        .Include(c => c.ClientUser)
                        .ToArray()
                        .Select(c => c.ClientUserId)
                        .Contains(user.Id))
                    {
                        long clientId = context.Clients
                        .Include(c => c.ClientUser)
                        .ToArray()
                        .First(s => s.ClientUserId.Contains(user.Id)).ClientId;
                        SecurityHelper.LoggedInClients.Add(clientId);

                        await signInManager.SignOutAsync();
                        Microsoft.AspNetCore.Identity.SignInResult result =
                            await signInManager.PasswordSignInAsync(
                                user, details.Password, false, false);
                        if (result.Succeeded)
                        {
                            session.Set<long>("client", clientId);
                            return Redirect(returnUrl ?? "/");
                        }
                    }
                }
                ModelState.AddModelError(nameof(LoginModel.Email), "Несуществующий пользователь или пароль");
            }
            return View(details);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            ISession session = HttpContext.Session;
            long idUser = session.Get<long>("student");
            if (idUser != default)
            {
                SecurityHelper.LoggedInStudents.Remove(idUser);
                session.Clear();
            }
            else
            {
                idUser = session.Get<long>("client");
                if (idUser != default)
                {
                    SecurityHelper.LoggedInClients.Remove(idUser);
                    session.Clear();
                }
            }

            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
