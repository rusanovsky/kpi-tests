using System.Threading.Tasks;
using Spendings.Core.Users;

namespace Spendings.Orchrestrators.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }
        public Core.Users.User Get(int userId)
        {
            return _repo.Get(userId);
        }
        public async Task<Core.Users.User> PostAsync(Core.Users.User user)
        {
            _repo.CheckLoginUniquenessAsync(user.Login);
            return await _repo.PostAsync(user);
        }
        public async Task<Core.Users.User> PatchAsync(int userId, string newLogin)
        {
            _repo.CheckLoginUniquenessAsync(newLogin);
            return await _repo.PatchAsync(userId, newLogin);
        }   
        public async Task<Core.Users.User> DeleteAsync(int userId)
        {
            return await _repo.DeleteAsync(userId);
        }
    }
}
