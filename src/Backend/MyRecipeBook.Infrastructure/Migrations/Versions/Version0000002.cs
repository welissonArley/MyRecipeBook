using FluentMigrator;
using System.Data;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_RECIPES, "Creating Recipe tables")]
public class Version0000002 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Recipes")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Title").AsString(250).NotNullable()
            .WithColumn("CookTime").AsString(50).NotNullable()
            .WithColumn("UserId").AsGuid().NotNullable()
                .ForeignKey("FK_Recipes_Users_UserId", "Users", "Id");

        Create.Table("RecipeIngredients")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Item").AsString(250).NotNullable()
            .WithColumn("RecipeId").AsGuid().NotNullable()
                .ForeignKey("FK_RecipeIngredients_Recipes_RecipeId", "Recipes", "Id").OnDelete(Rule.Cascade);

        Create.Table("RecipeInstructions")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Order").AsInt32().NotNullable()
            .WithColumn("Description").AsString(2000).NotNullable()
            .WithColumn("RecipeId").AsGuid().NotNullable()
                .ForeignKey("FK_RecipeInstructions_Recipes_RecipeId", "Recipes", "Id").OnDelete(Rule.Cascade);

        Create.Table("RecipeDishTypes")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Type").AsString(50).NotNullable()
            .WithColumn("RecipeId").AsGuid().NotNullable()
                .ForeignKey("FK_RecipeDishTypes_Recipes_RecipeId", "Recipes", "Id").OnDelete(Rule.Cascade);
    }
}
