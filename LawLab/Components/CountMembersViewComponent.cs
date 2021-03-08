using LawLab.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LawLab.Components
{
    public class CountMembersViewComponent : ViewComponent
    {
        private AppDbContext context;
        public CountMembersViewComponent(AppDbContext ctx)
        {
            context = ctx;
        }

        public IViewComponentResult Invoke()
        {
            return View(context.Students.Count());
        }
    }
}
