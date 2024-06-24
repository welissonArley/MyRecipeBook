using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Repositories.Token;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.OpenAI;
using MyRecipeBook.Domain.Services.ServiceBus;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Security.Cryptography;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Validator;
using MyRecipeBook.Infrastructure.Security.Tokens.Refresh;
using MyRecipeBook.Infrastructure.Services.LoggedUser;
using MyRecipeBook.Infrastructure.Services.OpenAI;
using MyRecipeBook.Infrastructure.Services.ServiceBus;
using MyRecipeBook.Infrastructure.Services.Storage;
using OpenAI_API;
using System.Reflection;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddPasswordEncrpter(services);
        AddRepositories(services);
        AddLoggedUser(services);
        AddTokens(services, configuration);
        AddOpenAI(services, configuration);
        AddAzureStorage(services, configuration);
        AddQueue(services, configuration);

        if (configuration.IsUnitTestEnviroment())
            return;

        var databaseType = configuration.DatabaseType();

        if (databaseType == DatabaseType.MySql)
        {
            AddDbContext_MySqlServer(services, configuration);
            AddFluentMigrator_MySql(services, configuration);
        }
        else
        {
            AddDbContext_SqlServer(services, configuration);
            AddFluentMigrator_SqlServer(services, configuration);
        }
    }

    private static void AddDbContext_MySqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnetionString();
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 35));

        services.AddDbContext<MyRecipeBookDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseMySql(connectionString, serverVersion);
        });
    }

    private static void AddDbContext_SqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnetionString();

        services.AddDbContext<MyRecipeBookDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseSqlServer(connectionString);
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        services.AddScoped<IUserDeleteOnlyRepository, UserRepository>();
        services.AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeReadOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeUpdateOnlyRepository, RecipeRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
    }

    private static void AddFluentMigrator_MySql(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnetionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
            .AddMySql5()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(Assembly.Load("MyRecipeBook.Infrastructure")).For.All();
        });
    }

    private static void AddFluentMigrator_SqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnetionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
            .AddSqlServer()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(Assembly.Load("MyRecipeBook.Infrastructure")).For.All();
        });
    }

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
        services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey!));

        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
    }

    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

    private static void AddPasswordEncrpter(IServiceCollection services)
    {
        services.AddScoped<IPasswordEncripter, BCryptNet>();
    }

    private static void AddOpenAI(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGenerateRecipeAI, ChatGPTService>();

        var key = configuration.GetValue<string>("Settings:OpenAI:ApiKey");

        var authentication = new APIAuthentication(key);

        services.AddScoped<IOpenAIAPI>(option => new OpenAIAPI(authentication));
    }

    private static void AddAzureStorage(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("Settings:BlobStorage:Azure");

        if(connectionString.NotEmpty())
        {
            services.AddScoped<IBlobStorageService>(c => new AzureStorageService(new BlobServiceClient(connectionString)));
        }
    }

    private static void AddQueue(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("Settings:ServiceBus:DeleteUserAccount")!;

        if (string.IsNullOrWhiteSpace(connectionString))
            return;

        var client = new ServiceBusClient(connectionString, new ServiceBusClientOptions
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        });

        var deleteQueue = new DeleteUserQueue(client.CreateSender("user"));

        var deleteUserProcessor = new DeleteUserProcessor(client.CreateProcessor("user", new ServiceBusProcessorOptions
        {
            MaxConcurrentCalls = 1
        }));

        services.AddSingleton(deleteUserProcessor);

        services.AddScoped<IDeleteUserQueue>(options => deleteQueue);
    }
}
