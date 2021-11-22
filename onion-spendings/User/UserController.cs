using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Spendings.Core.Users;
using AutoMapper;

namespace onion_spendings.Users
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        public UserController(IMapper mapper, IUserService service)
        {
            _service = service;
            _mapper = mapper;
        }
        [HttpGet]
        public User Get(int userId)
        {
            return _service.Get(userId);
        }
        [HttpPost]
        public async Task<User> PostAsync([FromBody] Spendings.Orchrestrators.Users.User user)
        {
            var mappedCoreUser = _mapper.Map<User>(user);
            var addResult = await _service.PostAsync(mappedCoreUser);
            return addResult;
        }
        [HttpPatch]
        public async Task<IActionResult> PatchAsync(int userId,string newLogin)
        {
            var addResult = await _service.PatchAsync(userId, newLogin);
            return Ok(addResult);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int userId)
        {
            var deletedUser = await _service.DeleteAsync(userId);
            return Ok(deletedUser);
        }
    }
}
