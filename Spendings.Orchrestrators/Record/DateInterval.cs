using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Spendings.Orchrestrators.Records
{
    public class DateInterval
    {
        [BindRequired]
        [RegularExpression(@"[0-9]{2}(.)[0-9]{2}(.)[0-9]{4}")]
        public string from { get; set; }
        [BindRequired]
        [RegularExpression(@"[0-9]{2}(.)[0-9]{2}(.)[0-9]{4}")]
        public string till { get; set; }
    }
}
