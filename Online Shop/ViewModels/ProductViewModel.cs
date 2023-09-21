using Microsoft.AspNetCore.Mvc;
using Online_Shop.Models;
using System.ComponentModel.DataAnnotations;

namespace Online_Shop.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Image { get; set; }
        public DateTime? InsertionDate { get; set; }

        [Remote("FutureDate","Product",ErrorMessage = "choose a valid expiry date")]
        public DateTime? ExpiryDate { get; set; }  
        public double Price { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
        [Remote("NotZero","Product",ErrorMessage = "please, choose a category")]
        public int CategoryId { get; set; }
        public string? SellerId { get; set; }
        public string? SellerName { get; set; }
        [Required]
        public IFormFile ImageFile { get; set; }

        public List<CategoryViewModel>? Categories { get; set; }
    }
}
