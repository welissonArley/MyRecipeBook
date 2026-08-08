using CommonTestUtilities.Entities;
using CommonTestUtilities.Files;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.Mappings;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;
using Shouldly;
using System.Net;

namespace UseCases.Tests.Recipe.Register;

public class RegisterRecipeUseCaseTests
{
    static RegisterRecipeUseCaseTests()
    {
        MapsterConfiguration.Configure();
    }

    [Fact]
    public async Task Success_WithoutImage()
    {
        var request = RequestRecipeJsonBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request, recipeIllustration: null);

        result.ShouldNotBeNull();
        result.Title.ShouldBe(request.Title);
    }

    [Fact]
    public async Task Success_WhenImageIsPng()
    {
        var request = RequestRecipeJsonBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request, recipeIllustration: FileBuilder.GetPng());

        result.ShouldNotBeNull();
        result.Title.ShouldBe(request.Title);
    }

    [Fact]
    public async Task Success_WhenImageIsJpeg()
    {
        var request = RequestRecipeJsonBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request, recipeIllustration: FileBuilder.GetJpeg());

        result.ShouldNotBeNull();
        result.Title.ShouldBe(request.Title);
    }

    [Fact]
    public async Task Error_WhenImageIsBmp()
    {
        var request = RequestRecipeJsonBuilder.Build();

        var useCase = CreateUseCase();

        var exception = await useCase.Execute(request, recipeIllustration: FileBuilder.GetBmp()).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_ONLY_IMAGES_ACCEPTED);
        });
    }

    [Fact]
    public async Task Error_WhenImageIsTxt()
    {
        var request = RequestRecipeJsonBuilder.Build();

        var useCase = CreateUseCase();

        var exception = await useCase.Execute(request, recipeIllustration: FileBuilder.GetTxt()).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_ONLY_IMAGES_ACCEPTED);
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenTitleIsEmpty()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase();

        var exception = await useCase.Execute(request, recipeIllustration: null).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_TITLE_REQUIRED);
        });
    }

    private static RegisterRecipeUseCase CreateUseCase()
    {
        var (user, _) = UserBuilder.Build();

        var loggedUser = ILoggedUserBuilder.Build(user);
        var repository = new IRecipeWriteOnlyRepositoryBuilder().Build();
        var unitOfWork = IUnitOfWorkBuilder.Build();

        return new RegisterRecipeUseCase(loggedUser, repository, unitOfWork);
    }
}
