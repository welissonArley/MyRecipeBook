using Azure.Storage.Blobs;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.AI;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Repositories.VerificationCode;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Storage;
using MyRecipeBook.Infrastructure.AI;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Identity;
using MyRecipeBook.Infrastructure.Security.PasswordHashing;
using MyRecipeBook.Infrastructure.Security.Tokens.Access;
using MyRecipeBook.Infrastructure.Storage;
using OpenAI.Chat;
using OpenAI.Images;
using System.ClientModel;
using System.Reflection;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection services)
    {
        public void AddInfrastructure(IConfiguration configuration)
        {
            services.AddRepositories();

            services.AddOpenAI(configuration);

            services.AddSecurity(configuration);

            services.AddScoped<ILoggedUser, LoggedUser>();

            services.AddSingleton(_ =>
            {
                var connectionString = configuration.GetConnectionString("BlobStorage")!;

                return new BlobServiceClient(connectionString);
            });

            services.AddScoped<IStorageService, AzureStorageService>();

            services.AddDbContext<MyRecipeBookDbContext>(config =>
            {
                var connectionString = configuration.GetConnectionString("DbConnection")!;

                config.UseMySQL(connectionString);
            });

            services.AddFluentMigratorCore().ConfigureRunner(config =>
            {
                config
                .AddMySql5()
                .WithGlobalConnectionString(_ =>
                {
                    var connectionString = configuration.GetConnectionString("DbConnection")!;

                    return connectionString;
                })
                .ScanIn(Assembly.Load("MyRecipeBook.Infrastructure"))
                .For.All();
            });
        }

        private void AddRepositories()
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();

            services.AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>();
            services.AddScoped<IRecipeReadOnlyRepository, RecipeRepository>();
            services.AddScoped<IRecipeUpdateOnlyRepository, RecipeRepository>();

            services.AddScoped<IVerificationCodeWriteOnlyRepository, VerificationCodeRepository>();
            services.AddScoped<IVerificationCodeReadOnlyRepository, VerificationCodeRepository>();
        }

        private void AddSecurity(IConfiguration configuration)
        {
            services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();

            services.AddScoped<IAccessTokenGenerator>(provider =>
            {
                var expirationTimeInMinutes = configuration.GetValue<uint>("Jwt:ExpirationTimeMinutes");
                var signingKey = configuration.GetValue<string>("Jwt:SigningKey")!;

                return new JwtTokenHandler(expirationTimeInMinutes, signingKey);
            });
        }

        private void AddOpenAI(IConfiguration configuration)
        {
            services.AddSingleton(_ =>
            {
                var endpoint = configuration.GetValue<string>("Settings:OpenAI:Endpoint")!;
                var apiKey = configuration.GetValue<string>("Settings:OpenAI:ApiKey")!;
                var deploymentName = configuration.GetValue<string>("Settings:OpenAI:Chat:DeploymentName")!;

                return new ChatClient(model: deploymentName, credential: new ApiKeyCredential(apiKey), options: new OpenAI.OpenAIClientOptions
                {
                    Endpoint = new Uri(endpoint)
                });
            });

            services.AddSingleton(_ =>
            {
                var endpoint = configuration.GetValue<string>("Settings:OpenAI:Endpoint")!;
                var apiKey = configuration.GetValue<string>("Settings:OpenAI:ApiKey")!;
                var deploymentName = configuration.GetValue<string>("Settings:OpenAI:Image:DeploymentName")!;

                return new ImageClient(model: deploymentName, credential: new ApiKeyCredential(apiKey), options: new OpenAI.OpenAIClientOptions
                {
                    Endpoint = new Uri(endpoint)
                });
            });

            services.AddScoped<IGenerateRecipeAI, ChatGptService>();
        }
    }
}
