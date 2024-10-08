﻿using System.Text.Json;
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
            Patcher.Main(inputPath, outputPath, jsonPath);

            var randomizerConfig = JsonSerializer.Deserialize<RandomizerConfig>(File.ReadAllText(jsonPath));
            var inputDir = Directory.GetParent(inputPath);
            var outputDir = Directory.GetParent(outputPath);

            // Copy original files to randomized game
            var filesToCopy = new string[] {
                "8bitoperator_jve.ttf",
                "audiogroup1.dat",
                "audiogroup2.dat",
                "DialogModule.dll",
                "execute_shell_simple.dll",
                "GMS-WinDev.dll",
                "Metroid Planets v1.27g.exe",
                "options.ini"
            };
                foreach (var file in filesToCopy)
                    if (File.Exists(Path.Combine(inputDir.FullName, file)))
                    {
                        if (file == "Metroid Planets v1.27g.exe")
                        {
                            if (randomizerConfig.LevelData.Room == "rm_Zebeth")
                                File.Copy(Path.Combine(inputDir.FullName, file), Path.Combine(outputDir.FullName, "Planets_Zebeth.exe"), true);
                            if (randomizerConfig.LevelData.Room == "rm_Novus")
                                File.Copy(Path.Combine(inputDir.FullName, file), Path.Combine(outputDir.FullName, "Planets_Novus.exe"), true);
                        }
                        else
                            File.Copy(Path.Combine(inputDir.FullName, file), Path.Combine(outputDir.FullName, file), true);
                    }
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