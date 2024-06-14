namespace MVCHamburgerApp.Models
{
    public class EditOrderViewModel
    {
        public int OrderId { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
