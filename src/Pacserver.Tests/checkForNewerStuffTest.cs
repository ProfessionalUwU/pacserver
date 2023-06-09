using Pacserver.Utils;

namespace Pacserver.Tests;

public class checkForNewerStuffTest {
    [Fact]
    public void checkForNewerPackages_throwsExceptionIfNoFilesExist() {
        // Arrange
        PacserverUtils utils = new PacserverUtils();
        File.Delete("/tmp/packages_before.txt");
        File.Delete("/tmp/packages_after.txt");

        // Act
        Action act = () => utils.diff("/tmp/packages_before.txt", "/tmp/packages_after.txt");

        // Assert
        act.Should().Throw<FileNotFoundException>().WithMessage("Necessary files could not be found");
    }

    [Fact]
    public void getEveryPackageNameAndVersionViaFolderName_createsFiles() {
        // Arrange
        PacserverUtils utils = new PacserverUtils();
        utils.readPacmanConfig();

        // Act
        utils.getEveryPackageNameAndVersion("before", "/tmp/packages_before.txt");
        utils.getEveryPackageNameAndVersion("after", "/tmp/packages_after.txt");

        // Assert
        File.Exists("/tmp/packages_before.txt").Should().BeTrue();
        File.Exists("/tmp/packages_before.txt").Should().BeTrue();
    }

    [Fact]
    public void packageNamesAndVersion_notEmpty() {
        // Arrange
        PacserverUtils utils = new PacserverUtils();
        utils.readPacmanConfig();
        utils.getEveryPackageNameAndVersion("before", "/tmp/packages_before.txt");

        // Act
        List<String> packageList = utils.packageNamesAndVersion;

        // Assert
        packageList.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void getEveryPackageNameAndVersionViaFolderName_throwsExceptionIfModeIsNotValid() {
        // Arrange
        PacserverUtils utils = new PacserverUtils();

        // Act
        Action act = () => utils.getEveryPackageNameAndVersion("test", "/tmp/test.txt");

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("No valid mode was given. Valid modes are before and after");
    }
}