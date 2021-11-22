using System.ComponentModel.DataAnnotations;

namespace Spendings.Orchrestrators.Categories
{
   public class Category
    {
        [MinLength(3)]
        [MaxLength(15)]
        public string Name { get; set; }
    }
}
