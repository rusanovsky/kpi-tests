
using System.Threading.Tasks;

namespace Spendings.Core.Users
{
    public interface IUserService
    {
        User Get(int userId);
        Task<User> PostAsync(User user);
        Task<User> PatchAsync(int userId,string newLogin);
        Task<User> DeleteAsync(int userId);

    }
}
