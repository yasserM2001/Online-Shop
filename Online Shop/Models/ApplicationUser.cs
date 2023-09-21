using Microsoft.AspNetCore.Identity;

namespace Online_Shop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; }
        public bool RequestActivation { get; set; }
        public bool RequestToBeSeller { get; set; }
        public bool RequestChangePaswword { get; set; }
        public List<Product> Products { get; set; }
        public List<Cart> Carts { get; set; }
    }
}
