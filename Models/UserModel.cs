using System.ComponentModel.DataAnnotations;

namespace DetailPage.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        [StringLength(30)]
        public string UserName { get; set; }
        [StringLength(30)]
        public string Password { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(30)]
        public string Role { get; set; }
    }
}