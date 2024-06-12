using System.ComponentModel.DataAnnotations;

namespace MVCHamburgerApp.Models
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Lütfen en az bir menü öğesi seçin.")]
        public List<OrderItemViewModel> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
