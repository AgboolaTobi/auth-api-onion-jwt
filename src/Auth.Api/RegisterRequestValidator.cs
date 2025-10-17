using Auth.Application.DTOs;
using DnsClient;
using DnsClient.Protocol;
using FluentValidation;
using System.Net.Sockets;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    private static readonly string[] CommonDomains =
        { "gmail.com", "yahoo.com", "outlook.com", "hotmail.com", "icloud.com", "proton.me" };

    private static readonly LookupClient Dns = new(new LookupClientOptions
    {
        Timeout = TimeSpan.FromSeconds(2),
        Retries = 1,
        UseCache = true
    });

    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .Must(HasDotInDomain)
                .WithMessage("Email must include a valid domain (e.g., example.com).")
            .Must(e => !DomainStartsWithWww(e))
                .WithMessage("Email domain must not start with 'www.'")
            .Must(NotCommonTypo)
                .WithMessage("Email domain looks misspelled. Did you mean a common provider?")
            .MustAsync(async (email, ct) => await HasMxAsync(email, ct))
                .WithMessage("Email domain does not accept mail (no MX record).");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);

        
        
    }

    private static bool HasDotInDomain(string email)
    {
        var at = email.IndexOf('@');
        if (at < 0 || at == email.Length - 1) return false;
        var domain = email[(at + 1)..];
        return domain.Contains('.');
    }

    private static bool DomainStartsWithWww(string email)
    {
        var at = email.IndexOf('@');
        if (at < 0 || at == email.Length - 1) return false;
        var domain = email[(at + 1)..];
        return domain.StartsWith("www.", StringComparison.OrdinalIgnoreCase);
    }

    private static bool NotCommonTypo(string email)
    {
        var at = email.IndexOf('@');
        if (at < 0 || at == email.Length - 1) return false;
        var domain = email[(at + 1)..].ToLowerInvariant();

        foreach (var known in CommonDomains)
        {
           var d = Levenshtein(domain, known);
            if (d <= 1) return d == 0;
        }
        return true;
    }

    private static async Task<bool> HasMxAsync(string email, CancellationToken ct)
    {
        var at = email.IndexOf('@');
        if (at < 0 || at == email.Length - 1) return false;
        var domain = email[(at + 1)..];

        try
        {
            var result = await Dns.QueryAsync(domain, QueryType.MX, cancellationToken: ct);
            return result.Answers.MxRecords().Any();
        }
        catch (SocketException)
        {
            return false;
        }
        catch
        {
           
            return false;
        }
    }

    
    private static int Levenshtein(string a, string b)
    {
        var n = a.Length; var m = b.Length;
        var dp = new int[n + 1, m + 1];
        for (int i = 0; i <= n; i++) dp[i, 0] = i;
        for (int j = 0; j <= m; j++) dp[0, j] = j;
        for (int i = 1; i <= n; i++)
            for (int j = 1; j <= m; j++)
            {
                var cost = a[i - 1] == b[j - 1] ? 0 : 1;
                dp[i, j] = Math.Min(
                    Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                    dp[i - 1, j - 1] + cost);
            }
        return dp[n, m];
    }
}
