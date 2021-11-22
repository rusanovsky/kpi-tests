using System.Threading.Tasks;


namespace Spendings.Core.Categories
{
    public interface ICategoryRepository
    {
        Task<Category> GetAsync(int categoryId);
        Task<Category> PostAsync(Category category);
        Task<Category> DeleteAsync(int categoryId);
        void CheckIfExists(string categoryName);
    }
}
