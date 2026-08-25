using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.Recipe.ChangeIllustration;
using MyRecipeBook.Application.UseCases.Recipe.DeleteById;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Application.UseCases.Recipe.GenerateRecipeAI;
using MyRecipeBook.Application.UseCases.Recipe.GetById;
using MyRecipeBook.Application.UseCases.Recipe.Recent;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Application.UseCases.Recipe.UpdateById;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class RecipesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegiteredRecipeJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterRecipeUseCase useCase,
        [FromForm] RequestRecipeJson request,
        IFormFile? recipeIllustration)
    {
        var result = await useCase.Execute(request, recipeIllustration?.OpenReadStream());

        return Created(string.Empty, result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        [FromServices] IGetRecipeByIdUseCase useCase)
    {
        var recipe = await useCase.Execute(id);
        return Ok(recipe);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] IDeleteRecipeByIdUseCase useCase)
    {
        await useCase.Execute(id);

        return NoContent();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] RequestRecipeJson request,
        [FromServices] IUpdateRecipeByIdUseCase useCase)
    {
        await useCase.Execute(id, request);

        return NoContent();
    }

    [HttpPut("{id}/illustration")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeIllustration(
        [FromRoute] Guid id,
        [FromServices] IChangeIllustrationUseCase useCase,
        IFormFile recipeIllustration)
    {
        await useCase.Execute(id, recipeIllustration.OpenReadStream());

        return NoContent();
    }

    [HttpGet("recent")]
    [ProducesResponseType(typeof(ResponseRecipesJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecent([FromServices] IGetRecentRecipesUseCase useCase)
    {
        var recipes = await useCase.Execute();

        return Ok(recipes);
    }

    [HttpPost("filter")]
    [ProducesResponseType(typeof(ResponseRecipesJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> Filter(
        [FromServices] IFilterRecipesUseCase useCase,
        [FromBody] RequestFilterRecipesJson? request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }

    [HttpPost("generate")]
    [ProducesResponseType(typeof(ResponseGeneratedRecipeJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Generate(
        [FromServices] IGenerateRecipeAIUseCase useCase,
        [FromBody] RequestGenerateRecipeJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}