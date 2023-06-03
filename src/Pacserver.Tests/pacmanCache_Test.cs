using Pacserver.Utils;

namespace Pacserver.Tests;

public class pacmanCache_Test {
    [Fact]
    public void doesPacmanCacheExist() {
        string result = PacserverUtils.determinePacmanCacheDirectory();

        Assert.Equivalent(result, "/var/cache/pacman/pkg/");
    }
}

