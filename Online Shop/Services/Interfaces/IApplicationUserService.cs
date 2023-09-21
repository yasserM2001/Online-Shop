using Microsoft.AspNetCore.Identity;
using Online_Shop.Models;
using Online_Shop.ViewModels;

namespace Online_Shop.Services.Interfaces
{
    public interface IApplicationUserService
    {
        ApplicationUserViewModel GetById(string userId);
        ApplicationUserViewModel GetByName(string name);
        ApplicationUserViewModel GetProductSeller(int productId);
        List<ApplicationUserViewModel> GetAll();
        List<ApplicationUserViewModel> GetAllRequestAvtivation();
        List<ApplicationUserViewModel> GetAllRequestToBeSeller();
        List<ApplicationUserViewModel> GetAllRequestChangePassword();
        void Add(ApplicationUserViewModel user);
        void Update(ApplicationUserViewModel user);
        void Delete(ApplicationUserViewModel user);
        ApplicationUserViewModel MapToViewModel(ApplicationUser model);
        ApplicationUser MapToModel(ApplicationUserViewModel userVM);
        public Task<List<ApplicationUserViewModel>> GetSellersAsync();
    }
}
