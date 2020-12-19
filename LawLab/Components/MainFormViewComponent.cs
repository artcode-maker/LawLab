using LawLab.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LawLab.Components
{
    public class MainFormViewComponent : ViewComponent
    {
        private AppDbContext context;
        public MainFormViewComponent(AppDbContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public IViewComponentResult Invoke()
        {
            Student student = new Student();
            return View(student);
        }

        //[HttpPost]
        //public IViewComponentResult Invoke(Student student)
        //{
        //    return RedirectToAction("https://localhost:5001/");
        //}
    }
}
