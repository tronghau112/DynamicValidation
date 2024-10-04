using FluentValidation.Results;

namespace DynamicValidation;

public class Validation<T>(IConfigurationReader configurationReader) where T : class
{
    private readonly IConfigurationReader _configurationReader = configurationReader;

    public async Task<List<string>> Validate(T model, string partnerName)
    {
        List<string> results = [];

        foreach (FieldRule rule in _configurationReader.GetFieldRules())
        {
            foreach (Condition condition in rule.Conditions.Where(c => c.PartnerName == partnerName))
            {
                DynamicValidation<T> validator = await DynamicValidation<T>.CreateAsync(rule.FieldName, condition);
                ValidationResult validationResult = validator.Validate(model);

                if (!validationResult.IsValid)
                {
                    results.AddRange(validationResult.Errors.Select(e => e.ErrorMessage));
                }
            }
        }

        return results;
    }
}