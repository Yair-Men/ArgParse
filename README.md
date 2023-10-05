# ArgParse
Lightweight Argument Parser for C# console application

Support parsing arguments from CLI in various methods for ease of use:
```
--argFullName argvalue
-argShortName argvalue
/argName:argvalue
/argName=argvalue
```

The CLI arguments parsed with case-insensitive (Only keys, not values) and converted (from string) to the desired/declared type.

## Arguments check
- Non-nullable/primitives data types get the default value if not provided from the CLI (i.e. enum/int = 0)
Therefore, use the `IsSet` method on the instantiated `ArgParser` object
- Nullable/ref type can simply be checked if they are null
<br>See example in [Program.cs](ArgParser/Program.cs)
