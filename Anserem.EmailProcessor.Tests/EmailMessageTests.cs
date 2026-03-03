public class EmailMessageTests
{
    [Fact]
    // Проверка создания сообщения с валидными данными из DTO
    public void Constructor_Should_Create_Message_With_Valid_Data()
    {
        var dto = new EmailDto
        {
            To = new List<string> { "t.kogni@acl.com", "i.ivanov@tbank.ru" },
            Copy = new List<string> { "e.gras@tbank.ru" }
        };

        var message = new EmailMessage(dto);

        Assert.Equal(2, message.To!.Count);
        Assert.Single(message.Copy!);
    }

    [Fact]
    // Должен выбрасывать исключение при попытке создать сообщение с невалидным email
    public void Constructor_Should_Throw_When_Email_Invalid()
    {
        var dto = new EmailDto
        {
            To = new List<string> { "invalid-email" }
        };

        Assert.Throws<ArgumentException>(() => new EmailMessage(dto));
    }

    [Fact]
    // Проверка форматирования строки с одним адресом (без точки с запятой)
    public void ToString_Should_Format_Single_Address_Without_Semicolon()
    {
        var message = new EmailMessage(
            null,
            new List<string> { "rivet@email.com" },
            null,
            null,
            null,
            null
        );

        var result = message.ToString();

        Assert.Equal("To: rivet@email.com", result);
    }

    [Fact]
    // Проверка форматирования строки с несколькими адресами (с точкой с запятой между адресами)
    public void ToString_Should_Format_Multiple_Addresses_With_Semicolon()
    {
        var message = new EmailMessage(
            null,
            new List<string> { "a@a.com", "b@b.com", "c@c.com" },
            null,
            null,
            null,
            null
        );

        var result = message.ToString();

        Assert.Equal("To: a@a.com; b@b.com; c@c.com", result);
    }

    [Fact]
    // Проверка форматирования строки с несколькими полями (To и Copy)
    public void ToString_Should_Combine_To_And_Copy()
    {
        var message = new EmailMessage(
            null,
            new List<string> { "t.kogni@acl.com", "i.ivanov@tbank.ru" },
            new List<string> { "e.gras@tbank.ru" },
            null,
            null,
            null
        );

        var result = message.ToString();

        Assert.Equal(
            "To: t.kogni@acl.com; i.ivanov@tbank.ru Copy: e.gras@tbank.ru",
            result
        );
    }
}