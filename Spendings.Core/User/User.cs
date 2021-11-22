using System.ComponentModel.DataAnnotations;
namespace Spendings.Core.Users
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
