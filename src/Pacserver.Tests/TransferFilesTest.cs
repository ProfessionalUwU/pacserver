using Pacserver.Utils;

namespace Pacserver.Tests;

public class TranserFilesTest {
    [Fact]
    public void transferPacmanCache_doesNotFail() {
        // Arrange
        PacserverUtils utils = new PacserverUtils();

        // Act
        utils.readPacmanConfig();
        utils.transferPacmanCache();

        // Assert
        //Assert.NotEmpty(Directory.GetFiles("/home/rene/test/"));
    }
}
