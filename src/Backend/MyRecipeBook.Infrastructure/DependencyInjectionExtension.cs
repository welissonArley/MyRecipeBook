using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configurarion)
    {
        var databaseType = configurarion.GetConnectionString("DatabaseType");

        var databaseTypeEnum = (DatabaseType)Enum.Parse(typeof(DatabaseType), databaseType!);

        if (databaseTypeEnum == DatabaseType.MySql)
            AddDbContext_MySqlServer(services, configurarion);
        else
            AddDbContext_SqlServer(services, configurarion);

        AddRepositories(services);
    }

    private static void AddDbContext_MySqlServer(IServiceCollection services, IConfiguration configurarion)
    {
        var connectionString = configurarion.GetConnectionString("ConnectionMySQLServer");
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 35));

        services.AddDbContext<MyRecipeBookDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseMySql(connectionString, serverVersion);
        });
    }

    private static void AddDbContext_SqlServer(IServiceCollection services, IConfiguration configurarion)
    {
        var connectionString = configurarion.GetConnectionString("ConnectionSQLServer");

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
    }
}
