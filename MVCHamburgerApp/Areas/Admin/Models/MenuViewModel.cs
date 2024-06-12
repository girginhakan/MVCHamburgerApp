using Microsoft.AspNetCore.Http;
using MVCHamburgerApp.Data.Enums;

namespace MVCHamburgerApp.Areas.Admin.Models
{
    public class MenuViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public IFormFile? Picture { get; set; }
        public MenuSize Size { get; set; }
    }
}
