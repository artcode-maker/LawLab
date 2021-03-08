using System;
using System.ComponentModel.DataAnnotations;

namespace LawLab.Models
{
    public class Rating
    {
        public long RatingId { get; set; }
        [Range(0, 10)]
        public int Rate { get; set; }
        public long StudentId { get; set; }
        public Student Student { get; set; }
    }
}
