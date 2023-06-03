public class Program {
	static void Main(string[] args) {
		if (args.Length == 0) {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Please specify an option.");
			Console.ResetColor();
		} else {
			switch (args[0]) {
				case "":
					break;
				default:
					break;
			}
		}
	}
}