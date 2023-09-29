using System;
using Args;

namespace ArgParserTemplate;

/// <summary>
/// Example how to use the Args namespace
/// Take the Args folder, add it to your project and consume
/// </summary>
internal class Program
{
    static void Main(string[] args)
    {
        ArgParser argParser = new(args);
        var parsedArgs = argParser.Parse<ArgsOptions>();

        Console.WriteLine("[!] Command={1}", parsedArgs.Command);
        ArgParser.PrintArgsDebug(parsedArgs);
    }
}
