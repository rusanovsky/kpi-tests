using System.ComponentModel.DataAnnotations;

namespace Spendings.Orchrestrators.Users
{
    public class User
    {
        [MinLength(3)]
        [MaxLength(30)]
        public string Login { get; set; }
        [MinLength(3)]
        [MaxLength(30)]
        public string Password { get; set; }
    }
}
