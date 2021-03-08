using Microsoft.AspNetCore.Identity;

namespace LawLab.Models
{    
    public class AppUser : IdentityUser
    {
        public Student Student { get; set; }
        public Client Client { get; set; }
    }
}
