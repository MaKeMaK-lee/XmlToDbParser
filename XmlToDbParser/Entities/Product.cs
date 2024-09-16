using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XmlToDbParser.Entities
{
    public class Product
    {
        [Key]
        [Required]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("Article")]
        public required string Article { get; set; }

        [Required]
        [Column("Price")]
        public required double Price { get; set; }

        [Column("CountInStock")]
        public int? CountInStock { get; set; }

        public Product() { }

        [Required]
        [Column("OrderProducts")]
        public ICollection<Order> Orders { get; set; } = [];

    }
}
