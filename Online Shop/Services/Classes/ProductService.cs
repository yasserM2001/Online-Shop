using Microsoft.EntityFrameworkCore;
using Online_Shop.Models;
using Online_Shop.Repository.Intefaces;
using Online_Shop.Services.Interfaces;
using Online_Shop.ViewModels;

namespace Online_Shop.Services.Classes
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductService(IProductRepository productRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public void Add(ProductViewModel product)
        {
            SaveProductImage(product);
            _productRepository.Add(MapToModel(product));
        }
        public void Delete(ProductViewModel product)
        {
            _productRepository.Delete(_productRepository.GetById(product.Id));
        }
        public List<ProductViewModel> GetAll()
        {
            List<Product> products = _productRepository.GetAll();
            List<ProductViewModel> viewModelsList = new List<ProductViewModel>();

            foreach (Product product in products)
            {
                viewModelsList.Add(MapToViewModel(product));
            }
            return viewModelsList;
        }
        public List<ProductViewModel> GetAllActive()
        {
            List<Product> products = _productRepository.GetAllActive();
            List<ProductViewModel> viewModels = new List<ProductViewModel>();
            foreach (Product product in products)
            {
                viewModels.Add(MapToViewModel(product));
            }
            return viewModels;
        }
        public ProductViewModel GetById(int id)
        {
            Product product = _productRepository.GetById(id);
            return MapToViewModel(product);
        }
        public List<ProductViewModel> GetCategoryProducts(int CategoryId)
        {
            List<Product> products = _productRepository.GetCategoryProducts(CategoryId);
            List<ProductViewModel> viewModels = new List<ProductViewModel>();
            foreach (Product product in products)
            {
                viewModels.Add(MapToViewModel(product));
            }
            return viewModels;
        }
        public List<ProductViewModel> GetUserProducts(string userId)
        {
            List<Product> products = _productRepository.GetUserProducts(userId);
            List<ProductViewModel> viewModels = new List<ProductViewModel>();
            foreach (Product product in products)
            {
                viewModels.Add(MapToViewModel(product));
            }
            return viewModels;
        }
        public Product MapToModel(ProductViewModel productVM)
        {
            Product productModel = new Product();

            productModel.Name = productVM.Name;
            productModel.Description = productVM.Description;
            productModel.Image = productVM.Image;
            productModel.CategoryId = productVM.CategoryId;
            productModel.SellerId = productVM.SellerId;
            productModel.IsActive = productVM.IsActive;
            productModel.StockQuantity = productVM.StockQuantity;
            productModel.Price = productVM.Price;
            productModel.ExpiryDate = productVM.ExpiryDate;

            return productModel;
        }
        public ProductViewModel MapToViewModel(Product model)
        {
            ProductViewModel productVM = new ProductViewModel();

            productVM.Id = model.Id;
            productVM.Name = model.Name;
            productVM.Description = model.Description;
            productVM.Image = model.Image;
            productVM.CategoryId = model.CategoryId;
            productVM.SellerId = model.SellerId;
            productVM.IsActive = model.IsActive;
            productVM.StockQuantity = model.StockQuantity;
            productVM.Price = model.Price;
            productVM.ExpiryDate = model.ExpiryDate;
            productVM.InsertionDate = model.InsertionDate;

            return productVM;
        }
        public void SaveProductImage(ProductViewModel productViewModel)
        {
            if (productViewModel.ImageFile != null && productViewModel.ImageFile.Length > 0)
            {
                var webRootPath = _webHostEnvironment.WebRootPath;
                var imagePath = Path.Combine(webRootPath, "images");

                if (!Directory.Exists(imagePath))
                {
                    Directory.CreateDirectory(imagePath);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + productViewModel.ImageFile.FileName;
                var filePath = Path.Combine(imagePath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    productViewModel.ImageFile.CopyTo(stream);
                }
                // Update the ProductViewModel with the image file name or path
                productViewModel.Image = uniqueFileName;
            }
        }
        public void Update(ProductViewModel product)
        {
            SaveProductImage(product);
            Product oldProduct = _productRepository.GetById(product.Id);
            oldProduct.Name = product.Name;
            oldProduct.Description = product.Description;
            oldProduct.Price = product.Price;
            oldProduct.Image = product.Image;
            oldProduct.ExpiryDate = product.ExpiryDate;
            oldProduct.IsActive = product.IsActive;
            oldProduct.StockQuantity = product.StockQuantity;
            oldProduct.CategoryId = product.CategoryId;
            oldProduct.SellerId = product.SellerId;

            _productRepository.Update(oldProduct);
        }
        public List<ProductViewModel> SearchProducts(string query, int categoryId)
        {
            List<Product> categoryProducts;
            if (categoryId == 0)
            {
                categoryProducts = _productRepository.GetAllActive();
            }
            else
            {
                categoryProducts = _productRepository.GetCategoryProducts(categoryId)
                                   .Where(p => p.IsActive).ToList();
            }
            List<Product> filteredProducts = categoryProducts
                .Where(p => p.Name.ToLower().Contains(query.ToLower()) || p.Description.ToLower().Contains(query.ToLower()))
                .ToList();

            // Map the filtered products to your ViewModel if necessary
            var filteredProductViewModels = new List<ProductViewModel>();
            foreach (var product in filteredProducts)
            {
                filteredProductViewModels.Add(MapToViewModel(product));
            }

            return filteredProductViewModels;
        }

        public List<ProductViewModel> NewArrivals()
        {
            List<ProductViewModel> products = GetAllActive();
            List<ProductViewModel> newArrivals = new List<ProductViewModel>();

            DateTime today = DateTime.Now;
            TimeSpan daysThreshold = TimeSpan.FromDays(1);

            foreach (var product in products)
            {
                if (product.InsertionDate.HasValue)
                {
                    TimeSpan timeSinceRelease = today - product.InsertionDate.Value;

                    if (timeSinceRelease <= daysThreshold)
                    {
                        newArrivals.Add(product);
                    }
                }
            }

            newArrivals = newArrivals.OrderByDescending(p => p.InsertionDate).ToList();
            return newArrivals;
        }
    }
}
