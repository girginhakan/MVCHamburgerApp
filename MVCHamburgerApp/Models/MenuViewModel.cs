using MVCHamburgerApp.Data.Entities;
using System.Collections.Generic;

namespace MVCHamburgerApp.Models
{
    public class MenuViewModel
    {
        public List<Menu> Menus { get; set; }
        public List<ExtraTopping> ExtraToppings { get; set; }
    }
}
