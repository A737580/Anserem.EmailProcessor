using System.Text.RegularExpressions;

public class EmailParser : IEmailParser
{
    public EmailDto ParseDataToDto(string rawData)
    {
        var dto = new EmailDto();

        string pattern = @"(From|To|Copy|BlindCopy|Title|Body):\s*(.*?)(?=\s*(?:From|To|Copy|BlindCopy|Title|Body):|$)";

        if (!Regex.IsMatch(rawData, pattern, RegexOptions.Singleline))
        {
            throw new FormatException(
                $"Строка не соответствует ожидаемому формату.\n" +
                $"Ожидаемый формат: Поле1: значение1 Поле2: значение2 ...\n" +
                $"Допустимые поля: From, To, Copy, BlindCopy, Title, Body\n" +
                $"Получено: '{rawData}'"
            );
        }

        var matches = Regex.Matches(rawData, pattern, RegexOptions.Singleline);

        foreach (Match match in matches)
        {
            string key = match.Groups[1].Value.ToLower();
            string value = match.Groups[2].Value.Trim();

            switch (key)
            {
                case "from":
                    dto.From = SplitAddressesToList(value);
                    break;
                case "to":
                    dto.To = SplitAddressesToList(value);
                    break;
                case "copy":
                    dto.Copy = SplitAddressesToList(value);
                    break;
                case "blindcopy":
                    dto.BlindCopy = SplitAddressesToList(value);
                    break;
                case "title":
                    dto.Title = value;
                    break;
                case "body":
                    dto.Body = value;
                    break;
            }
        }

        return dto;
    }

private List<string> SplitAddressesToList(string raw)
{
    if (string.IsNullOrWhiteSpace(raw)) return new List<string>();
    
    return raw.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
              .Select(address => ValidateAddress(address))
              .Where(address => !string.IsNullOrWhiteSpace(address))
              .ToList();
}

private string ValidateAddress(string address)
{
    string invalidCharacters= @"[^a-zA-Zа-яА-Я0-9.@]";

    if (string.IsNullOrWhiteSpace(address)) 
        return string.Empty;
    
    if (Regex.IsMatch(address, invalidCharacters))
    {
        throw new ArgumentException($"Адрес '{address}' содержит недопустимые символы.");
    }
    
    return address;
}
}