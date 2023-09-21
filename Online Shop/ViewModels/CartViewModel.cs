using Online_Shop.Models;
using System.ComponentModel.DataAnnotations;

namespace Online_Shop.ViewModels
{
    public class CartViewModel
    {
        public int Id { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        public int CartStatusId { get; set; }
        public double CartTotalPrice { get; set; }
        public List<ProductCartViewModel> Products { get; set; }
        public DateTime? PurchasingDate { get; set; }

    }
}
