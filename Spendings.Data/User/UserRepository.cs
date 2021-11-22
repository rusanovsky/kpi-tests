using AutoMapper;
using Spendings.Data.DB;
using Spendings.Core.Users;
using System.Threading.Tasks;
using Spendings.Core.Exeptions;
using System.Linq;

namespace Spendings.Data.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public UserRepository(IMapper mapper, AppDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public Core.Users.User Get(int userId)
        {
            User user = _context.Users.Where(u => u.Id == userId).Single();

            if (user.IsDeleted == true)
                throw new AlreadyDeletedException("That user already deleted");

            return _mapper.Map<Core.Users.User>(user);
        }
        public async Task<Core.Users.User> PostAsync(Core.Users.User user)
        {
            var daoNew = _mapper.Map<User>(user);
            var addAsync = await _context.Users.AddAsync(daoNew);
            await _context.SaveChangesAsync();
            return _mapper.Map<Core.Users.User>(addAsync.Entity);
        }

        public async Task<Core.Users.User> PatchAsync(int userId, string newLogin)
        {
            User user = (
                from n in _context.Users
                where n.Id == userId
                select n).First();

            if (user.IsDeleted == true)
                throw new AlreadyDeletedException("That user already deleted");

            user.Login = newLogin;
            var addResult = _context.Users.Update(user);

            await _context.SaveChangesAsync();
            return _mapper.Map<Core.Users.User>(addResult.Entity);
        }
        public async Task<Core.Users.User> DeleteAsync(int userId)
        {
            User user = (
               from n in _context.Users
               where n.Id == userId
               select n).First();

            if(user.IsDeleted == true)
                throw new AlreadyDeletedException("That user already deleted");
            
            user.IsDeleted = true;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<Core.Users.User>(user);
        }
        public void CheckLoginUniquenessAsync(string login)
        {
            var loginsCount = _context.Users.Count(u => u.Login == login && u.IsDeleted == false);
            
            if (loginsCount > 0)
                throw new FailedInsertionException("User with that login already exists");
        }
    }

}