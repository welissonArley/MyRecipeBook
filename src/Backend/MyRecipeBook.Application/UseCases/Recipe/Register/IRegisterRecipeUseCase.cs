using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Recipe.Register;
public interface IRegisterRecipeUseCase
{
    public Task<ResponseRegiteredRecipeJson> Execute(RequestRegisterRecipeFormData request);
}
