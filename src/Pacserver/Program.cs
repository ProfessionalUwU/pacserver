using Pacserver.Utils;

public class Program {
    static void Main(string[] args) {
        if (args.Length == 0) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Please specify an option.");
            Console.ResetColor();
            Console.WriteLine("Possible options are: before, after");
            return;
        }

        PacserverUtils utils = new PacserverUtils();
        utils.readPacmanConfig();

        switch (args[0]) {
            case "before":
                utils.getEveryPackageNameAndVersion("before", "/tmp/packages_before.txt");
                utils.checkIfDatabasesWereModified("before", "/tmp/databases_before.txt");
                break;
            case "after":
                utils.getEveryPackageNameAndVersion("after", "/tmp/packages_after.txt");
                utils.checkIfDatabasesWereModified("after", "/tmp/databases_after.txt");

                utils.diff("/tmp/packages_before.txt", "/tmp/packages_after.txt");
                utils.saveDiffToFile("/tmp/package_diff.txt");

                utils.diff("/tmp/databases_before.txt", "/tmp/databases_after.txt");
                utils.saveDiffToFile("/tmp/database_diff.txt");
                utils.filterDiffOutputForDatabases();

                utils.packageNamesAndVersion = utils.readDiffFileToList("/tmp/package_diff.txt");

                utils.combinePackagesWithDatabases();
                utils.transfer();
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(args[0] + " is not a recognized option.");
                Console.ResetColor();
                Console.WriteLine("Possible options are: before, after");
                break;
        }
    }
}