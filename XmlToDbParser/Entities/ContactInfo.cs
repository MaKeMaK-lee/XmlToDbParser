using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XmlToDbParser.Entities
{
    public class ContactInfo
    {
        [Key]
        [Required]
        [Column("ClientId")]
        public int ClientId { get; set; }

        [Required]
        [Column("Name")]
        [MaxLength(1000)]
        public required string Name { get; set; }

        [Column("Email")]
        [MaxLength(256)]
        public string? Email { get; set; }

        [Column("Telephone")]
        [MaxLength(32)]
        public string? Telephone { get; set; }

        [Required]
        public Client Client { get; set; } = default!;
    }
}
