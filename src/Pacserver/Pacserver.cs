using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Pacserver.Utils;
public partial class PacserverUtils {
    public string? pacserverDirectory { get; set; }
    public void prerequisites() {
        AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(100));

        pacserverDirectory = "/tmp/pacserver/";
        pacmanCacheDirectory = string.Empty;
        pacmanDatabaseDirectory = string.Empty;

        if (!Directory.Exists(pacserverDirectory)) {
            Directory.CreateDirectory(pacserverDirectory);
        }
    }

    public void cleanup() {
        if (Directory.Exists(pacserverDirectory)) {
            Directory.Delete(pacserverDirectory, true);
        }
    }

    public string? pacmanCacheDirectory { get; set; }
    public string? pacmanDatabaseDirectory { get; set; }
    public readonly ImmutableList<String> pathsToDetermine = ImmutableList.Create("CacheDir", "DBPath");
    [GeneratedRegex(@"\/(?:[\w.-]+\/)*[\w.-]+(?:\.\w+)*\/?$", RegexOptions.NonBacktracking)] // https://regex101.com/r/GwWeui/2
    private static partial Regex CacheDirOrDBPathRegex();
    public void readPacmanConfig() {
        using (StreamReader file = new StreamReader("/etc/pacman.conf")) {

            string? line;

            while ((line = file.ReadLine()) is not null) {
                foreach (string path in pathsToDetermine.Where(path => line.Contains(path))) {
                    Match match = CacheDirOrDBPathRegex().Match(line);
                    if (match.Success) {
                        switch (path) {
                            case "CacheDir":
                                pacmanCacheDirectory = match.ToString();
                                break;
                            case "DBPath":
                                pacmanDatabaseDirectory = match.ToString();
                                break;
                            default:
                                throw new ArgumentException("Could not deal with " + match.ToString());
                        }
                    } else {
                        string pathsToDetermineString = string.Join(",", pathsToDetermine);
                        throw new DirectoryNotFoundException("Could not determine the necessary file paths: " + pathsToDetermineString);
                    }
                }
            }
        }
    }

    public List<String> packageNamesAndVersion = new List<String>();
    [GeneratedRegex(@".+\.pkg\.tar\.zst$", RegexOptions.NonBacktracking)]
    private static partial Regex onlyGetPackages();
    public void getEveryPackageNameAndVersion(string mode, string filePath) {
        if (Directory.Exists(pacmanCacheDirectory)) {
            if (Directory.GetFiles(pacmanCacheDirectory) is not null) {
                packageNamesAndVersion = Directory.GetFiles(pacmanCacheDirectory).Where(file => onlyGetPackages().IsMatch(file)).ToList();
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

    public void saveDiffToFile(string filePath) {
        if (File.Exists(filePath)) {
            File.Delete(filePath);
        }

        using (File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)) {
            using (StreamWriter sw = new StreamWriter(filePath)) {
                foreach (string packageOrDatabase in diffOfPackagesOrDatabases) {
                    sw.WriteLine(packageOrDatabase);
                }
            }
        }
    }

    public List<String> readDiffFileToList(string filePath) {
        using (File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
            return File.ReadAllLines(filePath).ToList();
        }
    }

    public List<String> databases = new List<String>();
    public void checkIfDatabasesWereModified(string mode, string filePath) {
        databases = Directory.GetFiles(pacmanDatabaseDirectory + "sync/").ToList();

        switch (mode) {
            case "before":
                writeDatabaseAccessTimeToFile(filePath);
                break;
            case "after":
                writeDatabaseAccessTimeToFile(filePath);
                break;
            default:
                throw new ArgumentException("No valid mode was given. Valid modes are before and after");
        }
    }

    public void writeDatabaseAccessTimeToFile(string filePath) {
        if (File.Exists(filePath)) {
            File.Delete(filePath);
        }

        using (File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read)) {
            using (StreamWriter sw = new StreamWriter(filePath)) {
                foreach (string database in databases) {
                    sw.WriteLine(database + " " + File.GetLastAccessTime(database));
                }
            }
        }
    }

    public List<String> databasesToTransfer = new List<String>();
    [GeneratedRegex(@"\/(?:[\w.-]+\/)*[\w.-]+(?:\.\w+)*\/*db", RegexOptions.NonBacktracking)] // https://regex101.com/r/Wm5M0P/1
    private static partial Regex onlyGetDatabaseName();
    public void filterDiffOutputForDatabases() {
        foreach (string database in diffOfPackagesOrDatabases) {
            databasesToTransfer.Add(onlyGetDatabaseName().Match(database).Value);
        }
    }

    private static List<String> newerPackagesAndDatabases = new List<String>();
    public void combinePackagesWithDatabases() {
        newerPackagesAndDatabases.AddRange(packageNamesAndVersion);
        newerPackagesAndDatabases.AddRange(databasesToTransfer);
    }

    public async Task transfer() {
        HttpClient client = new HttpClient();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://192.168.0.69:12000/upload?path=/");
        MultipartFormDataContent content = new MultipartFormDataContent();

        foreach (string pkgOrDb in newerPackagesAndDatabases) {
            content.Add(new ByteArrayContent(File.ReadAllBytes(pacmanCacheDirectory + pkgOrDb)), "path", Path.GetFileName(pacmanCacheDirectory + pkgOrDb));
        }
        request.Content = content;

        await client.SendAsync(request);
    }
}