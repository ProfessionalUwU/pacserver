using Pacserver.Utils;

namespace Pacserver.Tests;

public class getEverySigFileTest {
    [Fact]
    public void getEverySigFile_ListShouldNotBeEmpty() {
        // Arrange
        PacserverUtils utils = new PacserverUtils();
        Directory.CreateDirectory("/tmp/pacserverTest/");
        utils.pacmanCacheDirectory = "/tmp/pacserverTest/";
        File.Create(utils.pacmanCacheDirectory + "zsh-5.9-3-x86_64.pkg.tar.zst.sig");

        // Act
        utils.getEverySigFile();
        var sigFiles = utils.sigFiles;
        Directory.Delete(utils.pacmanCacheDirectory, true);

        // Assert
        sigFiles.Should().NotBeEmpty();
    }
}