using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Mappings;
using MyRecipeBook.Application.UseCases.Login.WithEmailAndPassword;
using MyRecipeBook.Application.UseCases.PasswordRecovery.RequestCode;
using MyRecipeBook.Application.UseCases.PasswordRecovery.ResetPassword;
using MyRecipeBook.Application.UseCases.Recipe.DeleteById;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Application.UseCases.Recipe.GetById;
using MyRecipeBook.Application.UseCases.Recipe.Recent;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Application.UseCases.Recipe.UpdateById;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Application.UseCases.User.Profile;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Application.UseCases.User.Update;

namespace MyRecipeBook.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        MapsterConfiguration.Configure();

        services.AddUseCases();
    }

    private static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IRegisterUserAccountUseCase, RegisterUserAccountUseCase>();
        services.AddScoped<ILoginWithEmailAndPasswordUseCase, LoginWithEmailAndPasswordUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();

        services.AddScoped<IRegisterRecipeUseCase, RegisterRecipeUseCase>();
        services.AddScoped<IGetRecipeByIdUseCase, GetRecipeByIdUseCase>();
        services.AddScoped<IDeleteRecipeByIdUseCase, DeleteRecipeByIdUseCase>();
        services.AddScoped<IUpdateRecipeByIdUseCase, UpdateRecipeByIdUseCase>();
        services.AddScoped<IGetRecentRecipesUseCase, GetRecentRecipesUseCase>();
        services.AddScoped<IFilterRecipesUseCase, FilterRecipesUseCase>();

        services.AddScoped<IRequestPasswordRecoveryCodeUseCase, RequestPasswordRecoveryCodeUseCase>();
        services.AddScoped<IResetPasswordUseCase, ResetPasswordUseCase>();
    }
}
