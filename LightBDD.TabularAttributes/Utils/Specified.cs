using System.Reflection;
using LightBDD.Core.ExecutionContext;
using LightBDD.TabularAttributes.Attributes;

namespace LightBDD.TabularAttributes.Utils;

internal static class Specified
{
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

        var valuesByHeaderCollection = new List<T> { ModelReflector<T>.Construct() };
        for (int rowNumber = 0; rowNumber < numberOfRows; rowNumber++)
        {
            for (int columnNumber = 0; columnNumber < numberOfColumns; columnNumber++)
            {
                var key = headers[columnNumber];
                var value = values[rowNumber][columnNumber];

                ModelReflector<T>.SetProp(valuesByHeaderCollection[rowNumber], key!, value);
            }
            if (rowNumber != numberOfRows - 1)
                valuesByHeaderCollection.Add(ModelReflector<T>.Construct());
        }

        return valuesByHeaderCollection;
    }

    private static IEnumerable<object[]> GetValuesFrom<T>() where T : TabularAttribute
    {
        var method = ScenarioExecutionContext.CurrentScenario.Descriptor?.MethodInfo;
        var inputAttributes = method?.GetCustomAttributes<T>();
        foreach (var inputAttribute in inputAttributes ?? [])
            yield return inputAttribute.Values;
    }
}