
public class ConsoleUserInterface : IUserInterface
{
    public string? GetDataFromUser()
    {
        string? input;
        Console.WriteLine("Запущено консольное приложение. Оно ожидает ввод строки. Для выхода напишите exit.");

        Console.WriteLine("Введите строку данных: ");
        while (true)
        {
            input = Console.ReadLine();
            if(string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Строка должна быть не пустая.");
                continue;
            }
            else if(input.Trim().ToLower() == "exit")
            {
                Console.WriteLine("Завершение работы...");
                return null;
            }
            
            Console.WriteLine("Ввод принят.");
            return input;
        }
    }
    public bool AskToContinue()
    {
        while (true)
        {
            Console.WriteLine("Хотите отправить еще одно письмо? (да/нет): ");
            string? answer = Console.ReadLine()?.Trim().ToLower();
            
            if (answer == "да" || answer == "yes" || answer == "y" || answer == "д")
                return true;
            
            if (answer == "нет" || answer == "no" || answer == "n" || answer == "н" || answer == "exit" || answer == "выход")
                return false;
            
            Console.WriteLine("Пожалуйста, введите 'да' или 'нет'");
        }
    }

    public void PrintDataToConsole(EmailMessage data)
    {
        Console.WriteLine("Вывод на консоль:\n" + data);
    }
    public void PrintMessageToConsole(string message)
    {
        Console.WriteLine(message);
    }
}