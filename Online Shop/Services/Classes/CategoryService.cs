using Online_Shop.Models;
using Online_Shop.Repository.Intefaces;
using Online_Shop.Services.Interfaces;
using Online_Shop.ViewModels;

namespace Online_Shop.Services.Classes
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public void Add(CategoryViewModel category)
        {
            category.IsDeleted = false;
            _categoryRepository.Add(MapToModel(category));
        }

        public void Delete(CategoryViewModel category)
        {
            _categoryRepository.Delete(_categoryRepository.GetById(category.Id));
        }

        public List<CategoryViewModel> GetAll()
        {
            List<Category> categories = _categoryRepository.GetAll();
            List<CategoryViewModel> viewModelsList = new List<CategoryViewModel>();

            foreach (Category category in categories)
            {
                viewModelsList.Add(MapToViewModel(category));
            }
            return viewModelsList;
        }

        public List<CategoryViewModel> GetAllAvtive()
        {
            List<Category> categories = _categoryRepository.GetAll().Where(c=>c.IsDeleted == false).ToList();
            List<CategoryViewModel> viewModelsList = new List<CategoryViewModel>();

            foreach (Category category in categories)
            {
                viewModelsList.Add(MapToViewModel(category));
            }
            return viewModelsList;
        }

        public CategoryViewModel GetById(int id)
        {
            Category category = _categoryRepository.GetById(id);
            return MapToViewModel(category);
        }

        public Category MapToModel(CategoryViewModel categoryVM)
        {
            Category categoryModel = new Category();

            categoryModel.Name = categoryVM.Name;
            categoryModel.InsertionDate = categoryVM.InsertionDate;
            categoryModel.ModifiedDate = categoryVM.ModifiedDate;
            categoryModel.IsDeleted = categoryVM.IsDeleted;
            // product list
            return categoryModel;
        }

        public CategoryViewModel MapToViewModel(Category model)
        {
            CategoryViewModel categoryVM = new CategoryViewModel();

            categoryVM.Id = model.Id;
            categoryVM.Name = model.Name;
            categoryVM.InsertionDate = model.InsertionDate;
            categoryVM.ModifiedDate = model.ModifiedDate;
            categoryVM.IsDeleted = model.IsDeleted;
            // product list
            return categoryVM;
        }

        public void Update(CategoryViewModel category)
        {
            Category oldCategory = _categoryRepository.GetById(category.Id);
            oldCategory.ModifiedDate = DateTime.Now;
            oldCategory.Name = category.Name;
            oldCategory.IsDeleted = category.IsDeleted;
            _categoryRepository.Update(oldCategory);
        }
    }
}
