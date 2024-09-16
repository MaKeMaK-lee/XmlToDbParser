using Microsoft.EntityFrameworkCore;
using XmlToDbParser.Entities;

namespace XmlToDbParser.Database
{
    public class XmlToDbParserDatabaseContext(string filename) : DbContext
    {
        readonly private string _filename = filename;

        public DbSet<Client> Clients { get; set; } = null!;

        public DbSet<ContactInfo> ContactInfos { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;

        public DbSet<OrderProduct> OrderProducts { get; set; } = null!;

        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=" + _filename);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasMany(e => e.Products)
                .WithMany(e => e.Orders)
                .UsingEntity<OrderProduct>();

            modelBuilder.Entity<Order>()
                .HasOne(e => e.Client)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.ClientId)
                .IsRequired(true);

            modelBuilder.Entity<ContactInfo>()
                .HasOne(e => e.Client)
                .WithOne(e => e.ContactInfo)
                .HasForeignKey<ContactInfo>(e => e.ClientId)
                .IsRequired(true);





            modelBuilder.Entity<ContactInfo>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<OrderProduct>()
                .Property(b => b.ProductCount)
                .HasDefaultValue(1);
        }
    }
}
