using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_VERIFICATION_CODES, "Creating VerificationCodes table")]
public class Version0000003 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("VerificationCodes")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("Code").AsString(6).NotNullable()
            .WithColumn("Type").AsString(50).NotNullable()
            .WithColumn("UserId").AsGuid().NotNullable()
                .ForeignKey("FK_VerificationCodes_Users_UserId", "Users", "Id");
    }
}
