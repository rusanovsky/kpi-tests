using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Spendings.Core.Categories;
using AutoMapper;

namespace onion_spendings.Categories
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly IMapper _mapper;
        public CategoryController(IMapper mapper, ICategoryService service)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<Category> GetAsync(int categoryId)
        {
            return await _service.GetAsync(categoryId);
        }

        [HttpPost]
        public async Task<Category> PostAsync([FromBody]Spendings.Orchrestrators.Categories.Category category)
        {
            var mappedCoreCategory = _mapper.Map<Category>(category);
            var addResult = await _service.PostAsync(mappedCoreCategory);
            return addResult;
        }

        [HttpDelete]
        public async Task<Category> DeleteAsync(int categoryId)
        {
            return await _service.DeleteAsync(categoryId);
        }
    }
}