using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XmlToDbParser.Entities
{
    public class Client
    {
        [Key]
        [Required]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public ICollection<Order> Orders { get; set; } = [];

        [Required]
        public ContactInfo? ContactInfo { get; set; }
    }
}
