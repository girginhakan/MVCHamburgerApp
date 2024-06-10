using System.ComponentModel.DataAnnotations;

namespace MVCHamburgerApp.Models
{
    public class UserViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
