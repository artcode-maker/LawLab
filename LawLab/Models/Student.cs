using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
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
        public string FirstName { get; set; }
        [Required]
        public string FamilyName { get; set; }
        [Required]
        [NotMapped]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }

        [Range(1, 5)]
        public decimal UniversityYear { get; set; }
        public string Summary { get; set; }
        public string UniversityName { get; set; }
        public string FacultyName { get; set; }
        //[MaxLength(100)]
        //public byte[] Foto { get; set; }
        [Range(0, 10)]
        public decimal? Rating { get; set; }
    }
}
