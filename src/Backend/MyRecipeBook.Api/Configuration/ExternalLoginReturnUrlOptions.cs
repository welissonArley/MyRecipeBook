namespace MyRecipeBook.Api.Configuration;

public sealed class ExternalLoginReturnUrlOptions
{
    public const string SectionName = "ExternalLoginReturnUrlOptions";

    public string Site { get; set; } = string.Empty;
    public string App { get; set; } = string.Empty;
}