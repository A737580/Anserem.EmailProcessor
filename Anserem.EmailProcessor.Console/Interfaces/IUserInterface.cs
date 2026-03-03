public interface IUserInterface
{
    public string? GetDataFromUser();
    public void PrintDataToConsole(EmailMessage data);
    public void PrintMessageToConsole(string message);
    public bool AskToContinue();
}