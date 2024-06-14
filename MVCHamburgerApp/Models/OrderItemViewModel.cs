using MVCHamburgerApp.Data.Enums;

namespace MVCHamburgerApp.Models
{
    public class OrderItemViewModel
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int Quantity { get; set; }
        public MenuSize Size { get; set; }
        public List<int> SelectedToppingIds { get; set; }
        public decimal ItemPrice { get; set; } 

    }
}
