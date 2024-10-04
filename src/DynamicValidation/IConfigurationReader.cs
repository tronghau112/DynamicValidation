namespace DynamicValidation;

public interface IConfigurationReader
{
    List<FieldRule> GetFieldRules();
}