using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarShop.DOM.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task SaveChangesAsync();
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);

        bool Exists(int id);
    }
}
