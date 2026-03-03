using System.Text;
using System.Text.RegularExpressions;

public class EmailMessage
{
    public EmailMessage() { }
    public EmailMessage(List<string>? from, List<string>? to,List<string>? copy,List<string>? blindCopy,string? title,string? body)
    {
        ValidateEmailList(from, nameof(from));
        ValidateEmailList(to, nameof(to));
        ValidateEmailList(copy, nameof(copy));
        ValidateEmailList(blindCopy, nameof(blindCopy));
        this._from = from;
        this._to = to;
        this._copy = copy;
        this._blindCopy = blindCopy;
        this._title = title;
        this._body = body;
    }
    public EmailMessage(EmailDto emailDto)
    {
        ValidateEmailList(emailDto.From, nameof(emailDto.From));
        ValidateEmailList(emailDto.To, nameof(emailDto.To));
        ValidateEmailList(emailDto.Copy, nameof(emailDto.Copy));
        ValidateEmailList(emailDto.BlindCopy, nameof(emailDto.BlindCopy));

        this._from = emailDto.From;
        this._to = emailDto.To;
        this._copy = emailDto.Copy;
        this._blindCopy = emailDto.BlindCopy;
        this._title = emailDto.Title;
        this._body = emailDto.Body;
    }
    
    List<string>? _from;
    List<string>? _to;
    List<string>? _copy;
    List<string>? _blindCopy;
    string?_title;
    string? _body;

    public List<string>? From => _from?.Select(x=>x).ToList();
    public List<string>? To => _to?.Select(x=>x).ToList();
    public List<string>? Copy => _copy?.Select(x=>x).ToList();
    public List<string>? BlindCopy => _blindCopy?.Select(x=>x).ToList();
    public string? Title => _title;
    public string? Body => _body;


    private void ValidateEmailList(List<string>? emails, string fieldName)
    {
        if (emails == null || emails.Count == 0)
            return;
            
        foreach (var email in emails)
        {
            ValidateEmail(email, fieldName);
        }
    }
    
    private void ValidateEmail(string email, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException($"Email в поле {fieldName} не может быть пустым");
        
        if (!Regex.IsMatch(email, @"^[a-zA-Z0-9.]+@[a-zA-Z]+\.[a-zA-Z]+$"))
        {
            throw new ArgumentException(
                $"Email '{email}' в поле {fieldName} имеет неверный формат." 
            );
        }
    }
    
    public EmailMessage CopyWith(List<string>? from = null, List<string>? to = null,List<string>? copy = null,List<string>? blindCopy = null,string? title = null,string? body= null)
    {
        return new EmailMessage
        (
            from = from ?? this.From,
            to = to ?? this.To,
            copy = copy ?? this.Copy,
            blindCopy = blindCopy ?? this.BlindCopy,
            title = title ?? this.Title,
            body = body ?? this.Body
        );
    }

    public override string ToString()
    {
        StringBuilder parts = new StringBuilder();
        bool isFirst = true;

        if (_from != null && _from.Any())
        {
            if (!isFirst) parts.Append(" ");
            parts.Append("From: " + string.Join("; ",_from));
            isFirst = false;
        }

        if (_to != null && _to.Any())
        {
            if (!isFirst) parts.Append(" ");
            parts.Append("To: " + string.Join("; ",_to));
            isFirst = false;
        }

        if (_copy != null && _copy.Any())
        {
            if (!isFirst) parts.Append(" ");
            parts.Append("Copy: " + string.Join("; ",_copy));
            isFirst = false;
        }

        if (_blindCopy != null && _blindCopy.Any())
        {
            if (!isFirst) parts.Append(" ");
            parts.Append("BlindCopy: " + string.Join("; ",_blindCopy));
            isFirst = false;
        }

        if (!string.IsNullOrWhiteSpace(_title))
        {
            if (!isFirst) parts.Append(" ");
            parts.Append("Title: " + _title);
            isFirst = false;
        }

        if (!string.IsNullOrWhiteSpace(_body))
        {
            if (!isFirst) parts.Append(" ");
            parts.Append("Body: " + _body);
            isFirst = false;
        }
        return parts.ToString(); 
    }
}