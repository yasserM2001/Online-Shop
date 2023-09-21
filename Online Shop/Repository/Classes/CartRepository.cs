using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Online_Shop.Models;
using Online_Shop.Repository.Intefaces;
using Online_Shop.ViewModels;

namespace Online_Shop.Repository.Classes
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Cart> _dbSet;
        public CartRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<Cart>();
        }
        public Cart GetById(int id)
        {
            return _dbSet.FirstOrDefault(c => c.Id == id);
        }
        public List<Cart> GetAll()
        {
            return _dbSet.ToList();
        }
        public void Add(Cart cart)
        {
            _dbSet.Add(cart);
            _dbContext.SaveChanges();
        }
        public void Update(Cart cart)
        {
            _dbSet.Update(cart);
            _dbContext.SaveChanges();
        }
        // check it does not have any product
        public void Delete(Cart cart)
        {
            if (!_dbContext.ProductCarts.Where(p => p.CartId == cart.Id).Any())
            {
                _dbSet.Remove(cart);
                _dbContext.SaveChanges();
            }
        }
        public List<Cart> GetUserCarts(string userId)
        {
            return _dbSet.Where(c => c.ApplicationUserId == userId).ToList();
        }
        public Cart GetUserInProgressCart(string userId)
        {
            List<Cart> userCarts = GetUserCarts(userId);
            Cart inProgressCart = userCarts.FirstOrDefault(c => c.CartStatusId == 1);

            if (inProgressCart != null)
            {
                return inProgressCart;
            }
            else
            {
                Cart cart = new Cart
                {
                    ApplicationUserId = userId,
                    CartStatusId = 1,
                    CartTotalPrice = 0
                };
                Add(cart);

                return cart;
            }
        }
        public bool AddProductToCart(ProductViewModel product, int cartId)
        {
            if(product.StockQuantity == 0 || product.IsActive == false)
            {
                return false;
            }
            // Real product to decrese stock Quantity
            Product productModel = _dbContext.Products.FirstOrDefault(p=>p.Id==product.Id);
            // Cart of User
            Cart cart = GetById(cartId);
            // products of user in cart
            List<ProductCart> cartProducts =
                _dbContext.ProductCarts.Where(p => p.CartId == cartId).ToList();
            // if the user already have this product in the current cart
            ProductCart? oldProduct = cartProducts.FirstOrDefault(p => p.ProductId == product.Id);

            if (oldProduct == null)
            {
                ProductCart newProduct = new ProductCart
                {
                    ProductId = product.Id,
                    CartId = cartId,
                    AmountOfProduct = 1,
                    ProductPrice = product.Price,
                    ProductImage = product.Image
                };
                _dbContext.ProductCarts.Add(newProduct);
            }
            else
            {
                oldProduct.AmountOfProduct++;
                _dbContext.ProductCarts.Update(oldProduct);
            }
            // decrease product stock Quantity
            productModel.StockQuantity--;
            if(productModel.StockQuantity <= 0)
            {
                productModel.IsActive = false;
            }
            cart.CartTotalPrice += product.Price;

            _dbSet.Update(cart);
            _dbContext.Products.Update(productModel);
            _dbContext.SaveChanges();

            return true;
        }
        public void RemoveProductFromCart(int productCartId)
        {
            ProductCart productCart = _dbContext.ProductCarts.FirstOrDefault(p => p.Id == productCartId);
            Cart cart = _dbContext.Carts.FirstOrDefault(c => c.Id == productCart.CartId);

            Product product = _dbContext.Products.FirstOrDefault(p => p.Id == productCart.ProductId);
            if(product.StockQuantity == 0)
            {
                product.IsActive = true;
            }
            product.StockQuantity++;
            productCart.AmountOfProduct--;
            if(productCart.AmountOfProduct <= 0)  // just in case 
            {
                _dbContext.ProductCarts.Remove(productCart);
            }
            cart.CartTotalPrice -= productCart.ProductPrice;

            _dbContext.ProductCarts.Update(productCart);
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();
        }
    }
}
