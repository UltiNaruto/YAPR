using YAPR_LIB;

namespace YAPR_CLI;

public class Program
{
    public static int Main(string[] args)
    {
        string inputPath = "";
        string outputPath = "";
        string jsonPath = "";

        if (args.Length < 3)
        {
            Console.WriteLine("Insufficient arguments!");
            Console.WriteLine("Usage: ./YAPR-CLI [path-to-original-data-file] [path-to-output-data-file] [path-to-json-file]");
            return -1;
        }

        inputPath = args[0];
        outputPath = args[1];
        jsonPath = args[2];

        try {
            Patcher.Main(inputPath, outputPath, File.ReadAllText(jsonPath));
            return 0;
        } catch (Exception e) {
            Console.WriteLine(e.Message);
            if (e.StackTrace != null) {
                Console.WriteLine(e.StackTrace.ToString());
            }
            return e.HResult;
        }
    }
}