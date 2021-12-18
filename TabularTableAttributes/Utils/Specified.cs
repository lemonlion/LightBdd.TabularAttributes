using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using TabularAttributes.Attributes;

namespace TabularAttributes.Utils;

internal static class Specified
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static IEnumerable<T> Inputs<T>()
        => GetValuesFromAttributes<T, HeadInAttribute, InputsAttribute>(new StackTrace(1, false));

        
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static IEnumerable<T> Outputs<T>()
        => GetValuesFromAttributes<T, HeadOutAttribute, OutputsAttribute>(new StackTrace(1, false));

    private static IEnumerable<T> GetValuesFromAttributes<T, THeaders, TValues>(StackTrace stackTrace)
        where THeaders : TabularAttribute
        where TValues : TabularAttribute
    {
        var values = GetValuesFrom<TValues>(stackTrace).ToArray();
        var headers = GetValuesFrom<THeaders>(stackTrace).FirstOrDefault()?.Select(x => x.ToString()).ToArray();
        headers ??= Reflector<T>.GetPropNames().ToArray();

        var numberOfColumns = values.First().Length;
        var numberOfRows = values.Length;

        var valuesByHeaderCollection = new List<T> { Reflector<T>.Construct() };
        for (int rowNumber = 0; rowNumber < numberOfRows; rowNumber++)
        {
            for (int columnNumber = 0; columnNumber < numberOfColumns; columnNumber++)
            {
                var key = headers[columnNumber];
                var value = values[rowNumber][columnNumber];

                Reflector<T>.SetProp(valuesByHeaderCollection[rowNumber], key!, value);
            }
            if (rowNumber != numberOfRows - 1)
                valuesByHeaderCollection.Add(Reflector<T>.Construct());
        }

        return valuesByHeaderCollection;
    }

    private static IEnumerable<object[]> GetValuesFrom<T>(StackTrace stackTrace) where T : TabularAttribute
    {
        var method = GetMethodWithAttribute<T>(stackTrace);
        var inputAttributes = method?.GetCustomAttributes<T>();
        foreach (var inputAttribute in inputAttributes ?? Enumerable.Empty<T>())
            yield return inputAttribute.Values;
    }

    private static MethodBase? GetMethodWithAttribute<T>(StackTrace stackTrace) where T : Attribute
    {
        for (int i = 0; i < stackTrace.FrameCount; i++)
        {
            var method = stackTrace.GetFrame(i)!.GetMethod();
            var attributes = method?.GetCustomAttributes<T>();
            var containsAttribute = attributes?.Any() ?? false;
            if (containsAttribute)
                return method;
        }

        return null;
    }
}