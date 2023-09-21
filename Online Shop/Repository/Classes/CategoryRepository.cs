using Microsoft.EntityFrameworkCore;
using Online_Shop.Models;
using Online_Shop.Repository.Intefaces;

namespace Online_Shop.Repository.Classes
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Category> _dbSet;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<Category>();
        }

        public Category GetById(int id)
        {
            return _dbSet.FirstOrDefault(c => c.Id == id);
        }

        public List<Category> GetAll()
        {
            return _dbSet.ToList();
        }

        public void Add(Category category)
        {
            _dbSet.Add(category);
            _dbContext.SaveChanges();
        }

        public void Update(Category category)
        {
            _dbSet.Update(category);
            _dbContext.SaveChanges();
        }

        public void Delete(Category category)
        {
            //_dbSet.Remove(category);
            category.IsDeleted = true;
            _dbContext.SaveChanges();
        }

    }
}
