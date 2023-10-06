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
        
        if (parser.IsSet(nameof(parsedArgs.Session))) // int prop check
            Console.WriteLine("[!] Session is set");
        else
            Console.WriteLine("[!] Session is not set");

        if (parser.IsSet(nameof(parsedArgs.ChangeACL))) // Enum prop check
            Console.WriteLine($"[!] Enum value: {parsedArgs.ChangeACL}");
        else
            Console.WriteLine("[!] ChangeACL not set");

        if (parsedArgs.GetAdmin) // Boolean prop check
            Console.WriteLine("[!] GetAdmin is true: {0}", parsedArgs.GetAdmin);

        /// Check if ref (nullable) types are set
        
        if (parsedArgs.Interactive is not null)  // String prop check
            Console.WriteLine("[!] Interactive is set");
        else
            Console.WriteLine("[!] Interactive is not set");

#if DEBUG
        ArgParser.PrintArgsDebug(parsedArgs);
        Console.Write("Hit any key to exit...");
        Console.Read();
#endif
    }

}
