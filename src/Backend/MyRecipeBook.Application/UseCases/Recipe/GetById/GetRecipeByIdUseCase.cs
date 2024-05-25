using AutoMapper;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.GetById;
public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
{
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _repository;

    public GetRecipeByIdUseCase(IMapper mapper, ILoggedUser loggedUser, IRecipeReadOnlyRepository repository)
    {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseRecipeJson> Execute(long recipeId)
    {
        var loggedUser = await _loggedUser.User();

        var recipe = await _repository.GetById(loggedUser, recipeId);

        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

        return _mapper.Map<ResponseRecipeJson>(recipe);
    }
}
