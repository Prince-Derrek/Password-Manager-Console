using System;
using System.Collections.Generic;

namespace Pwm;

public class VaultItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = "";
    public string Url { get; set; } = "";
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Notes { get; set; } = "";

}
public class VaultData
{
    public List<VaultItem> Items { get; set; } = new();
}