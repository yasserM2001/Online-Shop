using Online_Shop.Models;
using Online_Shop.ViewModels;

namespace Online_Shop.Services.Interfaces
{
    public interface IProductService
    {
        ProductViewModel GetById(int id);
        List<ProductViewModel> GetAll();
        List<ProductViewModel> GetAllActive();
        List<ProductViewModel> NewArrivals();
        List<ProductViewModel> SearchProducts(string query,int categoryId);
        void Add(ProductViewModel product);
        void Update(ProductViewModel product);
        void Delete(ProductViewModel product);
        ProductViewModel MapToViewModel(Product model);
        Product MapToModel(ProductViewModel productVM);
        void SaveProductImage(ProductViewModel productViewModel);
        public List<ProductViewModel> GetCategoryProducts(int CategoryId);
        List<ProductViewModel> GetUserProducts(string userId);
    }
}
