using System;
using System.IO;
using System.Text;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the name of the GAME that appears in Moonlight. Include spaces and symbols. Include labels like MOONLIGHT_ in the front for more identification.");
            string gameName = Console.ReadLine();

                        string batchScript = @"@echo off
            cd C:\Program Files\Moonlight Game Streaming
            .\Moonlight.exe stream ""MY-PC-NAME"" """ + gameName + @"""";

            //string batchScript = @"start ms-settings:network-wifi";
            //string batchScript = @"start control";
            //string batchScript = @"start explorer";
            //string batchScript = @"start /min ms-settings:";
            //string batchScript = @"start ms-settings:windowsupdate";
            //string batchScript = @"start ms-settings:bluetooth";
            //string batchScript = @"start ms-settings:sound";

            // Path to the directory where you want to create the executable and batch files
            string sanitizedGameName = gameName.Replace(' ', '-');
            string outputPath = @"C:\Users\<username>\Desktop\Bat2EXE\exe";
            string exeFileName = sanitizedGameName + ".exe";
            string batchFileName = sanitizedGameName + ".bat";

            // Write the batch script to a .bat file
            File.WriteAllText(Path.Combine(outputPath, batchFileName), batchScript);

            // Create a simple C# program that executes the batch script
            string csharpCode = @"using System;
class Program
{
    static void Main(string[] args)
    {
        System.Diagnostics.Process.Start(""cmd.exe"", ""/C " + batchFileName + @""");
    }
}";

            // Compile the C# code into an executable
            var provider = new Microsoft.CSharp.CSharpCodeProvider();
            var compilerParams = new System.CodeDom.Compiler.CompilerParameters();
            compilerParams.GenerateExecutable = true;
            compilerParams.OutputAssembly = Path.Combine(outputPath, exeFileName);
            compilerParams.ReferencedAssemblies.Add("System.dll");

            var results = provider.CompileAssemblyFromSource(compilerParams, csharpCode);

            if (results.Errors.Count > 0)
            {
                foreach (var error in results.Errors)
                {
                    Console.WriteLine(error);
                }
            }
            else
            {
                Console.WriteLine("Executable created successfully: " + Path.Combine(outputPath, exeFileName));
            }
        }
    }
}
