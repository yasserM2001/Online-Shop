using Online_Shop.Models;

namespace Online_Shop.Repository.Intefaces
{
    public interface IProductRepository
    {
        Product GetById(int id);
        List<Product> GetAll();
        List<Product> GetAllActive();
        void Add(Product product);
        void Update(Product product);
        void Delete(Product product);
        List<Product> GetCategoryProducts(int CategoryId);
        List<Product> GetUserProducts(string userId);
    }
}
