namespace MVCHamburgerApp.Models
{
    public class OrderItemViewModel
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public List<int> SelectedToppingIds { get; set; }
        public decimal ItemPrice { get; set; } 

    }
}
