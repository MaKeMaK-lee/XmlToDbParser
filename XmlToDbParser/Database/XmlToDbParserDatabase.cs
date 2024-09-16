using XmlToDbParser.Entities;

namespace XmlToDbParser.Database
{
    public class XmlToDbParserDatabase : IDisposable
    {
        private bool disposed = false;
        private XmlToDbParserDatabaseContext _dbContext;

        public XmlToDbParserDatabase(string filename)
        {
            _dbContext = new XmlToDbParserDatabaseContext(filename);
            _dbContext.Database.EnsureCreated();
        }

        public void Add(IEnumerable<Order> entities)
        {
            _dbContext.AddRange(entities);
        }

        public Product? TryGetProduct(string article, double price)
        {
            return _dbContext.Products.FirstOrDefault(e => e.Article == article && e.Price == price);
        }

        public Client? TryGetClient(string email)
        {
            return _dbContext.Clients.FirstOrDefault(e => e.ContactInfo.Email == email);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            _dbContext.Dispose();

            GC.SuppressFinalize(this);
            disposed = true;
        }
    }
}
