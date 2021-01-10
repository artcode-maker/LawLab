using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace LawLab.Models
{
    public class Client
    {
        public long ClientId { get; set; }

        [Required]
        [DisplayName("Имя")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Фамилия")]
        public string FamilyName { get; set; }
        [Required]
        [NotMapped]
        [DisplayName("Пароль")]
        public string Password { get; set; }
        [Required]
        [DisplayName("Электронная почта")]
        public string Email { get; set; }
        [DisplayName("Адрес")]
        public string Address { get; set; }
        [DisplayName("Юридическая проблема")]
        public string Issue { get; set; }
        public AppUser ClientUser { get; set; }
    }
}
