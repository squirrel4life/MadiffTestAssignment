using MadiffTestAssignment.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;

namespace MadiffTestAssignment.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<ICardService> CardServiceMock { get; } = new();
    public Mock<ICardActionRegistry> ActionRegistryMock { get; } = new();
    public Mock<IAllowedActionsGenerator> AllowedActionsGeneratorMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(ICardService));
            services.RemoveAll(typeof(ICardActionRegistry));
            services.RemoveAll(typeof(IAllowedActionsGenerator));
            
            services.AddSingleton(CardServiceMock.Object);
            services.AddSingleton(ActionRegistryMock.Object);
            services.AddSingleton(AllowedActionsGeneratorMock.Object);
        });
    }
}
