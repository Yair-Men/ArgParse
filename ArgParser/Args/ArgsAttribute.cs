using System;

namespace Args;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public class ArgsAttribute : Attribute
{
    public string LongName { get; }
    public string ShortName { get; set; }
    public bool Required { get; set; } = false;
    public string Description { get; set; } = String.Empty;
    public bool IsSet { get; set; } = false;

    public ArgsAttribute(string longName)
    {
        LongName = longName;
    }
}
