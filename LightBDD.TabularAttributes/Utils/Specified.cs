using System.Reflection;
using System.Text.Json;
using LightBDD.Core.ExecutionContext;
using LightBDD.TabularAttributes.Attributes;

namespace LightBDD.TabularAttributes.Utils;

internal static class Specified
{
    private static readonly JsonSerializerOptions SerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public static IEnumerable<T> Inputs<T>()
        => GetValuesFromAttributes<T, HeadInAttribute, InputsAttribute>();

    public static IEnumerable<T> Outputs<T>()
        => GetValuesFromAttributes<T, HeadOutAttribute, OutputsAttribute>();

    private static IEnumerable<T> GetValuesFromAttributes<T, THeaders, TValues>()
        where THeaders : TabularAttribute
        where TValues : TabularAttribute
    {
        var values = GetValuesFrom<TValues>().ToArray();
        var headers = GetValuesFrom<THeaders>().FirstOrDefault()?.Select(x => x.ToString()).ToArray();
        headers ??= ModelReflector<T>.GetPropNames().ToArray();

        var numberOfColumns = values.First().Length;
        var numberOfRows = values.Length;

        var valuesByHeaderCollection = new List<T>();
        for (var rowNumber = 0; rowNumber < numberOfRows; rowNumber++)
        {
            var keyValues = new List<string>();
            for (var columnNumber = 0; columnNumber < numberOfColumns; columnNumber++)
            {
                var key = headers[columnNumber];
                var value = values[rowNumber][columnNumber];

                var keyValue = $"\"{SanitizeName(key!)}\":{GetJsonValue(value)}";
                keyValues.Add(keyValue);
            }

            var json = $"{{{string.Join(",", keyValues)}}}";
            var rowItem = JsonSerializer.Deserialize<T>(json, SerializerOptions);
            valuesByHeaderCollection.Add(rowItem!);
        }

        return valuesByHeaderCollection;

        string? GetJsonValue(object value) => value is string
            ? $"\"{value}\""
            : value is bool
                ? value?.ToString()?.ToLower()
                : $"{value}";
    }

    private static IEnumerable<object[]> GetValuesFrom<T>() where T : TabularAttribute
    {
        var method = ScenarioExecutionContext.CurrentScenario.Descriptor?.MethodInfo;
        var inputAttributes = method?.GetCustomAttributes<T>();
        foreach (var inputAttribute in inputAttributes ?? [])
            yield return inputAttribute.Values;
    }

    private static string SanitizeName(string name) => name.Replace(" ", "").Replace("&", "and").ToLower();
}