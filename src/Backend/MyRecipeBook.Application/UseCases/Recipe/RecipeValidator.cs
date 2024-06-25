using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.Recipe;
public class RecipeValidator : AbstractValidator<RequestRecipeJson>
{
    public RecipeValidator()
    {
        RuleFor(recipe => recipe.Title).NotEmpty().WithMessage(ResourceMessagesException.RECIPE_TITLE_EMPTY);
        RuleFor(recipe => recipe.CookingTime).IsInEnum().WithMessage(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);
        RuleFor(recipe => recipe.Difficulty).IsInEnum().WithMessage(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED);
        RuleFor(recipe => recipe.Ingredients.Count).GreaterThan(0).WithMessage(ResourceMessagesException.AT_LEAST_ONE_INGREDIENT);
        RuleFor(recipe => recipe.Instructions.Count).GreaterThan(0).WithMessage(ResourceMessagesException.AT_LEAST_ONE_INSTRUCTION);
        RuleForEach(recipe => recipe.DishTypes).IsInEnum().WithMessage(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED);
        RuleForEach(recipe => recipe.Ingredients).NotEmpty().WithMessage(ResourceMessagesException.INGREDIENT_EMPTY);
        RuleForEach(recipe => recipe.Instructions).ChildRules(instructionRule =>
        {
            instructionRule.RuleFor(instruction => instruction.Step).GreaterThan(0).WithMessage(ResourceMessagesException.NON_NEGATIVE_INSTRUCTION_STEP);
            instructionRule
            .RuleFor(instruction => instruction.Text)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.INSTRUCTION_EMPTY)
            .MaximumLength(2000)
            .WithMessage(ResourceMessagesException.INSTRUCTION_EXCEEDS_LIMIT_CHARACTERS);
        });
        RuleFor(recipe => recipe.Instructions)
            .Must(instructions => instructions.Select(i => i.Step).Distinct().Count() == instructions.Count)
            .WithMessage(ResourceMessagesException.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER);
    }
}
