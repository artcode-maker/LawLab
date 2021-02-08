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
            List<Student> students = context.Students.ToList();
            var studentsToCalulate = students.Select(s => new { id = s.StudentId, rating = s.Ratings?.Select(r => r.Rate).Average() });
            var bestStudents = studentsToCalulate.OrderByDescending(s => s.rating).Take(4).Select(s => s.id);
            return View(students.Where(s => bestStudents.Contains(s.StudentId)));
        }
    }
}
