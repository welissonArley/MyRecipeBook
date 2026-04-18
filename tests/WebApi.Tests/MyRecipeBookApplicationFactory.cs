using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.MySql;

namespace WebApi.Tests;

public class MyRecipeBookApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MySqlContainer _mySqlContainer;

    public MyRecipeBookApplicationFactory()
    {
        _mySqlContainer = new MySqlBuilder("mysql:8.0")
            .WithDatabase("meulivrodereceitas")
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Tests");
    }

    public async Task InitializeAsync()
    {
        await _mySqlContainer.StartAsync();
    }

    Task IAsyncLifetime.DisposeAsync() => _mySqlContainer.StopAsync();
}
