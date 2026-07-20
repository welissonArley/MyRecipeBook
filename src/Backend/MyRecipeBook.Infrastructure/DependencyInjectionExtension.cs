using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Identity;
using MyRecipeBook.Infrastructure.Security.PasswordHashing;
using MyRecipeBook.Infrastructure.Security.Tokens.Access;
using System.Reflection;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection services)
    {
        public void AddInfrastructure(IConfiguration configuration)
        {
            services.AddRepositories();

            services.AddSecurity(configuration);

            services.AddScoped<ILoggedUser, LoggedUser>();

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
    }
}
