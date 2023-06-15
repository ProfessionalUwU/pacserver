using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Pacserver.Utils;
public class PacserverUtils {
    public string pacmanCacheDirectory = string.Empty;
    public static string pacmanDatabaseDirectory = string.Empty;
    public static List<String> pathsToDetermine = new List<String>() { "CacheDir", "DBPath" };
    public void readPacmanConfig() {
        using (StreamReader file = new StreamReader("/etc/pacman.conf")) {
            Regex regex = new Regex(@"\/(?:[\w.-]+\/)*[\w.-]+(?:\.\w+)*\/?$"); // https://regex101.com/r/GwWeui/2
            string? line;

            while ((line = file.ReadLine()) is not null) {
                foreach (string path in pathsToDetermine) {
                    if (line.Contains(path)) {
                        Match match = regex.Match(line);
                        if (match.Success) {
                            switch (path) {
                                case "CacheDir":
                                    pacmanCacheDirectory = match.ToString();
                                    break;
                                case "DBPath":
                                    pacmanDatabaseDirectory = match.ToString();
                                    break;
                                default:
                                    throw new Exception("Could not deal with " + match.ToString());
                            }
                        } else {
                            string pathsToDetermineString = string.Join(",", pathsToDetermine);
                            throw new Exception("Could not determine the necessary file paths: " + pathsToDetermineString);
                        }
                    }
                }
            }
        }
    }

    private List<String> packageNamesAndVersion = new List<String>();
    public void getEveryPackageNameAndVersionViaFolderName(string filePath) {
        string[] directories = Directory.GetDirectories(pacmanDatabaseDirectory + "local/");
        foreach (string directory in directories) {
            packageNamesAndVersion.Add(new DirectoryInfo(directory).Name);
        }
        File.WriteAllLines(filePath, packageNamesAndVersion);
    }

    public List<String> newerPackages = new List<String>();
    public void checkForNewerPackages() {
        if (File.Exists("/tmp/before_update.txt") && File.Exists("/tmp/after_update.txt")) {
            newerPackages = File.ReadAllLines("/tmp/after_update.txt").Except(File.ReadLines("/tmp/before_update.txt")).ToList();
        } else {
            throw new FileNotFoundException("Necessary files could not be found");
        }
    }

    private static List<String> newerPackagesAndDatabases = new List<String>();
    public async void transferPacmanCache() {
        HttpClient client = new HttpClient();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://192.168.0.69:12000/upload?path=/");
        MultipartFormDataContent content = new MultipartFormDataContent();

        foreach (string pkgOrDb in newerPackagesAndDatabases) {
            content.Add(new ByteArrayContent(File.ReadAllBytes(pacmanCacheDirectory + pkgOrDb)), "path", Path.GetFileName(pacmanCacheDirectory + pkgOrDb));
        }
        request.Content = content;

        await client.SendAsync(request);
    }

    public void transferPacmanDatabases() {

    }
}