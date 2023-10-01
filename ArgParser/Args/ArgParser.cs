﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Args;

// ToDo:
// 1. Implement Help method to print all args name (Long/Short), description, and if required
// 2. Add support for modules (Where the first positional argument is the module name)
// 3. Add support/parsing for Properties of type Enum
// 4. Add logic for boolean properties (if exists as CLI arg without value, then true)

public class ArgParser
{
    public IEnumerable<string> RawArgs { get; }
    public Dictionary<string, string> ParsedArgsDict { get; private set; }

    public ArgParser(string[] args)
    {
        RawArgs = args;
    }

    /// <summary>
    /// Using generics to allow the user to delcare his arguments in whatever class he wants.
    /// This method parse the arguments and try to convert ther values to the value decalred in the arg class
    /// </summary>
    /// <typeparam name="TModuleArgs">The class with the arguments delcared with ArgAttribute</typeparam>
    /// <returns>A new instance of the class with all the params populated in the desired data type (Declared args whom not given in the CLI, are null or default)</returns>
    /// <exception cref="Exception">throws when the given class(TModuleArgs) does not contains any instance props with Attribute Args</exception>
    public TModuleArgs Parse<TModuleArgs>() where TModuleArgs : class, new()
    {

        // Get all properties (args) for current Module
        var props = typeof(TModuleArgs).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty).Where(m => m.IsDefined(typeof(ArgsAttribute)));


        // Error For dev (The given class have no props decorated with ArgsAttribute)
        if (props.Count() == 0)
        {
            throw new Exception(String.Format("[-] Class '{0}' does not have any members implements '{1}'", typeof(TModuleArgs).FullName, nameof(ArgsAttribute)));
        }

        ParsedArgsDict = MyParser(RawArgs);

        TModuleArgs parsedArgs = new();

        /// Check if the user supplied args, exist and declared as an ArgsAttribute
        /// Convert the argument to its corresponds (ArgsAttribute) prop type, and set value
        foreach (var prop in props)
        {
            var argAttribute = prop.GetCustomAttribute<ArgsAttribute>();
            string content = String.Empty;
            
            if (
                ParsedArgsDict.TryGetValue(argAttribute.LongName, out content) ||
                (argAttribute.ShortName is not null && ParsedArgsDict.TryGetValue(argAttribute.ShortName, out content))
                )
            {
                // Check if its a Nullable type. If is does, we need the underlying type or else we crash
                Type safeType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                try
                {
                    prop.SetValue(parsedArgs, Convert.ChangeType(content, safeType));
                }
                catch (Exception ex) when (ex is FormatException || ex is InvalidCastException) // We couldn't convert the argument to the desired type
                {
                    Console.WriteLine($"[-] Invalid value ({content}) for argument '{argAttribute.LongName}'. (Expected type: {prop.PropertyType.Name})");
                    Environment.Exit(0);
                }
                argAttribute.IsSet = true;
            }
            else
            {
                if (argAttribute.Required)
                {
                    Console.WriteLine($"[-] Required argument not given ({argAttribute.LongName})");
                    Environment.Exit(0);
                }
            }
        }

        return parsedArgs;
    }

    /// <summary>
    /// Check if the argument was actually set with a valid value, to not crash at runtime.
    /// Using "Convert.ChangeType" with primitive types (Like int) will set their value to 0 (If arg was not given, or given without value)
    /// This is not good when 0 is actually a valid value the developer expects/checks
    /// </summary>
    /// <typeparam name="Targ">The argument type</typeparam>
    /// <param name="arg">the parsed argument</param>
    /// <returns>Whether this argument was set with a value correctly</returns>
    public bool IsSet<Targ>(Targ arg) where Targ : struct // We don't need ref types as they're nullable
    {
        bool set = false;
        try
        {
            set = arg.GetType().GetCustomAttribute<ArgsAttribute>().IsSet;
        }
        catch (NullReferenceException) { }

        return set;
    }
        


    /// <summary>
    /// Parsing string[] args using regex to allow using various methods to pass arguments from cli (/arg:VALUE, /arg=VALUE, --arg VALUE, -arg VALUE)
    /// </summary>
    /// <param name="arrArgs">string array of args from cli</param>
    /// <returns>Dictionary contains the argument name as the dict key and the argument value as the dict value</returns>
    private static Dictionary<string, string> MyParser(IEnumerable<string> arrArgs)
    {
        Regex dashRx = new(@"^\-{1,2}(?<argName>[A-Za-z]{0,})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        Regex slashRx = new(@"^/(?<argName>[A-Za-z]{0,})[:=\s]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        Dictionary<string, string> parsedArgs = new(StringComparer.OrdinalIgnoreCase);
        List<string> args = arrArgs.ToList();
        
        while (args.Count() != 0)
        {
            string arg = args[0].TrimStart();
            string argValue = String.Empty;
            string argName = String.Empty;

            Match dashMatch = dashRx.Match(arg); // --argument || -arg
            Match slashMatch = slashRx.Match(arg); // /arg:VALUE || /arg=VALUE

            /// When dashMatch, the value for the arg may:
            /// 1. Not set (Boolean value - just checking if it exists)
            /// 2. The value is in the same item as the key (i.e. --user "Windy" -> Same arg)
            /// 3. The value for the current arg is in the next item (i.e. --user Windy -> 2 differnet args)
            if (dashMatch.Success)
            {
                argName = dashMatch.Groups["argName"]?.Value;

                if (args.Count == 1) /// Last arg, definitely no value in the preceeding item as there is no more items
                {
                    argValue = arg.Remove(0, dashMatch.Length);
                }
                else if (dashMatch.Value.Length == arg.Length) /// Same Length means that the next arg is maybe the value of this arg key (Or not if this is only a boolean arg)
                {
                    if (!(dashRx.IsMatch(args[1]) || slashRx.IsMatch(args[1]))) /// If next item is not arg (In our arg schema)
                    {
                        argValue = args[1];
                        args.RemoveAt(0);
                    }
                }
            }
            else if (slashMatch.Success)
            {
                argName = slashMatch.Groups["argName"]?.Value;
                argValue = arg.Remove(0, slashMatch.Length);
            }
#if DEBUG
            else
            {
                Console.WriteLine("[-] No match for arg: {0}", arg);
            }
#endif   
            if (argName != String.Empty)
                parsedArgs[argName] = argValue;

            args.RemoveAt(0);
        }

        return parsedArgs;
    }


#if DEBUG
    /// <summary>
    /// Method for debugging. List all delcared props and their value as recived from CLI
    /// </summary>
    /// <typeparam name="T">The class with the ArgAttribute arguments </typeparam>
    /// <param name="args">The parsed args</param>
    public static void PrintArgsDebug<T>(T args)
    {
        Console.WriteLine();
        var attributes = args.GetType()
           .GetProperties(BindingFlags.Instance|BindingFlags.Public|BindingFlags.GetProperty)
           .Where(t => t.IsDefined(typeof(ArgsAttribute)));
        
        var sepHeader = String.Concat(Enumerable.Repeat("=", 15 + 20 + 35 + 3));
        var header = String.Format($"{"Arg Name",-15}|{"Arg Value",-20}|{"Arg Type",-35}|\n{sepHeader}");
        Console.WriteLine(header);

        foreach (var attr in attributes)
        {
            var cols = String.Format("{0,-15}|{1,-20}|{2,-35}|", attr.Name, attr.GetValue(args) ?? "null", attr.PropertyType);
            Console.WriteLine(cols);
        }
        Console.WriteLine();
    }
#endif

}
