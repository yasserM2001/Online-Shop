using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Online_Shop.Models;
using Online_Shop.Repository.Intefaces;

namespace Online_Shop.Repository.Classes
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<ApplicationUser> _userDbSet;


        public ApplicationUserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _userDbSet = dbContext.Set<ApplicationUser>();
        }
        // Register ==> Add not used
        public void Add(ApplicationUser user)
        {
            _userDbSet.Add(user);
            _dbContext.SaveChanges();
        }

        public void Delete(ApplicationUser user)
        {
            user.IsActive = false;
            //_userDbSet.Remove(user);
            _dbContext.SaveChanges();
        }

        public List<ApplicationUser> GetAll()
        {
            return _userDbSet.ToList();
        }

        public List<ApplicationUser> GetAllRequestAvtivation()
        {
            return _userDbSet.Where(u => u.RequestActivation == true ).ToList();
        }

        public List<ApplicationUser> GetAllRequestChangePassword()
        {
            return _userDbSet.Where(u => u.RequestChangePaswword == true).ToList();
        }

        public List<ApplicationUser> GetAllRequestToBeSeller()
        {
            return _userDbSet.Where(u => u.RequestToBeSeller == true).ToList();
        }

        public ApplicationUser GetById(string userId)
        {
            return _userDbSet.FirstOrDefault(u => u.Id == userId);
        }

        public ApplicationUser GetProductSeller(int productId)
        {
            Product product = _dbContext.Products.FirstOrDefault(p => p.Id == productId);

            return GetById(product.SellerId);
        }

        public void Update(ApplicationUser user)
        {
            _userDbSet.Update(user);
            _dbContext.SaveChanges();
        }
    }
}
