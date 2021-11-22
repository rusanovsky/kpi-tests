using System.Threading.Tasks;
using Spendings.Core.Categories;

namespace Spendings.Orchrestrators.Categories
{
   public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }
        public async Task<Core.Categories.Category> GetAsync(int categoryId)
        {
            return await _repo.GetAsync(categoryId);
        }
        public async Task<Core.Categories.Category> PostAsync(Spendings.Core.Categories.Category category)
        {
            _repo.CheckIfExists(category.Name);
            return await _repo.PostAsync(category);
        }
        public async Task<Core.Categories.Category> DeleteAsync(int categoryId)
        {
            return await _repo.DeleteAsync(categoryId);
        }
    }
}
