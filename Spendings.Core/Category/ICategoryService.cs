using System.Threading.Tasks;

namespace Spendings.Core.Categories
{
    public interface ICategoryService
    {
        Task<Category> GetAsync(int categoryId);
        Task<Category> PostAsync(Category category);
        Task<Category> DeleteAsync(int categoryId);
    }
}
