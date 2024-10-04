using ConsoleApp;
using DynamicValidation;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddSingleton<IConfigurationReader, ConfigurationReader>();
services.AddSingleton<Validation<Client>>();
var app = services.BuildServiceProvider();

var validationEngine = app.GetRequiredService<Validation<Client>>();

Client client = new() { Name = "John", DateOfBirth = DateTime.Now.AddYears(-15), Number = 10 };
List<string> errors = validationEngine.Validate(client, "PartnerA").Result;

foreach (string error in errors)
{
    Console.WriteLine(error);
}

Console.WriteLine("Hello, World!");
