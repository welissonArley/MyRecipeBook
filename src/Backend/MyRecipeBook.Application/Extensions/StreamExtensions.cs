using FileTypeChecker.Extensions;
using FileTypeChecker.Types;

namespace MyRecipeBook.Application.Extensions;

internal static class StreamExtensions
{
    internal static string? DetectImageContentType(this Stream stream)
    {
        string? contentType = null;

        if (stream.Is<PortableNetworkGraphic>())
            contentType = "image/png";
        else if (stream.Is<JointPhotographicExpertsGroup>())
            contentType = "image/jpeg";

        return contentType;
    }
}