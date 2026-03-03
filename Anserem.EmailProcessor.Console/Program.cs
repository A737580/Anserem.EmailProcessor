using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

try 
{
    var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory()) 
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

    var serviceProvider = new ServiceCollection()
        .AddSingleton<IUserInterface, ConsoleUserInterface>()
        .AddSingleton<IEmailParser, EmailParser>()
        .AddSingleton<IEmailService, EmailService>()
        .AddSingleton<AppRunner>() 
        .Configure<EmailSettings>(config) 
        .BuildServiceProvider();

    var app = serviceProvider.GetRequiredService<AppRunner>();
    app.Run();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}