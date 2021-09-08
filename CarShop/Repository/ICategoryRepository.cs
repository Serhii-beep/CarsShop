using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Repository
{
    public interface ICategoryRepository
    {
        void AddCategory(Category category);
        Task<IEnumerable<Category>> GetCategories();
        Category GetCategory(int id);
        void DeleteCategory(int id);
        void UpdateCategory(Category category);
    }
}
