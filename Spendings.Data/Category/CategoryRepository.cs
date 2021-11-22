using AutoMapper;
using Spendings.Data.DB;
using System.Threading.Tasks;
using System.Linq;
using Spendings.Core.Categories;
using Spendings.Core.Exeptions;

namespace Spendings.Data.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CategoryRepository(IMapper mapper, AppDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Spendings.Core.Categories.Category> GetAsync(int categoryId)
        {
            var categorySearchResult = _context.Categories.Where(c => c.Id == categoryId).Single();
            return _mapper.Map<Core.Categories.Category>(categorySearchResult);
        }
        public async Task<Spendings.Core.Categories.Category> PostAsync(Spendings.Core.Categories.Category category)
        {
            var mappedToDataCategory = _mapper.Map<Spendings.Data.Categories.Category>(category);

            var AddResult =await _context.Categories.AddAsync(mappedToDataCategory);
            await _context.SaveChangesAsync();

            var outCategory = _mapper.Map<Core.Categories.Category>(AddResult.Entity);
            return outCategory;
        }
        public async Task<Spendings.Core.Categories.Category> DeleteAsync(int categoryId)
        {
            var categorSearchResult = _context.Categories.Where(c => c.Id == categoryId).Single();

            _context.Categories.Remove(categorSearchResult);
            await _context.SaveChangesAsync();

            return _mapper.Map<Core.Categories.Category>(categorSearchResult);
        }
        public void CheckIfExists(string categoryName)
        {
            var count = _context.Categories.Count(c => c.Name == categoryName); 
           
            if (count > 0)
                throw new FailedInsertionException(); 
        }
    }
}
