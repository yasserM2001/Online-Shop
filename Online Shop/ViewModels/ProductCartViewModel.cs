using Online_Shop.Models;

namespace Online_Shop.ViewModels
{
    public class ProductCartViewModel
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int AmountOfProduct { get; set; }
        public double ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public string? ProductName { get; set; }
    }
}
