using Microsoft.Extensions.Options;



public class EmailServiceTests
{
    private EmailService CreateService()
    {
        var settings = new EmailSettings
        {
            DomainRules = new List<DomainRule>
            {
                new DomainRule
                {
                    Domain = "tbank.ru",
                    Exceptions = new List<string> { "i.ivanov@tbank.ru" },
                    Substitutions = new List<string> { "boss@tbank.ru", "audit@tbank.ru" }
                }
            }
        };

        return new EmailService(Options.Create(settings));
    }

    [Fact]
    // Добавление адресов
    public void Should_Add_Substitutions_When_Domain_Matches()
    {
        var service = CreateService();

        var message = new EmailMessage(
            null,
            new List<string> { "user@tbank.ru" },
            new List<string> { "e.gras@tbank.ru" },
            null,
            null,
            null
        );

        var result = service.ProcessEmail(message);

        Assert.Contains("boss@tbank.ru", result.Copy!);
        Assert.Contains("audit@tbank.ru", result.Copy!);
    }

    [Fact]
    // Не добавлять при исключении
    public void Should_Not_Add_And_Remove_Substitutions_When_Exception_Found()
    {
        var service = CreateService();

        var message = new EmailMessage(
            null,
            new List<string> { "i.ivanov@tbank.ru" }, // exception
            new List<string> { "boss@tbank.ru" },
            null,
            null,
            null
        );

        var result = service.ProcessEmail(message);

        Assert.DoesNotContain("boss@tbank.ru", result.Copy!);
    }

    [Fact]
    // Не дублировать если уже есть в To
    public void Should_Not_Duplicate_Substitution_If_Already_In_To()
    {
        var service = CreateService();

        var message = new EmailMessage(
            null,
            new List<string> { "user@tbank.ru", "boss@tbank.ru" },
            new List<string>(),
            null,
            null,
            null
        );

        var result = service.ProcessEmail(message);

        Assert.Single(result.Copy!);
    }
    
    [Fact]
    // Ничего не менять если домена нет
    public void Should_Not_Change_When_No_Domain_Match()
    {
        var service = CreateService();

        var message = new EmailMessage(
            null,
            new List<string> { "user@gmail.com" },
            new List<string> { "copy@gmail.com" },
            null,
            null,
            null
        );

        var result = service.ProcessEmail(message);

        Assert.Single(result.Copy!);
        Assert.Equal("copy@gmail.com", result.Copy![0]);
    }

}

