using Microsoft.AspNetCore.Mvc;
using Online_Shop.Models;
using System.ComponentModel.DataAnnotations;

namespace Online_Shop.ViewModels
{
    public class ApplicationUserViewModel
    {
        public string? Id { get; set; }
        [Remote("UniqueName", "ApplicationUser",
            AdditionalFields = nameof(Id),
            ErrorMessage = "This username is already taken")]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool RequestActivation { get; set; }
        public bool RequestToBeSeller { get; set; }
        public bool RequestChangePaswword { get; set; }

        // Lists
        public List<ProductViewModel>? Products { get; set; }
        //public List<Cart>? Carts { get; set; }

    }
}
