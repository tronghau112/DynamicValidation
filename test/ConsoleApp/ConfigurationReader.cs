using DynamicValidation;
using System.Text.Json;

namespace ConsoleApp;

public class ConfigurationReader : IConfigurationReader
{
    public List<FieldRule> GetFieldRules()
    {
        string json = File.ReadAllText("validationConfig.json");

        return JsonSerializer.Deserialize<List<FieldRule>>(json)
            ?? throw new InvalidOperationException("Failed to deserialize configuration.");
    }
}