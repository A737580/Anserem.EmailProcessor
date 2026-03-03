using Microsoft.Extensions.Options;

public class AppRunner
{
    IUserInterface _userInterface;
    IEmailParser _emailParser;
    IEmailService _emailService;
    public AppRunner(IUserInterface userInterface, IEmailParser emailParser, IEmailService emailService)
    {
        this._userInterface = userInterface;
        this._emailParser = emailParser;
        this._emailService = emailService;
    }
    public void Run()
    {
        while (true)
        {
            try
            {
                string? data = _userInterface.GetDataFromUser();
                
                if (data is null)
                {
                    _userInterface.PrintMessageToConsole("Выход из программы.");
                    return;
                }
                
                EmailDto emailDto = _emailParser.ParseDataToDto(data!);
                
                EmailMessage emailMessage = new EmailMessage(emailDto);
                
                var processedEmail = _emailService.ProcessEmail(emailMessage);
                
                _userInterface.PrintDataToConsole(processedEmail);

                if (!_userInterface.AskToContinue())
                {
                    _userInterface.PrintMessageToConsole("Выход из программы.");
                    return;
                }
                    
            }
            catch (Exception ex)
            {
                _userInterface.PrintMessageToConsole($"\nОшибка: {ex.Message}");
                _userInterface.PrintMessageToConsole("Пожалуйста, попробуйте снова.\n\n\n");
            }
        }
    }   
}