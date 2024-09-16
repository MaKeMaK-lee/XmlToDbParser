using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XmlToDbParser.Entities
{
    public class OrderProduct
    {
        [Required]
        [Column("OrderId")]
        public int OrderId { get; set; }

        [Required]
        [Column("ProductId")]
        public int ProductId { get; set; }

        [Required]
        [Column("ProductCount")]
        public int ProductCount { get; set; }

        [Required]
        public Product Product { get; set; } = default!;

        [Required]
        public Order Order { get; set; } = default!;
    }
}
