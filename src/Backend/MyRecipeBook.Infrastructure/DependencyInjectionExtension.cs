using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Security.PasswordHashing;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    extension (IServiceCollection services)
    {
        public void AddInfrastructure()
        {
            services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();

            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();

            services.AddDbContext<MyRecipeBookDbContext>(config =>
            {
                config.UseMySQL("Server=localhost;Database=meulivrodereceitas;Uid=root;Pwd=@Password123;");
            });
        }
    }
}
