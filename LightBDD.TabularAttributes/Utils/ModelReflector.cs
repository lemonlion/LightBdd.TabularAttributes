using System.Reflection;

// ReSharper disable StaticMemberInGenericType

namespace LightBDD.TabularAttributes.Utils;

internal static class ModelReflector<T>
{
    private static readonly ConstructorInfo? ParameterlessConstructor;
    private static readonly Dictionary<string, MethodInfo?> Setters;
    private static readonly Dictionary<int, string> NamesByIndex;

    static ModelReflector()
    {
        var typeT = typeof(T);
        ParameterlessConstructor = typeT.GetConstructor(Type.EmptyTypes);
        var properties = typeT.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(x => x.CanRead && x.CanWrite)
            .Where(x => x.GetGetMethod(true)!.IsPublic)
            .Where(x => x.GetSetMethod(true)!.IsPublic)
            .ToArray();

        Setters = properties.ToDictionary(x => x.Name, x => x.GetSetMethod());
        NamesByIndex = properties.Select((x, i) => (i, x.Name)).ToDictionary(x => x.i, x => x.Name);
    }

    public static T Construct()
        => (T)ParameterlessConstructor!.Invoke(new object?[] { });

    public static IEnumerable<string> GetPropNames()
        => NamesByIndex.Values;

    public static void SetProp(T item, string propertyName, object value, bool ignoreSpacesAndCase = true)
        => (ignoreSpacesAndCase 
            ? Setters.Single(x => SanitizeName(x.Key) == SanitizeName(propertyName)).Value 
            : Setters[propertyName])?.Invoke(item, new []{ value });

    private static string SanitizeName(string name)
        => name.Replace(" ", "").Replace("&", "and").ToLower();
}