using LawLab.Models;
using Microsoft.AspNetCore.Mvc;

namespace LawLab.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        private AppDbContext context;
        public HeaderViewComponent(AppDbContext ctx)
        {
            context = ctx;
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
