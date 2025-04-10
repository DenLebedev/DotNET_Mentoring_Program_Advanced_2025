namespace CatalogService.Application.Intefaces
{
    public interface ISqsPublisher
    {
        Task PublishCatalogItemUpdatedAsync(CatalogItemUpdatedMessage message);
    }
}