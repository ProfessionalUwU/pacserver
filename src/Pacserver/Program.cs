using Pacserver.Utils;

public class Program {
    static void Main(string[] args) {
        if (args.Length == 0) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Please specify an option.");
            Console.ResetColor();
            Console.WriteLine("Possible options are: determinePacmanCacheDirectory, packagesBefore, packagesAfter, diffPackages, databasesBefore, databasesAfter, diffDatabases, filter");
            return;
        }

        PacserverUtils utils = new PacserverUtils();

        switch (args[0]) {
            case "determinePacmanCacheDirectory":
                utils.readPacmanConfig();
                Console.WriteLine(utils.pacmanCacheDirectory);
                break;
            case "packagesBefore":
                utils.readPacmanConfig();
                utils.getEveryPackageNameAndVersion("before", "/tmp/packages_before.txt");
                break;
            case "packagesAfter":
                utils.readPacmanConfig();
                utils.getEveryPackageNameAndVersion("after", "/tmp/packages_after.txt");
                break;
            case "diffPackages":
                utils.diff("/tmp/packages_before.txt", "/tmp/packages_after.txt");
                string packages = string.Join("\n", utils.diffOfPackagesOrDatabases);
                Console.WriteLine(packages);
                break;
            case "databasesBefore":
                utils.readPacmanConfig();
                utils.checkIfDatabasesWereModified("before", "/tmp/databases_before.txt");
                break;
            case "databasesAfter":
                utils.readPacmanConfig();
                utils.checkIfDatabasesWereModified("after", "/tmp/databases_after.txt");
                break;
            case "diffDatabases":
                utils.diff("/tmp/databases_before.txt", "/tmp/databases_after.txt");
                string databases = string.Join("\n", utils.diffOfPackagesOrDatabases);
                Console.WriteLine(databases);
                break;
            case "filter":
                utils.diff("/tmp/databases_before.txt", "/tmp/databases_after.txt");
                utils.filterDiffOutputForDatabases();
                string filteredDatabases = string.Join("\n", utils.databasesToTransfer);
                Console.WriteLine(filteredDatabases);
                break;
            case "getEveryPackageInCache":
                utils.readPacmanConfig();
                utils.getEveryPackageNameAndVersion("before", "/tmp/packages_before.txt");
                string allPackages = string.Join("\n", utils.packageNamesAndVersion);
                Console.WriteLine(allPackages);
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(args[0] + " is not a recognized option.");
                Console.ResetColor();
                Console.WriteLine("Possible options are: determinePacmanCacheDirectory, packagesBefore, packagesAfter, diffPackages, databasesBefore, databasesAfter, diffDatabases, filter");
                break;
        }
    }
}