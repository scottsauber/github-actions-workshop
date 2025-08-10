using Shouldly;
using WorkshopDemo.Core.Common;

namespace WorkshopDemo.Core.Tests.Common;

public class FileServiceTests
{
    private readonly FileService _fileService = new();

    [Fact]
    public void GetFileContents_ShouldReturnFileContents_WhenFileIsValid()
    {
        var fileContents = Guid.NewGuid().ToString();
        var fileName = "file-service-test.txt";
        File.WriteAllText(fileName, fileContents);

        var result = _fileService.GetFileContents(fileName);

        result.ShouldBe(fileContents);
    }

    [Fact]
    public void GetFileContents_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
    {
        var result = () => _fileService.GetFileContents("Nope.txt");

        result.ShouldThrow<FileNotFoundException>();
    }
}