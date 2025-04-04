using CartingService.DAL.Interfaces;
using CartingService.Entities;
using LiteDB;

namespace CartingService.DAL
{
    public class LiteDBUnitOfWork : IUnitOfWork
    {
        private LiteDatabase database;
        private CartDAO? cartDAO;

        #region Constructor

        public LiteDBUnitOfWork(string connectionString)
        {
            database = new LiteDatabase(connectionString);
        }

        #endregion

        public ILiteCollection<Cart> GetCartCollection()
        {
            return database.GetCollection<Cart>("carts");
        }

        public ICartDAO Cart
        {
            get
            {
                if (cartDAO == null)
                {
                    cartDAO = new CartDAO(this);
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
