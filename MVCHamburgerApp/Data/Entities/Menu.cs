using Microsoft.AspNetCore.Http;
using MVCHamburgerApp.Data.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCHamburgerApp.Data.Entities
{
    public class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public string? PictureName { get; set; }

        //[NotMapped]
        //public IFormFile? PictureFile { get; set; }

        [NotMapped]
        public MenuSize Size { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
