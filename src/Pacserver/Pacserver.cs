using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Pacserver.Utils;
public class PacserverUtils {
    public string pacmanCacheDirectory = string.Empty;
    public string pacmanDatabaseDirectory = string.Empty;
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

    public List<String> packageNamesAndVersion = new List<String>();
    public void getEveryPackageNameAndVersion(string mode, string filePath) {
        Regex regex = new Regex(@".+\.pkg\.tar\.zst$");

        if (Directory.Exists(pacmanCacheDirectory)) {
            if (Directory.GetFiles(pacmanCacheDirectory) is not null) {
                packageNamesAndVersion = Directory.GetFiles(pacmanCacheDirectory).Where(file => regex.IsMatch(file)).ToList();
            } else {
                Console.WriteLine("No packages found in pacman cache");
            }
        } else {
            Console.WriteLine("No pacman cache directory found");
        }

        switch (mode) {
            case "before":
                writePackageNamesAndVersionToFile(filePath);
                break;
            case "after":
                writePackageNamesAndVersionToFile(filePath);
                break;
            default:
                throw new ArgumentException("No valid mode was given. Valid modes are before and after");
        }
    }

    public void writePackageNamesAndVersionToFile(string filePath) {
        if (File.Exists(filePath)) {
            File.Delete(filePath);
        }
        
        using (File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)) {
            using (StreamWriter sw = new StreamWriter(filePath)) {
                foreach (string package in packageNamesAndVersion) {
                    sw.WriteLine(package);
                }
            }
        }
    }

    public List<String> diffOfPackagesOrDatabases = new List<String>();
    public void diff(string before, string after) {
        if (File.Exists(before) && File.Exists(after)) {
            diffOfPackagesOrDatabases = File.ReadAllLines(after).Except(File.ReadLines(before)).ToList();
        } else {
            throw new FileNotFoundException("Necessary files could not be found");
        }
    }

    public List<String> databases = new List<String>();
    public void checkIfDatabasesWereModified(string mode, string filePath) {
        string[] databases = Directory.GetFiles(pacmanDatabaseDirectory + "sync/");

        foreach (string database in databases) {
            switch (mode) {
                case "before":
                    writeDatabaseAccessTimeToFile(filePath, database);
                    break;
                case "after":
                    writeDatabaseAccessTimeToFile(filePath, database);
                    break;
                default:
                    throw new ArgumentException("No valid mode was given. Valid modes are before and after");
            }
        }
    }

    public void writeDatabaseAccessTimeToFile(string filePath, string database) {
        if (File.Exists(filePath)) {
            File.Delete(filePath);
        }

        using (File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)) {
            using (StreamWriter sw = new StreamWriter(filePath)) {
                sw.WriteLine(database + " " + File.GetLastAccessTime(database));
            }
        }
    }

    public List<String> databasesToTransfer = new List<String>();
    public void filterDiffOutputForDatabases() {
        foreach (string database in diffOfPackagesOrDatabases) {
            databasesToTransfer.Add(getDatabaseFromRegex(database, @"\/(?:[\w.-]+\/)*[\w.-]+(?:\.\w+)*\/*db")); // https://regex101.com/r/Wm5M0P/1
        }
    }

    public string getDatabaseFromRegex(string input, string pattern) {
        string match = string.Empty;
        MatchCollection matchCollection = Regex.Matches(input, pattern);

        foreach (Match matches in matchCollection) {
            match = matches.Value;
        }

        return match;
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