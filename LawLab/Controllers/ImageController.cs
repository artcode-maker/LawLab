using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using LawLab.Models;

namespace LawLab.Controllers
{
    public class ImageController : Controller
    {
        private AppDbContext context;
        IWebHostEnvironment _env;

        public ImageController(AppDbContext ctx, IWebHostEnvironment env)
        {
            context = ctx;
            _env = env;
        }

        public FileResult GetAvatar(long id)
        {
            var user = context.Students.Find(id);
            if (user?.Avatar != null)
            {
                return File(user.Avatar, "image/jpg");
            }
            else
            {
                var avatarPath = "images/anonymous.png";
                return File(_env.WebRootFileProvider.GetFileInfo(avatarPath).CreateReadStream(), "image/png");
            }
        }
    }
}
