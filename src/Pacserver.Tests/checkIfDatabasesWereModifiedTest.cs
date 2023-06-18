using Pacserver.Utils;

namespace Pacserver.Tests;

public class checkIfDatabasesWereModifiedTest {
    [Fact]
    public void checkIfDatabasesWereModified_throwsExceptionIfNoValidModeIsGiven() {
        // Arrange
        PacserverUtils utils = new PacserverUtils();
        utils.readPacmanConfig();

        // Act
        Action act = () => utils.checkIfDatabasesWereModified("test", "/tmp/test.txt");

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("No valid mode was given. Valid modes are before and after");
    }

    [Fact]
    public void checkIfDatabasesWereModified_throwsNoExceptionIfValidModeIsGiven() {
        // Arrange
        PacserverUtils utils = new PacserverUtils();
        utils.readPacmanConfig();

        // Act
        Action act = () => utils.checkIfDatabasesWereModified("before", "/tmp/test.txt");

        // Assert
        act.Should().NotThrow();
    }
}