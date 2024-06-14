using MVCHamburgerApp.Data.Enums;

namespace MVCHamburgerApp.Data.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MenuId { get; set; }
        public int Quantity { get; set; }
        public int? ExtraToppingId { get; set; }  // Ekstra malzeme opsiyonel
        public MenuSize MenuSize { get; set; }
        public Order Order { get; set; }
        public Menu Menu { get; set; }
        public ExtraTopping ExtraTopping { get; set; }
    }
}
