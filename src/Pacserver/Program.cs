using Pacserver.Utils;

public class Program {

    protected Program() {
    }
    static void Main(string[] args) {
        if (args.Length == 0) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Please specify an option.");
            Console.ResetColor();
            Console.WriteLine("Possible options are: before, after");
            return;
        }

        PacserverUtils utils = new PacserverUtils();
        utils.prerequisites();
        utils.readPacmanConfig();

        switch (args[0]) {
            case "before":
                utils.getEveryPackageNameAndVersion("before", utils.pacserverDirectory + "packages_before.txt");
                utils.checkIfDatabasesWereModified("before", utils.pacserverDirectory + "databases_before.txt");
                break;
            case "after":
                utils.getEveryPackageNameAndVersion("after", utils.pacserverDirectory + "packages_after.txt");
                utils.checkIfDatabasesWereModified("after", utils.pacserverDirectory + "databases_after.txt");

                utils.diff(utils.pacserverDirectory + "packages_before.txt", utils.pacserverDirectory + "packages_after.txt");
                utils.saveDiffToFile(utils.pacserverDirectory + "package_diff.txt");

                utils.diff(utils.pacserverDirectory + "databases_before.txt", utils.pacserverDirectory + "databases_after.txt");
                utils.saveDiffToFile(utils.pacserverDirectory + "database_diff.txt");
                utils.filterDiffOutputForDatabases();

                utils.packageNamesAndVersion = utils.readDiffFileToList(utils.pacserverDirectory + "package_diff.txt");

                utils.getEverySigFile();
                utils.combinePackagesWithDatabases();
                utils.transfer();

                utils.cleanup();
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