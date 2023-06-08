using Pacserver.Utils;

namespace Pacserver.Tests;

public class TranserFilesTest {
    [Fact]
    public void TransferPacmanCacheTest() {
        string result = PacserverUtils.determinePacmanCacheDirectory();
        PacserverUtils.TransferPacmanCache();

        //Assert.NotEmpty(Directory.GetFiles("/home/rene/test/"));
    }
}
