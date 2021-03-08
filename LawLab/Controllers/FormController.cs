using LawLab.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LawLab.Extensions;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace LawLab.Components
{
    [ViewComponent(Name = "MainForm")]
    public class FormController : Controller
    {
        private UserManager<AppUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private AppDbContext context;
        private IUserValidator<AppUser> userValidator;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;

        public FormController(AppDbContext ctx, 
            UserManager<AppUser> usrMgr,
            RoleManager<IdentityRole> rm,
            IUserValidator<AppUser> sv,
            IPasswordValidator<AppUser> pv,
            IPasswordHasher<AppUser> ph)
        {
            context = ctx;
            roleManager = rm;
            userManager = usrMgr;
            userValidator = sv;
            passwordValidator = pv;
            passwordHasher = ph;
        }

        public ViewResult Index()
        {
            (IEnumerable<AppUser>, IEnumerable<Student>, IEnumerable<Client>) tuple 
                = (userManager.Users.ToArray(), 
                context.Students.ToArray(), 
                context.Clients.ToArray());
            return View(tuple);
        }

        public IActionResult ChoiceRole() => View();

        // Student's Ajax Actions for Form
        [HttpGet]
        public IActionResult CreateStudentForm()
        {
            ViewBag.User = "Student";
            (Student, Client) users = (new Student(), new Client());
            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateForm", users);
            }
            else
            {
                return View("CreateCommonForm", users);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudentForm(Student student)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = student.FirstName,
                    Email = student.Email
                };                
                IdentityResult result
                    = await userManager.CreateAsync(user, student.Password);
                if (result.Succeeded)
                {
                    AppUser savedUser = await userManager.FindByEmailAsync(user.Email);
                    student.StudentUser = savedUser;
                    if (student.Foto != null)
                    {
                        if (student.Foto.Length > 0)
                        {
                            byte[] p1 = null;
                            using (var fs1 = student.Foto.OpenReadStream())
                            using (var ms1 = new MemoryStream())
                            {
                                fs1.CopyTo(ms1);
                                p1 = ms1.ToArray();
                            }
                            student.Avatar = p1;
                        }
                    }
                    context.Add(student);
                    IdentityResult identityResult =
                        await userManager.AddToRoleAsync(savedUser, "student");
                    if (!identityResult.Succeeded)
                    {
                        AddErrorsFromResult(identityResult);
                    }
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
            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateForm", users);
            }
            else
            {
                return View("CreateCommonForm", users);
            }
        }

        // Client's Ajax Actions for Form
        [HttpGet]
        public IActionResult CreateClientForm() 
        {
            ViewBag.User = "Client";
            (Student, Client) users = (new Student(), new Client());
            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateForm", users);
            }
            else
            {
                return View("CreateCommonForm", users);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateClientForm(Client client)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = client.FirstName,
                    Email = client.Email
                };
                IdentityResult result
                    = await userManager.CreateAsync(user, client.Password);
                if (result.Succeeded)
                {
                    AppUser savedUser = await userManager.FindByEmailAsync(user.Email);
                    client.ClientUser = savedUser;
                    if (client.Foto != null)
                    {
                        if (client.Foto.Length > 0)
                        {
                            byte[] p1 = null;
                            using (var fs1 = client.Foto.OpenReadStream())
                            using (var ms1 = new MemoryStream())
                            {
                                fs1.CopyTo(ms1);
                                p1 = ms1.ToArray();
                            }
                            client.Avatar = p1;
                        }
                    }
                    context.Add(client);
                    IdentityResult identityResult =
                        await userManager.AddToRoleAsync(savedUser, "client");
                    if (!identityResult.Succeeded)
                    {
                        AddErrorsFromResult(identityResult);
                    }
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
            (Student, Client) users = (new Student(), new Client());
            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateForm", users);
            }
            else
            {
                return View("CreateCommonForm", users);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id, string userType)
        {
            if (userType == "student")
            {
                Student student = context.Students
                    .Include(s => s.StudentUser)
                    .First(s => s.StudentId == id);
                string idAppUser = student.StudentUser.Id;
                context.Remove(student);
                context.SaveChanges();
                return await DeleteAppUser(idAppUser);
            }
            else
            {
                Client client = context.Clients
                    .Include(c => c.ClientUser)
                    .First(c => c.ClientId == id);
                string idAppUser = client.ClientUser.Id;
                context.Remove(client);
                context.SaveChanges();
                return await DeleteAppUser(idAppUser);
            }

            async Task<IActionResult> DeleteAppUser(string idAppUser)
            {
                AppUser user = await userManager.FindByIdAsync(idAppUser);
                if (user != null)
                {
                    IdentityResult result = await userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User Not Found");
                }
                (IEnumerable<AppUser>, IEnumerable<Student>, IEnumerable<Client>) tuple
                = (userManager.Users.ToArray(),
                context.Students.ToArray(),
                context.Clients.ToArray());
                return View(nameof(Index), tuple);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id, string userType)
        {
            AppUser user;
            if (userType == "student")
            {
                Student student = context.Students
                    .Include(s => s.StudentUser)
                    .First(s => s.StudentId == id);
                string idAppUser = student.StudentUser.Id;
                user = await userManager.FindByIdAsync(idAppUser);
                if (user != null)
                {
                    ViewBag.User = "Student";
                    Client client = new Client();
                    return View((user, student, client));
                }
                else
                {
                    (IEnumerable<AppUser>, IEnumerable<Student>, IEnumerable<Client>) tuple
                        = (userManager.Users.ToArray(),
                        context.Students.ToArray(),
                        context.Clients.ToArray());
                    return RedirectToAction(nameof(Index), tuple);
                }
            }
            else
            {
                Client client = context.Clients
                    .Include(c => c.ClientUser)
                    .First(c => c.ClientId == id);
                string idAppUser = client.ClientUser.Id;
                user = await userManager.FindByIdAsync(idAppUser);
                if (user != null)
                {
                    ViewBag.User = "Client";
                    Student student = new Student();
                    return View((user, student, client));
                }
                else
                {
                    (IEnumerable<AppUser>, IEnumerable<Student>, IEnumerable<Client>) tuple
                        = (userManager.Users.ToArray(),
                        context.Students.ToArray(),
                        context.Clients.ToArray());
                    return RedirectToAction(nameof(Index), tuple);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(long id, string userType, string email, string password)
        {
            AppUser user;
            if (userType == "student")
            {
                Student student = context.Students
                    .Include(s => s.StudentUser)
                    .First(s => s.StudentId == id);
                string idAppUser = student.StudentUser.Id;
                user = await userManager.FindByIdAsync(idAppUser);
                return await ValidateEmailPassword(user);
            }
            else
            {
                Client client = context.Clients
                    .Include(c => c.ClientUser)
                    .First(c => c.ClientId == id);
                string idAppUser = client.ClientUser.Id;
                user = await userManager.FindByIdAsync(idAppUser);
                return await ValidateEmailPassword(user);
            }

            async Task<IActionResult> ValidateEmailPassword(AppUser appUserForValidation)
            {
                if (appUserForValidation != null)
                {
                    appUserForValidation.Email = email;
                    IdentityResult validEmail = await userValidator.ValidateAsync(userManager, appUserForValidation);
                    if (!validEmail.Succeeded)
                    {
                        AddErrorsFromResult(validEmail);
                    }
                    IdentityResult validPass = null;
                    if (!string.IsNullOrEmpty(password))
                    {
                        validPass = await passwordValidator.ValidateAsync(userManager, appUserForValidation, password);
                        if (validPass.Succeeded)
                        {
                            appUserForValidation.PasswordHash = passwordHasher.HashPassword(appUserForValidation, password);
                        }
                        else
                        {
                            AddErrorsFromResult(validPass);
                        }
                    }
                    if ((validEmail.Succeeded && validPass == null) || 
                        (validEmail.Succeeded && password != string.Empty && validPass.Succeeded))
                    {
                        IdentityResult result = await userManager.UpdateAsync(appUserForValidation);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User Not Found");
                }
                (IEnumerable<AppUser>, IEnumerable<Student>, IEnumerable<Client>) tuple
                    = (userManager.Users.ToArray(),
                    context.Students.ToArray(),
                    context.Clients.ToArray());
                return RedirectToAction(nameof(Index), tuple);
            }
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
        }

        // ViewComponent's Methods
        public IViewComponentResult Invoke(string userName)
        {
            string name = userName;
            return new ViewViewComponentResult()
            {
                ViewData = new ViewDataDictionary<string>(ViewData, name)
            };
        }
    }
}
