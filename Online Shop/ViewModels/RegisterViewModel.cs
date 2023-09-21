using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Online_Shop.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Remote("UniqueName", "ApplicationUser"
            , ErrorMessage = "This username is already taken")]
        public string UserName { get; set; }
        [Required, DataType(DataType.Password)]
        [Remote("ValidPassword","Account",
            ErrorMessage = "Password must contain at least one uppercase letter," +
            " one lowercase letter, and one number, and length more than 5.")]
        public string Password { get; set; }

        [Required,DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage = "confirm password should match password")]
        public string ConfirmPassword { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
