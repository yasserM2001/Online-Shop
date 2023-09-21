using Online_Shop.Models;
using Online_Shop.ViewModels;

namespace Online_Shop.Services.Interfaces
{
    public interface IProductCartService
    {
        ProductCartViewModel MapToViewModel(ProductCart model);
        ProductCart MapToModel(ProductCartViewModel viewModel);
        List<ProductCartViewModel> GetCartProducts(int cartId);
        void Update(ProductCartViewModel viewModel);
        ProductCartViewModel GetById(int id);
        List<ProductCartViewModel> GetAll();
        void Add(ProductCartViewModel viewModel);
        void Delete(ProductCartViewModel viewModel);
    }
}
