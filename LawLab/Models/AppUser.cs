using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LawLab.Models
{    
    public class AppUser : IdentityUser
    {
        //public int StudentId { get; set; }
        public Student Student { get; set; }
        //public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
