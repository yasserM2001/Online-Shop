using Microsoft.AspNetCore.Mvc;
using Online_Shop.Models;
using System.ComponentModel.DataAnnotations;

namespace Online_Shop.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [Required]
        [Remote("UniqueName", "Category",
            AdditionalFields = nameof(Id),
            ErrorMessage = "This category name already exists")]
        public string Name { get; set; }
        public DateTime InsertionDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public List<ProductViewModel>? Products { get; set; }
    }
}
