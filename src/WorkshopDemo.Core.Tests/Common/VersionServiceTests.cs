using Shouldly;
using NSubstitute;
using WorkshopDemo.Core.Common;

namespace WorkshopDemo.Core.Tests.Common;

public class VersionServiceTests
{
    [Fact]
    public void GetVersion_ShouldRetrieveFileContentsOnce_WhenVersionServiceIsCreatedMultipleTimesToProtectFromWrongServiceLifetimeRegistration()
    {
        var fileContents = Guid.NewGuid().ToString();
        var fileService = Substitute.For<IFileService>();
        var versionFilePath = "version.txt";
        fileService.GetFileContents(versionFilePath).Returns(fileContents);

        var version1 = new VersionService(fileService).GetVersion();
        var version2 = new VersionService(fileService).GetVersion();

        version1.ShouldBe(fileContents);
        version2.ShouldBe(fileContents);
        fileService.Received(1).GetFileContents(versionFilePath);
    }
}