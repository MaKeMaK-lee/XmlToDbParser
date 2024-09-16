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
            _dbContext.SaveChanges();
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
