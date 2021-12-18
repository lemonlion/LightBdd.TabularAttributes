using System.Reflection;

namespace TabularAttributes.Utils;

internal static class Reflector<T>
{
    private static readonly ConstructorInfo? ParameterlessConstructor;
    private static readonly Dictionary<string, MethodInfo?> Setters;
    private static readonly Dictionary<int, string> NamesByIndex;

    static Reflector()
    {
        var typeT = typeof(T);
        ParameterlessConstructor = typeT.GetConstructor(Type.EmptyTypes);
        var properties = typeT.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(ø => ø.CanRead && ø.CanWrite)
            .Where(ø => ø.GetGetMethod(true)!.IsPublic)
            .Where(ø => ø.GetSetMethod(true)!.IsPublic)
            .ToArray();

        Setters = properties.ToDictionary(x => x.Name, x => x.GetSetMethod());
        NamesByIndex = properties.Select((x, i) => (i, x.Name)).ToDictionary(x => x.i, x => x.Name);
    }

    public static T Construct()
        => (T)ParameterlessConstructor!.Invoke(new object?[] { });

    public static void SetProp(T item, string propertyName, object value, bool ignoreSpacesAndCase = true)
        => (ignoreSpacesAndCase 
            ? Setters.Single(x => x.Key.ToLower().Replace(" ", "") == propertyName.ToLower().Replace(" ", "")).Value 
            : Setters[propertyName])?.Invoke(item, new []{ value });

    public static IEnumerable<string> GetPropNames()
        => NamesByIndex.Values;
}