using Microsoft.EntityFrameworkCore;
using Online_Shop.Models;
using Online_Shop.Repository.Intefaces;

namespace Online_Shop.Repository.Classes
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<Product> _dbSet;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Product>();
        }

        public Product GetById(int id)
        {
            return _dbSet.FirstOrDefault(p => p.Id == id);
        }

        public List<Product> GetAll()
        {
            List<Product> products = _dbSet.ToList();
            foreach(Product p in products)
            {
                if (p.ExpiryDate.HasValue && p.ExpiryDate < DateTime.Now)
                {
                    p.IsActive = false;
                    Update(p);
                }
            }
            
            return products;
        }

        public void Add(Product product)
        {
            _dbSet.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _dbSet.Update(product);
            _context.SaveChanges();
        }

        public void Delete(Product product)
        {
            _dbSet.Remove(product);
            _context.SaveChanges();
        }

        public List<Product> GetCategoryProducts(int CategoryId)
        {
            return _dbSet.Where(p =>  p.CategoryId == CategoryId).ToList();
        }

        public List<Product> GetUserProducts(string userId)
        {
            return _dbSet.Where(p => p.SellerId == userId).ToList();
        }

        public List<Product> GetAllActive()
        {
            List<Product> products = GetAll();
            return products.Where(p => p.IsActive == true).ToList();
        }

    }
}
