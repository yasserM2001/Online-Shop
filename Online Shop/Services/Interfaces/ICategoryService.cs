using Online_Shop.Models;
using Online_Shop.ViewModels;

namespace Online_Shop.Services.Interfaces
{
    public interface ICategoryService
    {
        CategoryViewModel GetById(int id);
        List<CategoryViewModel> GetAll();
        List<CategoryViewModel> GetAllAvtive();
        void Add(CategoryViewModel category);
        void Update(CategoryViewModel category);
        void Delete(CategoryViewModel category);
        CategoryViewModel MapToViewModel(Category model);
        Category MapToModel(CategoryViewModel categoryVM);
    }
}
