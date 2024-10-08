using FluentValidation;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;

namespace DynamicValidation;

public class DynamicValidation<T> : AbstractValidator<T> where T : class
{
    private static ScriptOptions? _cachedScriptOptions;

    public static async Task<DynamicValidation<T>> CreateAsync(string fieldName, Condition condition)
    {
        DynamicValidation<T> validator = new();
        await validator.InitializeAsync(fieldName, condition);

        return validator;
    }

    private async Task InitializeAsync(string fieldName, Condition condition)
    {
        PropertyInfo propertyInfo = typeof(T).GetProperty(fieldName)! ?? throw new InvalidOperationException($"Property {fieldName} not found on {typeof(T).Name}.");
        Delegate conditionDelegate = await CompileCondition(propertyInfo.PropertyType, condition.Expression);

        RuleFor(x => propertyInfo.GetValue(x, null))
            .Must((_, value) => (bool)conditionDelegate.DynamicInvoke(value)!)
            .WithMessage(condition.ErrorMessage);

    }

    private static async Task<Delegate> CompileCondition(Type propertyType, string expression)
    {
        string func = $"new Func<{propertyType.FullName}, bool>({expression})";
        ScriptOptions options = GetScriptOptionsFromSolution();

        return await CSharpScript.EvaluateAsync<Delegate>(func, options);
    }

    public static ScriptOptions GetScriptOptionsFromSolution()
    {
        if (_cachedScriptOptions is not null)
        {
            return _cachedScriptOptions;
        }

        Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        ScriptOptions options = ScriptOptions.Default;

        foreach (Assembly assembly in loadedAssemblies)
        {
            if (assembly.IsDynamic)
            {
                continue;
            }

            options = options.AddReferences(assembly.ManifestModule.Name);
        }

        options = options.AddImports("System", "System.Linq", "System.Collections.Generic");
        _cachedScriptOptions = options;

        return _cachedScriptOptions;
    }
}