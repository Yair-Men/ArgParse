namespace Args;

/// <summary>
/// Example Class for declaring arguments to the program:
/// Each argument must be an instance prop
/// The class name does not matter.
/// </summary>
internal class ArgsOptions
{

    [Args("command", ShortName = "c", Required = true)]
    public string Command { get; set; }

    [Args("pid", ShortName = "p")]
    public string Pid { get; set; }
    
    [Args("session", ShortName = "s")]
    public int Session { get; set; }
    
    [Args("interactive", ShortName = "it")]
    public string Interactive { get; set; }
    
    [Args("changeacl", ShortName = "ca")]
    public string ChangeACL { get; set; }

}