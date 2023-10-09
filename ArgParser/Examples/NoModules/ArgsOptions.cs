using Args;

namespace ArgParserTemplate.Modules;

/// <summary>
/// Example Class for declaring arguments to the program:
/// Each argument must be an instance prop
/// The class name does not matter.
/// </summary>
internal class ArgsOptions
{

    [Args("command", ShortName = "c", Required = true, Description = "Command to execute")]
    public string Command { get; set; }

    [Args("pid", ShortName = "p")]
    public string Pid { get; set; }
    
    [Args("session", ShortName = "s")]
    public int Session { get; set; }
    
    [Args("interactive", ShortName = "it")]
    public string Interactive { get; set; }
    
    [Args("changeacl", ShortName = "ca")]
    public AclActions ChangeACL { get; set; }

    [Args("getadmin", ShortName = "ga")]
    public bool GetAdmin { get; set; }

}

internal enum AclActions
{
    All,
    Dynamic,
    Sid,
    User
};