using System.ComponentModel.DataAnnotations;

namespace RRRRCoreMVC.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Dept { get; set; }
    }
}
