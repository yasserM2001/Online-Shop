using Microsoft.AspNetCore.Identity;
using Online_Shop.Models;
using Online_Shop.Repository.Intefaces;
using Online_Shop.Services.Interfaces;
using Online_Shop.ViewModels;

namespace Online_Shop.Services.Classes
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;


        public ApplicationUserService(IApplicationUserRepository userRepository,
            UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public ApplicationUserViewModel GetById(string userId)
        {
            ApplicationUser user = _userRepository.GetById(userId);
            return MapToViewModel(user);
        }

        public List<ApplicationUserViewModel> GetAll()
        {
            List<ApplicationUser> users = _userRepository.GetAll();
            List<ApplicationUserViewModel> viewModels = new List<ApplicationUserViewModel>();
            foreach (ApplicationUser user in users)
            {
                viewModels.Add(MapToViewModel(user));
            }
            return viewModels;
        }

        public void Add(ApplicationUserViewModel userVM)
        {
            ApplicationUser user = MapToModel(userVM);
            _userRepository.Add(user);
        }
        public void Update(ApplicationUserViewModel userVM)
        {
            ApplicationUser oldUser = _userRepository.GetById(userVM.Id);
            oldUser.UserName = userVM.UserName;
            oldUser.Email = userVM.Email;
            oldUser.IsActive = userVM.IsActive;
            oldUser.RequestActivation = userVM.RequestActivation;
            oldUser.RequestToBeSeller = userVM.RequestToBeSeller;
            oldUser.RequestChangePaswword = userVM.RequestChangePaswword;
            _userRepository.Update(oldUser);
        }

        public void Delete(ApplicationUserViewModel userVM)
        {
            _userRepository.Delete(_userRepository.GetById(userVM.Id));
        }

        public ApplicationUserViewModel MapToViewModel(ApplicationUser model)
        {
            ApplicationUserViewModel viewModel = new ApplicationUserViewModel();
            viewModel.Id = model.Id;
            viewModel.UserName = model.UserName;
            viewModel.Email = model.Email;
            viewModel.Password = model.PasswordHash;
            viewModel.IsActive = model.IsActive;
            viewModel.RequestActivation = model.RequestActivation;
            viewModel.RequestToBeSeller = model.RequestToBeSeller;
            viewModel.RequestChangePaswword = model.RequestChangePaswword;
            // lists
            return viewModel;

        }

        public ApplicationUser MapToModel(ApplicationUserViewModel userVM)
        {
            ApplicationUser userModel = new ApplicationUser();
            userModel.UserName = userVM.UserName;
            userModel.Email = userVM.Email;
            //userModel.PasswordHash = userVM.Password;
            userModel.IsActive = userVM.IsActive;
            userModel.RequestToBeSeller = userVM.RequestToBeSeller;
            userModel.RequestActivation = userVM.RequestActivation;

            return userModel;
        }
        public async Task<List<ApplicationUserViewModel>> GetSellersAsync()
        {
            List<ApplicationUser> sellersModels =
                 (List<ApplicationUser>)await _userManager.GetUsersInRoleAsync(Roles.Seller);
            
            List<ApplicationUserViewModel> sellersVMs =
                sellersModels.Select(u => MapToViewModel(u)).ToList();

            return sellersVMs;
        }

        public ApplicationUserViewModel GetByName(string name)
        {
            List<ApplicationUserViewModel> allUsers = GetAll();
            return allUsers.FirstOrDefault(u => u.UserName == name);
        }

        public List<ApplicationUserViewModel> GetAllRequestAvtivation()
        {
            List<ApplicationUser> users = _userRepository.GetAllRequestAvtivation();
            List<ApplicationUserViewModel> viewModels = new List<ApplicationUserViewModel>();
            foreach (ApplicationUser user in users)
            {
                viewModels.Add(MapToViewModel(user));
            }
            return viewModels;
        }

        public List<ApplicationUserViewModel> GetAllRequestToBeSeller()
        {
            List<ApplicationUser> users = _userRepository.GetAllRequestToBeSeller();
            List<ApplicationUserViewModel> viewModels = new List<ApplicationUserViewModel>();
            foreach (ApplicationUser user in users)
            {
                viewModels.Add(MapToViewModel(user));
            }
            return viewModels;
        }

        public List<ApplicationUserViewModel> GetAllRequestChangePassword()
        {
            List<ApplicationUser> users = _userRepository.GetAllRequestChangePassword();
            List<ApplicationUserViewModel> viewModels = new List<ApplicationUserViewModel>();
            foreach (ApplicationUser user in users)
            {
                viewModels.Add(MapToViewModel(user));
            }
            return viewModels;
        }

        public ApplicationUserViewModel GetProductSeller(int productId)
        {
            ApplicationUser userModel = _userRepository.GetProductSeller(productId);
            return MapToViewModel(userModel);
        }
    }
}
