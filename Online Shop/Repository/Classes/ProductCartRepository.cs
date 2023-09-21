using Microsoft.EntityFrameworkCore;
using Online_Shop.Models;
using Online_Shop.Repository.Intefaces;

namespace Online_Shop.Repository.Classes
{
    public class ProductCartRepository : IProductCartRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<ProductCart> _dbSet;

        public ProductCartRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<ProductCart>();
        }

        public ProductCart GetById(int id)
        {
            ProductCart productCart = _dbSet.FirstOrDefault(pc => pc.Id == id);
            return productCart;
        }

        public List<ProductCart> GetAll()
        {
            return _dbSet.ToList();
        }

        public void Add(ProductCart entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Update(ProductCart entity)
        {
            _dbSet.Update(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(ProductCart entity)
        {
            Cart cart = _dbContext.Carts.FirstOrDefault(c => c.Id == entity.CartId);
            Product product = _dbContext.Products.FirstOrDefault(p => p.Id == entity.ProductId);
            if(product.StockQuantity == 0)
            {
                product.IsActive = true;
            }
            product.StockQuantity += entity.AmountOfProduct;
            cart.CartTotalPrice -= entity.ProductPrice * entity.AmountOfProduct;

            _dbContext.Products.Update(product);
            _dbContext.Carts.Update(cart);
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }

        public List<ProductCart>? GetCartProducts(int cartId)
        {
            return _dbSet.Where(p => p.CartId == cartId).ToList();
        }
    }
}
