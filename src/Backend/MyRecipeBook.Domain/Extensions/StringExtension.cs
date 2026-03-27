using System.Diagnostics.CodeAnalysis;

namespace MyRecipeBook.Domain.Extensions;

public static class StringExtension
{
    public static bool IsEmpty([NotNullWhen(false)] this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    public static bool IsNotEmpty([NotNullWhen(true)] this string? value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }
}
