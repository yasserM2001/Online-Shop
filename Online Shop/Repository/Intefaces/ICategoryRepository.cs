using Online_Shop.Models;

namespace Online_Shop.Repository.Intefaces
{
    public interface ICategoryRepository
    {
        Category GetById(int id);
        List<Category> GetAll();
        void Add(Category category);
        void Update(Category category);
        void Delete(Category category);
    }
}
