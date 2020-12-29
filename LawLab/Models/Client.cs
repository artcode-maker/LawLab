using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace LawLab.Models
{
    public class Client
    {
        public long ClientId { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string FamilyName { get; set; }
        [Required]
        [NotMapped]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public string Address { get; set; }
        //[MaxLength(100)]
        //public byte?[] Foto { get; set; }
    }
}
