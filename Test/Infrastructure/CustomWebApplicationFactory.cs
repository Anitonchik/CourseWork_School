using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SchoolContracts.Infrastructure;
using Microsoft.Extensions.Logging;

namespace SchoolTests.Infrastructure;

internal class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var databaseConfig = services.SingleOrDefault(x => x.ServiceType == typeof(IConnectionString));
            if (databaseConfig is not null)
                services.Remove(databaseConfig);

            var loggerFactory = services.SingleOrDefault(x => x.ServiceType == typeof(LoggerFactory));
            if (loggerFactory is not null)
                services.Remove(loggerFactory);

            services.AddSingleton<IConnectionString, ConnectionStringTest>();
        });

        builder.UseEnvironment("Development");

        base.ConfigureWebHost(builder);
    }
}
