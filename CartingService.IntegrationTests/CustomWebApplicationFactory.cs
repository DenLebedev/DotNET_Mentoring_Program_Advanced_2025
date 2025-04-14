using System.IO;
using CartingService.BLL.Interfaces;
using CartingService.DTOs;
using CartingService.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace CartingService.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICartBL));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                var mockCartBL = new Mock<ICartBL>();
                mockCartBL.Setup(x => x.AddItemToCartAsync(It.IsAny<string>(), It.IsAny<ItemDto>()));
                services.AddSingleton(mockCartBL.Object);
            });
        }
    }
}
