using System.Reflection;

// ReSharper disable StaticMemberInGenericType

namespace LightBDD.TabularAttributes.Utils;

internal static class ModelReflector<T>
{
    private static readonly Dictionary<int, string> NamesByIndex;

    static ModelReflector()
    {
        var typeT = typeof(T);
        var properties = typeT.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(x => x is { CanRead: true, CanWrite: true })
            .Where(x => x.GetGetMethod(true)!.IsPublic)
            .Where(x => x.GetSetMethod(true)!.IsPublic)
            .ToArray();

        NamesByIndex = properties.Select((x, i) => (i, x.Name)).ToDictionary(x => x.i, x => x.Name);
    }

    public static IEnumerable<string> GetPropNames() => NamesByIndex.Values;
}