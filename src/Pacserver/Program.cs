using Pacserver.Utils;

public class Program {
    static void Main(string[] args) {
        if (args.Length == 0) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Please specify an option.");
            Console.ResetColor();
        } else {
            switch (args[0]) {
                case "determinePacmanCacheDirectory":
                    Console.WriteLine(PacserverUtils.determinePacmanCacheDirectory());
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Possible options are: determinePacmanCacheDirectory");
                    Console.ResetColor();
                    break;
            }
        }
    }
}