namespace CommonTestUtilities.Files;

public static class FileBuilder
{
    private const string ValidPng = "valid-image.png";
    private const string ValidJpeg = "valid-image.jpg";
    private const string InvalidBmp = "invalid-image.bmp";
    private const string InvalidTxt = "invalid-file.txt";

    public static Stream GetPng() => GetStream(ValidPng);

    public static Stream GetJpeg() => GetStream(ValidJpeg);

    public static Stream GetBmp() => GetStream(InvalidBmp);

    public static Stream GetTxt() => GetStream(InvalidTxt);

    private static Stream GetStream(string fileName)
    {
        var assembly = typeof(FileBuilder).Assembly;

        var resourceName = assembly
            .GetManifestResourceNames()
            .Single(name => name.EndsWith(fileName));

        return assembly.GetManifestResourceStream(resourceName)!;
    }
}
