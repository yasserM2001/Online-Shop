namespace Online_Shop.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<ProductCart>? Products { get; set; }
        public int CartStatusId { get; set; }
        public CartStatus CartStatus { get; set; }
        public double CartTotalPrice { get; set; }
        public DateTime? PurchasingDate { get; set; }
    }
}
