using Microsoft.Extensions.Options; 
public class EmailService: IEmailService
{
    private readonly EmailSettings _settings;
    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        this._settings = emailSettings.Value;
    }
    public EmailMessage ProcessEmail(EmailMessage emailMessage)
    {
        var to = emailMessage.To?.ToList() ?? new List<string>();
        var copy = emailMessage.Copy?.ToList() ?? new List<string>();

        foreach (var rule in _settings.DomainRules)
        {
            if (HasAny(to, rule.Exceptions) || HasAny(copy, rule.Exceptions))
            {
                copy = RemoveSubstitutions(copy, rule.Substitutions);
                return emailMessage.CopyWith(copy:copy);
            }

            if (HasDomain(to, rule.Domain) || HasDomain(copy, rule.Domain))
            {
                copy = AddSubstitutions(to,copy, rule.Substitutions);
            }
        }

        return emailMessage.CopyWith(copy:copy);
    }

    private bool HasDomain(List<string> emails, string domain)
    {
        return emails.Where(e => e.EndsWith($"@{domain}", StringComparison.OrdinalIgnoreCase)).ToList().Any();
    }

    private bool HasAny(List<string> emails, List<string> targets)
    {
        return emails.Intersect(targets, StringComparer.OrdinalIgnoreCase).Any();
    }

   private List<string> AddSubstitutions(List<string> to, List<string> copy, List<string> substitutions)
    {
        var result = new HashSet<string>(copy, StringComparer.OrdinalIgnoreCase);
        var existing = new HashSet<string>(to.Concat(copy), StringComparer.OrdinalIgnoreCase);

        foreach (var sub in substitutions)
        {
            if (!existing.Contains(sub))
            {
                result.Add(sub);
            }
        }

        return result.ToList();
    }

    private List<string> RemoveSubstitutions(List<string> copy, List<string> substitutions)
    {
        return copy
            .Where(c => !substitutions.Contains(c, StringComparer.OrdinalIgnoreCase))
            .ToList();
    }
}