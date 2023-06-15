using Pacserver.Utils;

namespace Pacserver.Tests;

public class readPacmanConfigTest {
    [Fact]
    public void readPacmanConfig_returnsNoException() {
        // Arrange
        PacserverUtils utils = new PacserverUtils();

        // Act
        var exception = Record.Exception(() => utils.readPacmanConfig());

        // Assert
        Assert.Null(exception);
    }
}

