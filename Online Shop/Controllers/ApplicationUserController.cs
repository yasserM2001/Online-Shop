using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_Shop.Models;
using Online_Shop.Services.Interfaces;
using Online_Shop.ViewModels;

namespace Online_Shop.Controllers
{
    public class ApplicationUserController : Controller
    {
        private readonly IApplicationUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartService _cartService;
        private readonly IProductCartService _productCartService;

        public ApplicationUserController(IApplicationUserService userService,
            UserManager<ApplicationUser> userManager,
            ICartService cartService,
            IProductCartService productCartService)
        {
            _userService = userService;
            _userManager = userManager;
            _cartService = cartService;
            _productCartService = productCartService;
        }

        [Authorize(Roles = Roles.Admin)]
        public IActionResult Index()
        {
            List<ApplicationUserViewModel> users = _userService.GetAll();
            ViewBag.Sellers = false;
            return View(users);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Sellers()
        {
            List<ApplicationUserViewModel> sellers = await _userService.GetSellersAsync();
            ViewBag.Sellers = true;
            return View("Index", sellers);
        }

        [HttpGet, Authorize(Roles = Roles.Admin)]
        public IActionResult New()
        {
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> New(RegisterViewModel registeredUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser newUser = new ApplicationUser();
                newUser.Email = registeredUser.Email;
                newUser.UserName = registeredUser.UserName;
                newUser.IsActive = true;

                IdentityResult result = await _userManager.CreateAsync(newUser, registeredUser.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, Roles.Seller);
                    return RedirectToAction("Sellers");
                }
                else
                {
                    foreach (var errorItem in result.Errors)
                    {
                        ModelState.AddModelError("Register", errorItem.Description);
                    }
                }
            }
            return PartialView();
        }

        public JsonResult UniqueName(string userName, string id)
        {
            List<ApplicationUserViewModel> users = _userService.GetAll();
            ApplicationUserViewModel? sameNameUser = users.FirstOrDefault(u => (u.UserName == userName));
            if (sameNameUser == null || sameNameUser.Id == id)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Edit(string userId)
        {
            ApplicationUserViewModel user = _userService.GetById(userId);
            return PartialView(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken, Authorize(Roles = Roles.Admin)]
        public IActionResult Edit(ApplicationUserViewModel user)
        {
            if (ModelState.IsValid)
            {
                _userService.Update(user);
                return RedirectToAction("Index");
            }
            return PartialView();
        }

        [Authorize(Roles = Roles.Admin), HttpGet]
        public IActionResult Activation(string userId)
        {
            ApplicationUserViewModel user = _userService.GetById(userId);
            return PartialView(user);
        }

        [Authorize(Roles = Roles.Admin), HttpPost]
        public IActionResult Activation(ApplicationUserViewModel user)
        {
            user.IsActive = !user.IsActive;
            if(user.RequestActivation == true)
            {
                user.RequestActivation = false;
            }
            _userService.Update(user);
            return RedirectToAction("Sellers");
        }

        [Authorize(Roles=Roles.Admin), HttpGet]
        public IActionResult ChangePassword(string userId,string? returnViewName)
        {
            ApplicationUserViewModel user = _userService.GetById(userId);
            RegisterViewModel registerViewModel = new RegisterViewModel();
            registerViewModel.UserName = user.UserName;
            registerViewModel.Email = user.Email;
            registerViewModel.IsActive = user.IsActive;

            ViewBag.returnView = returnViewName;

            return PartialView(registerViewModel);
        }

        [Authorize(Roles=Roles.Admin), HttpPost]
        public async Task<IActionResult> ChangePassword(RegisterViewModel newPasswordUser, string? returnViewName)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(newPasswordUser.UserName);
                if (user != null)
                {
                    // Use a method to change the user's password without requiring the old password
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, newPasswordUser.Password);

                    if (result.Succeeded)
                    {
                        // Password changed successfully, you can redirect or display a success message
                        if (returnViewName == null)
                        {
                            if(user.RequestChangePaswword == true)
                            {
                                user.RequestChangePaswword = false;
                                await _userManager.UpdateAsync(user);
                            }
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            user.RequestChangePaswword = false;
                            await _userManager.UpdateAsync(user);
                            return RedirectToAction("Notification");
                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            // If the model state is not valid or password change failed, return to the form
            return PartialView(newPasswordUser);
        }

        [HttpGet]
        public IActionResult RequestActivation(string userId)
        {
            ApplicationUserViewModel user = _userService.GetById(userId);
            return View(user);
        }
        
        [HttpPost]
        public IActionResult RequestActivation(ApplicationUserViewModel userViewModel)
        {
            userViewModel.RequestActivation = true;
            _userService.Update(userViewModel);
            return RedirectToAction("Login", "Account");
        }

        [Authorize(Roles = Roles.Admin)]
        public IActionResult Notification()
        {
            List<ApplicationUserViewModel> users = new List<ApplicationUserViewModel>();

            List<ApplicationUserViewModel> requestActivation = _userService.GetAllRequestAvtivation();
            List<ApplicationUserViewModel> requestBeSeller = _userService.GetAllRequestToBeSeller();
            List<ApplicationUserViewModel> requestChangePassword = _userService.GetAllRequestChangePassword();

            foreach (ApplicationUserViewModel user in requestActivation)
            {
                users.Add(new ApplicationUserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    RequestActivation = user.RequestActivation,
                });
            }
            foreach (ApplicationUserViewModel user in requestBeSeller)
            {
                users.Add(new ApplicationUserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    RequestToBeSeller = user.RequestToBeSeller,
                });
            }
            foreach (ApplicationUserViewModel user in requestChangePassword)
            {
                users.Add(new ApplicationUserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    RequestChangePaswword = user.RequestChangePaswword,
                });
            }

            //users.AddRange(_userService.GetAllRequestAvtivation());
            //users.AddRange(_userService.GetAllRequestToBeSeller());
            //users.AddRange(_userService.GetAllRequestChangePassword());

            return View(users);
        }

        [Authorize(Roles = Roles.Admin)]
        public IActionResult NotificationActivationReply(string userId,bool accept)
        {
            ApplicationUserViewModel user = _userService.GetById(userId);
            user.RequestActivation = false;

            if (accept)
            {
                user.IsActive = true;
            }
            _userService.Update(user);
            return RedirectToAction("Notification");
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> NotificationBeSellerReply(string userId, bool accept)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            user.RequestToBeSeller = false;

            if (accept)
            {
                // Add the user to the "Seller"
                await _userManager.AddToRoleAsync(user, Roles.Seller);

                // Remove the "Customer" 
                await _userManager.RemoveFromRoleAsync(user, Roles.Customer);

                CartViewModel cart = _cartService.GetUserInProgressCart(userId);
                List<ProductCartViewModel> productCarts = _productCartService.GetCartProducts(cart.Id);
                foreach(var product in productCarts)
                {
                    _cartService.RemoveProductFromCart(product.Id);
                }
            }
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Notification");
        }
    }
}
