using CommonTestUtilities.AI;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyRecipeBook.Domain.AI;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure.DataAccess;
using Testcontainers.Azurite;
using Testcontainers.MySql;
using WebApi.Tests.Resources;

namespace WebApi.Tests;

public class MyRecipeBookApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public UserIdentityManager User1 { get; private set; } = default!;

    public string TOKEN_USER_NOT_FOUND_IN_DATABASE { get; private set; } = string.Empty;

    private readonly MySqlContainer _mySqlContainer;
    private readonly AzuriteContainer _azuriteContainer;

    public MyRecipeBookApplicationFactory()
    {
        _mySqlContainer = new MySqlBuilder("mysql:8.0")
            .WithDatabase("meulivrodereceitas")
            .WithCommand("--innodb-use-native-aio=0")
            .Build();

        _azuriteContainer = new AzuriteBuilder("mcr.microsoft.com/azure-storage/azurite:latest")
            .WithCommand("--skipApiVersionCheck")
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Tests")
            .ConfigureAppConfiguration((_, configuration) =>
            {
                var parameters = new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DbConnection"] = _mySqlContainer.GetConnectionString(),
                    ["ConnectionStrings:BlobStorage"] = _azuriteContainer.GetConnectionString()
                };

                configuration.AddInMemoryCollection(parameters);
            })
            .ConfigureTestServices(services =>
            {
                services.RemoveAll<IGenerateRecipeAI>();
                services.AddScoped(_ => IGenerateRecipeAIBuilder.Build());
            });
    }

    public async Task InitializeAsync()
    {
        await _mySqlContainer.StartAsync();
        await _azuriteContainer.StartAsync();

        await using var scope = Services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

        var (user, password) = UserBuilder.Build();
        user.Password = passwordHasher.HashPassword(password);

        var recipe = RecipeBuilder.Build(user);

        await dbContext.Users.AddAsync(user);
        await dbContext.Recipes.AddAsync(recipe);

        await dbContext.SaveChangesAsync();

        var user1AccessToken = accessTokenGenerator.Generate(user);

        User1 = new UserIdentityManager(user, recipe, password, user1AccessToken);

        TOKEN_USER_NOT_FOUND_IN_DATABASE = accessTokenGenerator.Generate(new MyRecipeBook.Domain.Entities.User());
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _mySqlContainer.StopAsync();
        await _azuriteContainer.StopAsync();
    }
}
