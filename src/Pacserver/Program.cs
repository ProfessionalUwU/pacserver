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

        switch (args[0]) {
            case "determinePacmanCacheDirectory":
                PacserverUtils utils = new PacserverUtils();
                utils.readPacmanConfig();
                Console.WriteLine(utils.pacmanCacheDirectory);
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