using System.ComponentModel.DataAnnotations;

namespace MVCHamburgerApp.Models
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [DataType(DataType.Password)]
        
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}
