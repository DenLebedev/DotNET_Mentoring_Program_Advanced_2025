using CartingService.Entities;
using LiteDB;

namespace CartingService.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ILiteCollection<Cart> GetCartCollection();
        ICartDAO Cart { get; }
    }
}
