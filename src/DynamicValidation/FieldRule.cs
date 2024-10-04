namespace DynamicValidation;

public class FieldRule
{
    public string FieldName { get; set; } = null!;
    public IEnumerable<Condition> Conditions { get; set; } = null!;
}

public class Condition
{
    public string PartnerName { get; set; } = null!;
    public string Expression { get; set; } = null!;
    public string ErrorMessage { get; set; } = string.Empty;
}