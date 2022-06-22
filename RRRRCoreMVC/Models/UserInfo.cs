using System.ComponentModel.DataAnnotations;

namespace RRRRCoreMVC.Models
{
    public class UserInfo
    {
      
        public int ID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
