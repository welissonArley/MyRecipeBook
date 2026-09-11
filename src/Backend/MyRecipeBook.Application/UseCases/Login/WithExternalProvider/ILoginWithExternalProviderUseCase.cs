using MyRecipeBook.Domain.Dtos;

namespace MyRecipeBook.Application.UseCases.Login.WithExternalProvider;

public interface ILoginWithExternalProviderUseCase
{
    Task<string> Execute(ExternalLoginInfo externalLoginInfo);
}
