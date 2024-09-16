using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XmlToDbParser.Entities
{
    public class Order
    {
        [Key]
        [Required]
        [Column("Id")]

        public required int Id { get; set; }

        [Required]
        [Column("ClientId")]
        public int ClientId { get; set; }

        [Required]
        [Column("OrderStatus")]
        [MaxLength(100)]
        public string OrderStatus { get; set; }

        [Column("DateOfCreation")]
        [MaxLength(30)]
        public string DateOfCreation { get; set; }

        [Column("DateOfStatusChange")]
        [MaxLength(30)]
        public string DateOfStatusChange { get; set; }

        public Order() { }

        public Order InitializeMissingValues()
        {
            OrderStatus ??= "Created";
            DateOfCreation ??= DateTime.Now.ToShortDateString();
            DateOfStatusChange ??= DateOfCreation;
            return this;
        }

        [Required]
        public required Client Client { get; set; }

        [Required]
        public ICollection<Product> Products { get; set; } = [];

        [Required]
        public ICollection<OrderProduct> OrderProducts { get; set; } = [];

    }
}
