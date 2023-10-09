#define EXAMPLE_WITH_MODULES // EXAMPLE_WITH_MODULES | EXAMPLE_WITHOUT_MODULES

using ArgParserTemplate.Modules;
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
#if EXAMPLE_WITHOUT_MODULES
        var parser = ArgParser.Init(args);

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

#elif EXAMPLE_WITH_MODULES
        var parser = ArgParser.Init(args, true);

        switch (parser.ModuleName)
        {
            case "create":
                DoCreate(parser.Parse<CreateArgs>());
                break;
            case "list":
                DoList(parser.Parse<ListArgs>());
                break;
            default:
                Console.WriteLine("What module now ?!");
                break;
        }
#endif



    }

#if EXAMPLE_WITH_MODULES
    private static void DoCreate(CreateArgs args) => Console.WriteLine("Create module execution flow");
    private static void DoList(ListArgs args) =>  Console.WriteLine("List module execution flow");

#endif

}

