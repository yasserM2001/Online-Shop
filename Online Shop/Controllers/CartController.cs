using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using Online_Shop.Models;
using Online_Shop.Services.Interfaces;
using Online_Shop.ViewModels;

namespace Online_Shop.Controllers
{
    public class CartController : Controller
    {
        private ICartService _cartService;
        private IApplicationUserService _userService;
        private IProductCartService _productCartService;
        private IProductService _productService;
        public CartController(ICartService cartService,
            IApplicationUserService userService,
            IProductCartService productCartService,
            IProductService productService)
        {
            _cartService = cartService;
            _userService = userService;
            _productCartService = productCartService;
            _productService = productService;
        }

        [Authorize(Roles = Roles.Customer)]
        public IActionResult Index()
        {
            CartViewModel cart = _cartService.GetUserInProgressCart(_userService.GetByName(User.Identity.Name).Id);
            List<ProductCartViewModel> products = _productCartService.GetCartProducts(cart.Id);
            foreach (var product in products)
            {
                product.ProductName = _productService.GetById(product.ProductId).Name;
            }
            ViewBag.TotalPrice = cart.CartTotalPrice;
            ViewBag.CartId = cart.Id;
            return View(products);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Customer)]
        public IActionResult AddToCart(int productId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login","Account");
            }

            ApplicationUserViewModel user = _userService.GetByName(User.Identity.Name);
            CartViewModel cart = _cartService.GetUserInProgressCart(user.Id);

            // add to cart
            bool result = _cartService.AddProductToCart(_productService.GetById(productId), cart.Id);
            
            return PartialView(result);
        }

        [Authorize(Roles = Roles.Customer)]
        public IActionResult Decrease(int productCartId)
        {
            ProductCartViewModel productCart = _productCartService.GetById(productCartId);
            CartViewModel cart = _cartService.GetById(productCart.CartId);

            if (productCart.AmountOfProduct == 1)
            {
                _productCartService.Delete(productCart);
                return RedirectToAction("Index");
            }
            else
            {
                _cartService.RemoveProductFromCart(productCartId);
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = Roles.Customer)]
        public IActionResult Increase(int productId)
        {
            ProductViewModel product = _productService.GetById(productId);
            ApplicationUserViewModel user = _userService.GetByName(User.Identity.Name);
            CartViewModel cart = _cartService.GetUserInProgressCart(user.Id);

            _cartService.AddProductToCart(product, cart.Id);

            return RedirectToAction("Index");
            //return Json(new
            //{
            //    success = true,
            //    newPrice = _cartService.GetUserInProgressCart(user.Id).CartTotalPrice,
            //});
        }
        [Authorize(Roles = Roles.Customer)]
        public IActionResult Remove(int productCartId)
        {
            ProductCartViewModel productCart = _productCartService.GetById(productCartId);
            _productCartService.Delete(productCart);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = Roles.Customer)]
        public IActionResult Buy(int cartId)
        {
            CartViewModel cart = _cartService.GetById(cartId);
            if (cart.CartTotalPrice > 0)
            {
                cart.CartStatusId = Statuses.CompletedValue;
                cart.PurchasingDate = DateTime.Now;
                _cartService.Update(cart);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = Roles.Customer)]
        public IActionResult UpdateCartPrice()
        {
            ApplicationUserViewModel user = _userService.GetByName(User.Identity.Name);
            CartViewModel cart = _cartService.GetUserInProgressCart(user.Id);
            // JSON response with cart details
            return Json(new
            {
                success = true,
                newPrice = _cartService.GetUserInProgressCart(user.Id).CartTotalPrice,
            });
        }

        [Authorize(Roles = Roles.Customer)]
        public IActionResult CartHistory()
        {
            ApplicationUserViewModel user = _userService.GetByName(User.Identity.Name);
            List<CartViewModel> carts = _cartService.GetUserCarts(user.Id);
            CartViewModel inProgressCart = _cartService.GetUserInProgressCart(user.Id);
            return View(carts);
        }

        [Authorize(Roles = Roles.Customer)]
        public IActionResult Details(int id)
        {
            CartViewModel cart = _cartService.GetById(id);
            List<ProductCartViewModel> productCarts = _productCartService.GetCartProducts(cart.Id);
            foreach (var productCart in productCarts)
            {
                productCart.ProductName = _productService.GetById(productCart.ProductId).Name;
            }
            return View(productCarts);
        }
    }
}
