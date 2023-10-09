using System;

namespace Args;

[AttributeUsage(AttributeTargets.Class)]
public class ModuleAttribute : Attribute
{
    public string ModuleName { get; }

    public ModuleAttribute(string moduleName)
    {
        ModuleName = moduleName;
    }

}
