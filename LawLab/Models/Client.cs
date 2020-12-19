using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LawLab.Models
{
    public class Client
    {
        public long ClientId { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public string Address { get; set; }
        //[MaxLength(100)]
        //public byte?[] Foto { get; set; }
    }
}
