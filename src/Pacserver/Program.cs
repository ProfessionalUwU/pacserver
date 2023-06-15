using Pacserver.Utils;

public class Program {
    static void Main(string[] args) {
        if (args.Length == 0) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Please specify an option.");
            Console.ResetColor();
            Console.WriteLine("Possible options are: determinePacmanCacheDirectory");
            return;
        }

        PacserverUtils utils = new PacserverUtils();

        switch (args[0]) {
            case "determinePacmanCacheDirectory":
                utils.readPacmanConfig();
                Console.WriteLine(utils.pacmanCacheDirectory);
                break;
            case "before":
                utils.readPacmanConfig();
                utils.getEveryPackageNameAndVersionViaFolderName("/tmp/before_update.txt");
                break;
            case "after":
                utils.readPacmanConfig();
                utils.getEveryPackageNameAndVersionViaFolderName("/tmp/after_update.txt");
                break;
            case "diff":
                utils.checkForNewerPackages();
                Console.WriteLine(utils.newerPackages);
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(args[0] + " is not a recognized option.");
                Console.ResetColor();
                Console.WriteLine("Possible options are: determinePacmanCacheDirectory");
                break;
        }
    }
}