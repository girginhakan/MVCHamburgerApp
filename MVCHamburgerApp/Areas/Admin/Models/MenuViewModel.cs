using Microsoft.AspNetCore.Http;

namespace MVCHamburgerApp.Areas.Admin.Models
{
    public class MenuViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public IFormFile? Picture { get; set; }
        public string Size { get; set; }
    }
}
