using Pacserver.Utils;

namespace Pacserver.Tests;

public class pacmanDatabase_Test {
    [Fact]
    public void doesPacmanDatabaseExist() {
        string result = PacserverUtils.determinePacmanDatabaseDirectory();

        Assert.Equivalent(result, "/var/lib/pacman/");
    }
}