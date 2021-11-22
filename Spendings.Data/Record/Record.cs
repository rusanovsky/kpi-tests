using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Spendings.Data.Records
{
    [Table("Records")]
    public class Record
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        [ForeignKey("Users")]
        [Column("userId")]
        public int UserId { get; set; }
        [ForeignKey("Categories")]
        [Column("categoryId")]
        public int CategoryId { get; set; }
        [Column("rDate")]
        public DateTime Date { get; set; }
        [Column("amount")]
        public int Amount { get; set; }
    }
}
