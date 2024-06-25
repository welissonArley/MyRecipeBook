namespace MyRecipeBook.Application.UseCases.Login.External;
public interface IExternalLoginUseCase
{
    Task<string> Execute(string name, string email);
}
