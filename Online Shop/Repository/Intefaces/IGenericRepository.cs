namespace Online_Shop.Repository.Intefaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity GetById(int id);
        List<TEntity> GetAll();
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
