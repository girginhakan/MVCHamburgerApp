namespace MVCHamburgerApp.Data.Entities
{
    public class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public string PictureName { get; set; }
        public byte[] PictureFile { get; set; }
        public string Size { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
