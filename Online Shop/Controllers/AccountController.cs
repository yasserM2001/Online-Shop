using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Online_Shop.Models;
using Online_Shop.ViewModels;
using System.Text.RegularExpressions;

namespace Online_Shop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet, AllowAnonymous]
        public IActionResult Login()
        {
            ViewBag.ShowInactiveUserModal = false;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel loginUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(loginUser.UserName);
                if (user != null)
                {
                    bool found = await _userManager.CheckPasswordAsync(user, loginUser.Password);
                    if (found)
                    {
                        if (user.IsActive)
                        {
                            // Signin Manager Creates Cookie
                            await _signInManager.SignInAsync(user, loginUser.RememberMe);
                            return RedirectToAction("Index", "Product");
                        }
                        else
                        {
                            //ViewBag.ShowInactiveUserModal = true;
                            return RedirectToAction("RequestActivation", "ApplicationUser", new { userId = user.Id });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "wrong password");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "username doesn't exists");
                }
            }
            return View(loginUser);
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Product");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registeredUser)
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
                    await _userManager.AddToRoleAsync(newUser, Roles.Customer);
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var errorItem in result.Errors)
                    {
                        ModelState.AddModelError("Register", errorItem.Description);
                    }
                }
            }
            return View(registeredUser);
        }
        public JsonResult ValidPassword(string password)
        {
            var lowercaseRegex = new Regex(@"[a-z]");
            var uppercaseRegex = new Regex(@"[A-Z]");
            var digitRegex = new Regex(@"[0-9]");
            var checkLength = password.Length;

            // Check if the password meets the criteria
            bool hasLowercase = lowercaseRegex.IsMatch(password);
            bool hasUppercase = uppercaseRegex.IsMatch(password);
            bool hasDigit = digitRegex.IsMatch(password);
            bool trueLength = checkLength >= 6;

            // Check if all criteria are met
            bool isValid = hasLowercase && hasUppercase && hasDigit && trueLength;

            return Json(isValid);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(LoginUserViewModel loginUser)
        {
            ModelState.Remove("Password");
            ModelState.Remove("RememberMe");

            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(loginUser.UserName);
                if (user != null)
                {
                    user.RequestChangePaswword = true;
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found");
                }

            }
            return View(loginUser);
        }
        [HttpGet]
        public IActionResult RequestToBeSeller()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RequestToBeSeller(LoginUserViewModel loginUser)
        {
            ModelState.Remove("Password");
            ModelState.Remove("RememberMe");

            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(loginUser.UserName);
                if (user != null)
                {
                    bool isSeller = await _userManager.IsInRoleAsync(user, Roles.Seller);
                    if (!isSeller)
                    {
                        bool isAdmin = await _userManager.IsInRoleAsync(user, Roles.Admin);
                        if (!isAdmin)
                        {
                            user.RequestToBeSeller = true;
                            await _userManager.UpdateAsync(user);
                            return RedirectToAction("Login", "Account");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "admin can't be a seller");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "User already a seller");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found");
                }
            }
            return View(loginUser);

        }
    }
}
