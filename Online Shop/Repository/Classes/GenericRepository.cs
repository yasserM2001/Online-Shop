using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Online_Shop.Repository.Intefaces;

namespace Online_Shop.Repository.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(IdentityDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public TEntity GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public List<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
    }
}
