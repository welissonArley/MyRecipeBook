using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Dashboard;
public interface IGetDashboardUseCase
{
    Task<ResponseRecipesJson> Execute();
}
