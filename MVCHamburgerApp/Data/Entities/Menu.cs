using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCHamburgerApp.Data.Entities
{
    public class Menu
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0, 1000)]
        public decimal BasePrice { get; set; }

        public string PictureName { get; set; }

        [NotMapped]
        [BindNever]
        public IFormFile PictureFile { get; set; }

        [Required]
        public string Size { get; set; } //(enum mı olacak acaba müşteriye selectbox'tan seçtirmek için)

        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
