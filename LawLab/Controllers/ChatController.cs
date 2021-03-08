using System.Linq;
using System.Threading.Tasks;
using LawLab.Hubs;
using LawLab.Infrastructure;
using LawLab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> SendInvitation(long idUser, string role, [FromServices] IHubContext<ChatHub, IMessageClient> hubContext)
        {
            if (role == "client")
            {
                var student = context.Students.Find(idUser);
                var userId = student.StudentUserId;
                ISession session = HttpContext.Session;
                var user = session.Get<long>("client");
                await hubContext.Clients.User(userId).SendLink($"Chat?idClient={user}");
            }
            else if (role == "student")
            {
                var client = context.Clients.Find(idUser);
                var userId = client.ClientUserId;
                ISession session = HttpContext.Session;
                var user = session.Get<long>("student");
                await hubContext.Clients.User(userId).SendLink($"Chat?idStudent={user}");
            }
            return View("Index");
        }

        public async Task StopChat(long idStudent, IHubContext<ChatHub, IMessageClient> hubContext)
        {
            var student = context.Students.Find(idStudent);
            var userId = student.StudentUserId;
            await hubContext.Clients.User(userId).StopChat();
        }

        public async Task StopChatWithClient(long idClient, IHubContext<ChatHub, IMessageClient> hubContext)
        {
            var client = context.Clients.Find(idClient);
            var userId = client.ClientUserId;
            await hubContext.Clients.User(userId).StopChat();
        }

        [Authorize]
        [HttpPost]
        public IActionResult StopChatByStudent(long idClient, [FromServices] IHubContext<ChatHub, IMessageClient> hubContext)
        {
            StopChatWithClient(idClient, hubContext).Wait();
            return RedirectPermanent("/");
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Estimate(long idStudent, int rating, [FromServices] IHubContext<ChatHub, IMessageClient> hubContext)
        {
            Student student = context.Students
                    .Include(s => s.Ratings)
                    .Include(s => s.StudentUser)
                    .First(s => s.StudentId == idStudent);
            Rating newRating = new Rating { Rate = rating };
            student.Ratings.Add(newRating);
            context.Update(student);
            context.SaveChanges();
            StopChat(idStudent, hubContext).Wait();
            return RedirectPermanent("/");
        }
    }
}
