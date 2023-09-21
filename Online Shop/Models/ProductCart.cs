namespace Online_Shop.Models
{
    public class ProductCart
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int AmountOfProduct { get; set; }
        public DateTime InsertionDate { get; set; }
        public double ProductPrice { get; set; }
        public string ProductImage { get; set; }
    }

}
