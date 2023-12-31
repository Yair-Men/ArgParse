﻿using Args;

namespace ArgParserTemplate.Modules;

[Module("list", Description = "List all users")]
public class ListArgs
{
    [Args("id", Required = true)]
    public uint Id { get; set; }

    [Args("username", ShortName = "u")]
    public string UserName { get; set; }
}
