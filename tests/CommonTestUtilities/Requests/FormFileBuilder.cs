using Microsoft.AspNetCore.Http;

namespace CommonTestUtilities.Requests;
public class FormFileBuilder
{
    public static IFormFile Png()
    {
        var stream = File.OpenRead("Files/FilePng.png");

        var file = new FormFile(
            baseStream: stream,
            baseStreamOffset: 0,
            length: stream.Length,
            name: "File",
            fileName: "IMG0001.png")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/png"
        };

        return file;
    }

    public static IFormFile Jpg()
    {
        var stream = File.OpenRead("Files/FileJpg.jpg");

        var file = new FormFile(
            baseStream: stream,
            baseStreamOffset: 0,
            length: stream.Length,
            name: "File",
            fileName: "IMG0001.jpg")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpg"
        };

        return file;
    }

    public static IFormFile Txt()
    {
        var stream = File.OpenRead("Files/FileText.txt");

        var file = new FormFile(
            baseStream: stream,
            baseStreamOffset: 0,
            length: stream.Length,
            name: "File",
            fileName: "FILE0001.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain"
        };

        return file;
    }

    public static IList<IFormFile> ImageCollection()
    {
        return
        [
            Png(),
            Jpg()
        ];
    }
}
