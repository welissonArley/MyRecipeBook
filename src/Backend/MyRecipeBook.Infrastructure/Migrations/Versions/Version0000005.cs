using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.INCREASE_VERIFICATION_CODE_LENGTH, "Increasing the size of the Code column in the VerificationCodes table")]
public class Version0000005 : ForwardOnlyMigration
{
    public override void Up()
    {
        Alter.Table("VerificationCodes")
            .AlterColumn("Code").AsString(255).NotNullable();
    }
}
