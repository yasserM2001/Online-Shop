using Online_Shop.Models;
using Online_Shop.ViewModels;

namespace Online_Shop.Repository.Intefaces
{
    public interface IApplicationUserRepository
    {
        ApplicationUser GetById(string userId);
        ApplicationUser GetProductSeller(int productId);
        List<ApplicationUser> GetAll();
        List<ApplicationUser> GetAllRequestAvtivation();
        List<ApplicationUser> GetAllRequestToBeSeller();
        List<ApplicationUser> GetAllRequestChangePassword();
        void Add(ApplicationUser user);
        void Update(ApplicationUser user);
        void Delete(ApplicationUser user);
    }
}
