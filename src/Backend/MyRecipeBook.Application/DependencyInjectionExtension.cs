using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.UseCases.User.Register;

namespace MyRecipeBook.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRegisterUserAccountUseCase, RegisterUserAccountUseCase>();
    }
}
