using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LawLab.Models
{
    public class Student
    {
        public long StudentId { get; set; }

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

        [Range(1, 5)]
        [DisplayName("Текущий курс обучения")]
        public decimal UniversityYear { get; set; }
        [DisplayName("Интересующее юридическая отрасль")]
        public string Summary { get; set; }
        [DisplayName("Название университета")]
        public string UniversityName { get; set; }
        [DisplayName("Название факультета")]
        public string FacultyName { get; set; }
        public List<Rating> Ratings { get; set; }
        [MaxLength(2097152)]
        public byte[] Avatar { get; set; }
        [DisplayName("Фото")]
        [NotMapped]
        public IFormFile Foto { get; set; }
        public string StudentUserId { get; set; }
        public AppUser StudentUser { get; set; }
    }
}
