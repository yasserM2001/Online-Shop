namespace Online_Shop.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime InsertionDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public List<Product>? Products { get; set; }
    }
}
