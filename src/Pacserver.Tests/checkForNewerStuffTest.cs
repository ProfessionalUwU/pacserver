using Pacserver.Utils;

namespace Pacserver.Tests;

public class checkForNewerStuffTest {
    [Fact]
    public void checkForNewerPackages_throwsExceptionIfNoFilesExist() {
        // Arrange
        PacserverUtils utils = new PacserverUtils();

        // Act
        Action act = () => utils.diff("/tmp/before_update.txt", "/tmp/after_update.txt");

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
        File.Exists("/tmp/before_update.txt").Should().BeTrue();
        File.Exists("/tmp/after_update.txt").Should().BeTrue();
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
    public void getEveryPackageNameAndVersionViaFolderName_throwsExceptionIfListIsEmpty() {
        // Arrange
        PacserverUtils utils = new PacserverUtils();
        Directory.CreateDirectory("/tmp/local");
        utils.pacmanDatabaseDirectory = "/tmp/";

        // Act
        Action act = () => utils.getEveryPackageNameAndVersion("before", "/tmp/packages_before.txt");

        // Assert
        act.Should().Throw<Exception>().WithMessage("How did you execute this without any packages?");
    }
}