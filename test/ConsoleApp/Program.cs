using ConsoleApp;
using DynamicValidation;
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new();
services.AddSingleton<IConfigurationReader, ConfigurationReader>();
services.AddSingleton<Validation<Client>>();
ServiceProvider app = services.BuildServiceProvider();

Validation<Client> validationEngine = app.GetRequiredService<Validation<Client>>();

Client client = new() { Name = "John", DateOfBirth = DateTime.Now.AddYears(-15), Number = 10 };

List<string> errors = validationEngine.ValidateAsync(client, "PartnerA").Result;

foreach (string error in errors)
{
    Console.WriteLine(error);
}

Console.WriteLine("Hello, World!");
