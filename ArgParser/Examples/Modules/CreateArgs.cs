using Args;

namespace ArgParserTemplate.Modules;

[Module("create", Description = "Create a new user")]
public class CreateArgs
{
    [Args("username", ShortName = "u", Description = "The name of the user", Required = true)]
    public string UserName { get; set; }

    [Args("password", ShortName = "p", Description = "Password for the user", Required = true)]
    public string Password { get; set; }
    
    [Args("age", ShortName = "a", Description = "Optional value for age")]
    public uint Age { get; set; }

    [Args("role", ShortName = "r", Description = "One option from UserRole enum")]
    public UserRole Role { get; set; }
}

public enum UserRole
{
    ReadOnly,
    User,
    Admin
}