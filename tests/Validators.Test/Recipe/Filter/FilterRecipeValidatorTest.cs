using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Exceptions;
using Xunit;

namespace Validators.Test.Recipe.Filter;
public class FilterRecipeValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new FilterRecipeValidator();

        var request = RequestFilterRecipeJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_Invalid_Cooking_Time()
    {
        var validator = new FilterRecipeValidator();

        var request = RequestFilterRecipeJsonBuilder.Build();
        request.CookingTimes.Add((MyRecipeBook.Communication.Enums.CookingTime)1000);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED));
    }

    [Fact]
    public void Error_Invalid_Difficulty()
    {
        var validator = new FilterRecipeValidator();

        var request = RequestFilterRecipeJsonBuilder.Build();
        request.Difficulties.Add((MyRecipeBook.Communication.Enums.Difficulty)1000);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED));
    }

    [Fact]
    public void Error_Invalid_DishTypes()
    {
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.DishTypes.Add((MyRecipeBook.Communication.Enums.DishType)1000);

        var validator = new FilterRecipeValidator();

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED));
    }
}
