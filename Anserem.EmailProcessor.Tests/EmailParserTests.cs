public class EmailParserTests
{
    private readonly EmailParser _parser = new();

    [Fact]
    // Проверка парсинга корректной строки с несколькими адресами в To и одним в Copy
    public void Parse_Should_Parse_Valid_Input()
    {
        var raw = "To: t.kogni@acl.com; i.ivanov@tbank.ru Copy: e.gras@tbank.ru";

        var dto = _parser.ParseDataToDto(raw);

        Assert.Equal(2, dto.To!.Count);
        Assert.Single(dto.Copy!);
    }

    [Fact]
    // Проверка парсинга строки только с одним полем To и одним адресом
    public void Parse_Should_Handle_Single_Address()
    {
        var raw = "To: rivet@email.com";

        var dto = _parser.ParseDataToDto(raw);

        Assert.Single(dto.To!);
        Assert.Equal("rivet@email.com", dto.To![0]);
    }

    [Fact]
    // Должен выбрасывать исключение при передаче строки несоответствующего формата
    public void Parse_Should_Throw_When_Format_Invalid()
    {
        var raw = "Invalid string without fields";

        Assert.Throws<FormatException>(() => _parser.ParseDataToDto(raw));
    }

    [Fact]
    // Должен выбрасывать исключение при наличии недопустимых символов в email
    public void Parse_Should_Throw_When_Address_Has_Invalid_Characters()
    {
        var raw = "To: test!@mail.com";

        Assert.Throws<ArgumentException>(() => _parser.ParseDataToDto(raw));
    }
}