using Online_Shop.Models;

namespace Online_Shop.Repository.Intefaces
{
    public interface IProductCartRepository
    {
        ProductCart GetById(int id);
        List<ProductCart> GetAll();
        void Add(ProductCart entity);
        void Update(ProductCart entity);
        void Delete(ProductCart entity);
        List<ProductCart> GetCartProducts(int cartId);
    }
}
