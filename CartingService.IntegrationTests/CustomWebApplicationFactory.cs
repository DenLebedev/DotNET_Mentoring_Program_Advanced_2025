using System.IO;
using CartingService.BLL.Interfaces;
using CartingService.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace CartingService.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

                services.AddAuthorization(options =>
                {
                    options.AddPolicy("CustomerOrManager", policy =>
                        policy.RequireAssertion(_ => true));
                });

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
