using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exception;

namespace MyRecipeBook.Application.UseCases.Recipe;

public class RecipeValidator : AbstractValidator<RequestRecipeJson>
{
    public RecipeValidator()
    {
        RuleFor(recipe => recipe.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_TITLE_REQUIRED)
            .MaximumLength(250)
            .WithMessage(ResourceMessagesException.VALIDATION_TITLE_MAX_LENGTH);

        RuleFor(recipe => recipe.CookTime)
            .IsInEnum()
            .WithMessage(ResourceMessagesException.VALIDATION_COOK_TIME_INVALID);

        RuleFor(recipe => recipe.DishTypes)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_AT_LEAST_ONE_DISH_TYPE);

        RuleForEach(recipe => recipe.DishTypes)
            .IsInEnum()
            .WithMessage(ResourceMessagesException.VALIDATION_DISH_TYPE_INVALID);

        RuleFor(recipe => recipe.Ingredients)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_AT_LEAST_ONE_INGREDIENT);

        RuleForEach(recipe => recipe.Ingredients)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_INGREDIENT_EMPTY)
            .MaximumLength(250)
            .WithMessage(ResourceMessagesException.VALIDATION_INGREDIENT_MAX_LENGTH);

        RuleFor(recipe => recipe.Instructions)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_AT_LEAST_ONE_INSTRUCTION);

        RuleFor(recipe => recipe.Instructions)
            .Must(instructions => instructions.Select(instruction => instruction.Order).Distinct().Count() == instructions.Count)
            .WithMessage(ResourceMessagesException.VALIDATION_INSTRUCTION_ORDER_DUPLICATED)
            .When(recipe => recipe.Instructions.Count > 1);

        RuleForEach(recipe => recipe.Instructions).ChildRules(instruction =>
        {
            instruction.RuleFor(item => item.Order)
                .GreaterThan(0)
                .WithMessage(ResourceMessagesException.VALIDATION_INSTRUCTION_ORDER_INVALID);

            instruction.RuleFor(item => item.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.VALIDATION_INSTRUCTION_DESCRIPTION_REQUIRED)
                .MaximumLength(2000)
                .WithMessage(ResourceMessagesException.VALIDATION_INSTRUCTION_DESCRIPTION_MAX_LENGTH);
        });
    }
}
