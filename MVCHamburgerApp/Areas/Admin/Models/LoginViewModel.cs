using System.ComponentModel.DataAnnotations;

namespace MVCHamburgerApp.Areas.Admin.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
