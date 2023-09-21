using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Shop.Models;
using Online_Shop.Services.Classes;
using Online_Shop.Services.Interfaces;
using Online_Shop.ViewModels;

namespace Online_Shop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public CategoryController(ICategoryService categoryService,IProductService productService)
        {
            this._categoryService = categoryService;
            this._productService = productService;
        }
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Index()
        {
            List<CategoryViewModel> categories = _categoryService.GetAll();
            return View(categories);
        }

        [HttpGet,Authorize(Roles = Roles.Admin)]
        public IActionResult New()
        {
            return PartialView();
        }
        [HttpPost, Authorize(Roles = Roles.Admin)]
        public IActionResult New(CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                category.IsDeleted = false;
                _categoryService.Add(category);
                return RedirectToAction("Index");
            }
            return PartialView();
        }

        [HttpGet,Authorize(Roles = Roles.Admin)]
        public IActionResult Edit(int categoryId)
        {
            CategoryViewModel category = _categoryService.GetById(categoryId);
            return PartialView(category);
        }

        [HttpPost, Authorize(Roles = Roles.Admin)]
        public IActionResult Edit(CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                _categoryService.Update(category);
                return RedirectToAction("Index");
            }
            return PartialView();
        }
        public JsonResult UniqueName(string name, int id)
        {
            List<CategoryViewModel> users = _categoryService.GetAll();
            CategoryViewModel? sameCategory = users.FirstOrDefault(c => c.Name == name);
            if (sameCategory == null || sameCategory.Id == id)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        [HttpGet, Authorize(Roles = Roles.Admin)]
        public IActionResult Activation(int categoryId)
        {
            CategoryViewModel category = _categoryService.GetById(categoryId);
            return PartialView(category);
        }
        
        [HttpPost, Authorize(Roles = Roles.Admin)]
        public IActionResult Activation(CategoryViewModel category)
        {
            category.IsDeleted = !category.IsDeleted;
            _categoryService.Update(category);

            return RedirectToAction("Index");
        }
    }
}
