using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter;
public class FilterRecipeUseCase : IFilterRecipeUseCase
{
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _repository;

    public FilterRecipeUseCase(
        IMapper mapper,
        IRecipeReadOnlyRepository repository,
        ILoggedUser loggedUser)
    {
        _mapper = mapper;
        _loggedUser = loggedUser;
        _repository = repository;
    }

    public async Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.User();

        var filters = new Domain.Dtos.FilterRecipesDto
        {
            RecipeTitle_Ingredient = request.RecipeTitle_Ingredient,
            CookingTimes = request.CookingTimes.Distinct().Select(c => (Domain.Enums.CookingTime)c).ToList(),
            Difficulties = request.Difficulties.Distinct().Select(c => (Domain.Enums.Difficulty)c).ToList(),
            DishTypes = request.DishTypes.Distinct().Select(c => (Domain.Enums.DishType)c).ToList()
        };

        var recipes = await _repository.Filter(loggedUser, filters);

        return new ResponseRecipesJson
        {
            Recipes = _mapper.Map<List<ResponseShortRecipeJson>>(recipes)
        };
    }

    private static void Validate(RequestFilterRecipeJson request)
    {
        var validator = new FilterRecipeValidator();

        var result = validator.Validate(request);

        if (result.IsValid.IsFalse())
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).Distinct().ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
