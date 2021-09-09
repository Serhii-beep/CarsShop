using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Repository
{
    public interface ICategoryRepository
    {
        Task AddCategory(Category category);
        Task<IEnumerable<Category>> GetCategories();
        Category GetCategory(int id);
        Task DeleteCategory(int id);
        Task UpdateCategory(Category category);
    }
}
