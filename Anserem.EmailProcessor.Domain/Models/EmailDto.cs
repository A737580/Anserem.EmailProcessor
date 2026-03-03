public class EmailDto
{
    public List<string>? From { get; set; }
    public List<string>? To { get; set; }
    public List<string>? Copy { get; set; }
    public List<string>? BlindCopy { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
}