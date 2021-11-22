
using System.Threading.Tasks;

namespace Spendings.Core.Users
{
    public interface IUserRepository
    {
        User Get(int userId);
        Task<User> PostAsync(User user);
        Task<User> PatchAsync(int userId,string newUser);
        Task<User> DeleteAsync(int userId);
        void CheckLoginUniquenessAsync(string login);


    }
}
