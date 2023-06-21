using Pacserver.Utils;

namespace Pacserver.Tests;

public class TranserFilesTest {
    [Fact]
    public void transferPacmanCache_doesNotFail() {
        // Arrange
        PacserverUtils utils = new PacserverUtils();

        // Act
        utils.readPacmanConfig();
        Action act = () => utils.transfer();

        // Assert
        act.Should().NotThrow();
    }
}
