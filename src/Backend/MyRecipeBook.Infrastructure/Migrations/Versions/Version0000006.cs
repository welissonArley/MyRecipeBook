using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_REFRESH_TOKENS, "Creating RefreshTokens table")]
public class Version0000006 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("RefreshTokens")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Value").AsString(255).NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("UserId").AsGuid().NotNullable()
                .ForeignKey("FK_RefreshTokens_Users_UserId", "Users", "Id");
    }
}
