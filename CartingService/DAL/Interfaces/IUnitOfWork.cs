namespace CartingService.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICartDAO Cart { get; }
    }
}
