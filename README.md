# ArgParse
Lightweight Argument Parser for C# console application

Support parsing arguments from CLI in various methods for ease of use:
```
--argFullName argvalue
-argShortName argvalue
/argName:argvalue
/argName=argvalue
```

The cli arguments are case-insensitive (Only keys, not values).
Converts the cli arg data type (string) to the desired/declared type.
