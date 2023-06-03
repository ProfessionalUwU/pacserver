using System.Text.RegularExpressions;

public class Pacserver {
    public static string pacmanCacheDirectory { get; set; } = string.Empty;
    public static string determinePacmanCacheDirectory() {
        string defaultPacmanCacheDirectory = "/var/cache/pacman/pkg/";

        Regex regex = new Regex(@"\/(?:[\w.-]+\/)*[\w.-]+(?:\.\w+)*\/?$"); // https://regex101.com/r/GwWeui/2
        string? line;
        StreamReader file = new StreamReader(@"/etc/pacman.conf");
        while ((line = file.ReadLine()) is not null) {
            if (line.Contains("CacheDir")) {
                Match match = regex.Match(line);
                if (match.Success) {
                    pacmanCacheDirectory = match.ToString();
                } else {
                    throw new Exception("Could not determine where pacman cache is! Would normally be found here " + defaultPacmanCacheDirectory);
                }
            } else {
                throw new Exception("Pacman config has no CacheDir specified!");
            }
        }
        file.Close();

        return pacmanCacheDirectory;
    }

    public static string pacmanDatabaseDirectory { get; set; } = string.Empty;
    public static string determinePacmanDatabaseDirectory() {
        return pacmanDatabaseDirectory;
    }

    public static void checkForNewerPackagesAndDatabases() {

    }

    public static void transferPacmanCache() {

    }

    public static void transferPacmanDatabases() {

    }
}