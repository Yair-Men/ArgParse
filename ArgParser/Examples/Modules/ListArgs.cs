using Args;

namespace ArgParserTemplate.Modules;

[Module("list")]
public class ListArgs
{
    [Args("id", Required = true)]
    public string Id { get; set; }

    [Args("username", ShortName = "u")]
    public string UserName { get; set; }
}
