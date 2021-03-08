using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

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
        [MaxLength(2097152)]
        public byte[] Avatar { get; set; }
        [DisplayName("Фото")]
        [NotMapped]
        public IFormFile Foto { get; set; }
        public string ClientUserId { get; set; }
        public AppUser ClientUser { get; set; }
    }
}
