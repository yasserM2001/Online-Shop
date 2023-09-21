using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Shop.Models;
using Online_Shop.Services.Interfaces;
using Online_Shop.ViewModels;

namespace Online_Shop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IApplicationUserService _appUserService;
        private readonly ICartService _cartService;

        public ProductController(IProductService productService,
            ICategoryService categoryService,
            IApplicationUserService applicationUserService,
            ICartService cartService)
        {
            this._productService = productService;
            this._categoryService = categoryService;
            this._appUserService = applicationUserService;
            this._cartService = cartService;
        }
        public IActionResult Index(int categoryId , bool newArrivals)
        {
            List<ProductViewModel> products;
            if (!newArrivals)
            {
                if (categoryId == 0)
                {
                    products = _productService.GetAllActive();
                    //  category IDs to be removed
                    List<int> deletedCategoryIds = new List<int>();

                    foreach (ProductViewModel product in products)
                    {
                        CategoryViewModel productCategory = _categoryService.GetById(product.CategoryId);

                        if (productCategory.IsDeleted)
                        {
                            deletedCategoryIds.Add(productCategory.Id);
                        }
                    }
                    products.RemoveAll(product => deletedCategoryIds.Contains(product.CategoryId));
                }
                else
                {
                    products = _productService.GetCategoryProducts(categoryId);
                }
            }
            else
            {
                products = _productService.NewArrivals();
            }
            ViewBag.Categories = _categoryService.GetAllAvtive();
            ViewBag.CategoryId = categoryId;
            //if (User.Identity.IsAuthenticated)
            if (User.IsInRole(Roles.Customer))
            {
                ApplicationUserViewModel user = _appUserService.GetByName(User.Identity.Name);
                ViewBag.CartTotalPrice = _cartService.GetUserInProgressCart(user.Id).CartTotalPrice;
            }
            if (User.IsInRole(Roles.Admin))
            {
                List<ApplicationUserViewModel> requestedActivation = _appUserService.GetAllRequestAvtivation();
                List<ApplicationUserViewModel> requestedBeSeller = _appUserService.GetAllRequestToBeSeller();
                List<ApplicationUserViewModel> requestedChangePassword = _appUserService.GetAllRequestChangePassword();
                ViewBag.NotificationCount = requestedActivation.Count + requestedBeSeller.Count + requestedChangePassword.Count;
            }
            return View(products);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Seller)]
        public IActionResult New()
        {
            ProductViewModel product = new ProductViewModel();
            product.Categories = _categoryService.GetAll();
            product.SellerId = _appUserService.GetByName(User.Identity.Name).Id;
            return PartialView(product);
        }
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Seller)]
        public IActionResult New(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                _productService.Add(product);
                return RedirectToAction("SellerProducts");
            }
            product.Categories = _categoryService.GetAll();
            return PartialView();
        }

        public JsonResult FutureDate(DateTime expiryDate)
        {
            if (expiryDate == null)
            {
                return Json(true);
            }
            else if (expiryDate > DateTime.Now)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        public IActionResult Details(int id, int categoryId)
        {
            ProductViewModel product = _productService.GetById(id);
            ViewBag.CategoryId = categoryId;
            if (User.IsInRole(Roles.Customer))
            {
                ApplicationUserViewModel user = _appUserService.GetByName(User.Identity.Name);
                ViewBag.CartTotalPrice = _cartService.GetUserInProgressCart(user.Id).CartTotalPrice;
            }
            return View(product);
        }

        [HttpGet, Authorize(Roles = Roles.Seller)]
        public IActionResult SellerProducts()
        {
            ApplicationUserViewModel user = _appUserService.GetByName(User.Identity.Name);
            List<ProductViewModel> products = _productService.GetUserProducts(user.Id);

            return View(products);
        }

        [HttpGet, Authorize(Roles = Roles.Seller)]
        public IActionResult Edit(int id)
        {
            ProductViewModel product = _productService.GetById(id);
            product.Categories = _categoryService.GetAll();
            return PartialView(product);
        }

        [HttpPost, Authorize(Roles = Roles.Seller)]
        public IActionResult Edit(ProductViewModel product)
        {
            ModelState.Remove("ImageFile");

            if (ModelState.IsValid)
            {
                _productService.Update(product);
                return RedirectToAction("SellerProducts");
            }
            product.Categories = _categoryService.GetAll();
            return PartialView(product);
        }

        [Authorize(Roles = Roles.Seller)]
        public IActionResult Activation(int id)
        {
            ProductViewModel product = _productService.GetById(id);
            product.IsActive = !product.IsActive;
            _productService.Update(product);

            return RedirectToAction("SellerProducts");
        }

        public IActionResult Search(string query,int categoryId)
        {
            if(query == null)
            {
                return PartialView("_ProductCardPartial", _productService.GetAllActive());
            }
            // Query your database or data source to filter products based on the search query
            var filteredProducts = _productService.SearchProducts(query,categoryId);

            // Return a partial view or JSON result with the filtered products
            return PartialView("_ProductCardPartial", filteredProducts);
        }

        [HttpPost]
        public IActionResult UpdateProducts()
        {
            List<ProductViewModel> products = _productService.GetAllActive();
            return PartialView("_ProductCardPartial", products);
        }

        [Authorize(Roles = Roles.Admin)]
        public IActionResult CategoryProducts(int categoryId)
        {
            List<ProductViewModel> products = _productService.GetCategoryProducts(categoryId);
            foreach (ProductViewModel product in products)
            {
                product.SellerName = _appUserService.GetProductSeller(product.Id).UserName;
            }
            return View(products);
        }

        public JsonResult NotZero(int categoryId)
        {
            if(categoryId == 0)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }
    }
}
