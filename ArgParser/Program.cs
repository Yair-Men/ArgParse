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
        ArgParser parser = new(args);
        var parsedArgs = parser.Parse<ArgsOptions>();

        /// Check if primitives (Non-nullable) types are set
        if (parser.IsSet(parsedArgs.Session)) 
            Console.WriteLine("[!] Session is set");
        else
            Console.WriteLine("[!] Session is not set");

        if (parser.IsSet(parsedArgs.ChangeACL))
            Console.WriteLine($"[!] Enum value: {parsedArgs.ChangeACL}");

        /// Check if ref (nullable) types are set
        if (parsedArgs.Interactive is not null) 
            Console.WriteLine("[!] Interactive is set");
        else
            Console.WriteLine("[!] Interactive is not set");


        ArgParser.PrintArgsDebug(parsedArgs);
    }

}
