using System.Threading.Tasks;

namespace CatalogService.Application.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T message);
    }
}
