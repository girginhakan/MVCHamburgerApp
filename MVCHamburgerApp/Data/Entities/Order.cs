namespace MVCHamburgerApp.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }

        public AppUser AppUser { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
