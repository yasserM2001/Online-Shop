using Online_Shop.Models;

namespace Online_Shop.ViewModels
{
    public class HomeViewModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public double CartTotalPrice { get; set; }
        public List<ProductViewModel> Products { get; set; }
    }
}
