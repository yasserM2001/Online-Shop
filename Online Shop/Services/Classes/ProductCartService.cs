using Online_Shop.Models;
using Online_Shop.Repository.Intefaces;
using Online_Shop.Services.Interfaces;
using Online_Shop.ViewModels;

namespace Online_Shop.Services.Classes
{
    public class ProductCartService : IProductCartService
    {
        private readonly IProductCartRepository _productCartRepository;
        public ProductCartService(IProductCartRepository productCartRepository)
        {
            _productCartRepository = productCartRepository;
        }

        public void Add(ProductCartViewModel viewModel)
        {
            _productCartRepository.Add(MapToModel(viewModel));
        }

        public void Delete(ProductCartViewModel viewModel)
        {
            ProductCart oldProductCart = _productCartRepository.GetById(viewModel.Id);
            _productCartRepository.Delete(oldProductCart);
        }

        public List<ProductCartViewModel> GetAll()
        {
            List<ProductCart> productCarts = _productCartRepository.GetAll();
            List<ProductCartViewModel> viewModels = new List<ProductCartViewModel>();
            foreach(var productCart in productCarts)
            {
                viewModels.Add(MapToViewModel(productCart));
            }
            return viewModels;
        }

        public ProductCartViewModel GetById(int id)
        {
            return MapToViewModel(_productCartRepository.GetById(id));
        }

        public List<ProductCartViewModel> GetCartProducts(int cartId)
        {
            List<ProductCart> products = _productCartRepository.GetCartProducts(cartId);
            List<ProductCartViewModel> viewModels = new List<ProductCartViewModel>();
            foreach (ProductCart product in products)
            {
                viewModels.Add(MapToViewModel(product));
            }

            return viewModels;
        }

        public ProductCart MapToModel(ProductCartViewModel viewModel)
        {
            ProductCart model = new ProductCart();
            model.ProductId = viewModel.ProductId;
            model.CartId = viewModel.CartId;
            model.AmountOfProduct = viewModel.AmountOfProduct;
            model.ProductPrice = viewModel.ProductPrice;
            model.ProductImage = viewModel.ProductImage;

            return model;
        }

        public ProductCartViewModel MapToViewModel(ProductCart model)
        {
            ProductCartViewModel viewModel = new ProductCartViewModel();
            viewModel.Id = model.Id;
            viewModel.ProductId = model.ProductId;
            viewModel.CartId = model.CartId;
            viewModel.ProductPrice = model.ProductPrice;
            viewModel.AmountOfProduct = model.AmountOfProduct;
            viewModel.ProductImage = model.ProductImage;

            return viewModel;
        }

        public void Update(ProductCartViewModel viewModel)
        {
            ProductCart oldProductCart = _productCartRepository.GetById(viewModel.Id);
            oldProductCart.ProductImage = viewModel.ProductImage;
            oldProductCart.ProductPrice = viewModel.ProductPrice;
            oldProductCart.AmountOfProduct = viewModel.AmountOfProduct;
            oldProductCart.CartId = viewModel.CartId;
            oldProductCart.ProductId = viewModel.ProductId;

            _productCartRepository.Update(oldProductCart);
        }

    }
}
