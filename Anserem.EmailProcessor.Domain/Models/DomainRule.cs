public class EmailSettings
{
    public List<DomainRule> DomainRules { get; set; } = new();
}
public class DomainRule
{
    public string Domain { get; set; } = string.Empty;
    public List<string> Exceptions { get; set; } = new();
    public List<string> Substitutions { get; set; } = new();
}