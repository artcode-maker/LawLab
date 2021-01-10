using LawLab.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LawLab.Components
{
    public class BestStudentsViewComponent : ViewComponent
    {
        private AppDbContext context;
        public BestStudentsViewComponent(AppDbContext ctx)
        {
            context = ctx;
        }

        public IViewComponentResult Invoke()
        {
            return View(context.Students.OrderByDescending(s => s.Rating).ToList().Take(4));
        }
    }
}
