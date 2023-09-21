using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Shop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime? InsertionDate { get; set; }
        public DateTime? ExpiryDate { get; set; } // NULL if the product has not expiry date 
        public double Price { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        // Bigger than 0
        public int StockQuantity { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string SellerId { get; set; }
        public ApplicationUser Seller { get; set; }
        public List<ProductCart>? Carts { get; set; }
        
    }
}
