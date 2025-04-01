using CartingService.DAL.Interfaces;
using LiteDB;

namespace CartingService.DAL
{
    public class LiteDBUnitOfWork : IUnitOfWork
    {
        private LiteDatabase database;
        private CartDAO cartDAO;

        public LiteDBUnitOfWork(string connectionString)
        {
            database = new LiteDatabase(connectionString);
        }

        public ICartDAO Cart
        {
            get
            {
                if (cartDAO == null)
                {
                    cartDAO = new CartDAO(database);
                }
                return cartDAO;
            }
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    database.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
