using LightBDD.Framework.Parameters;
using LightBDD.TabularAttributes.Utils;

namespace LightBDD.TabularAttributes;

public static class TableFrom
{
    public static InputTable<T> Inputs<T>() => Specified.Inputs<T>().ToTable();
    public static InputTable<T> Outputs<T>() => Specified.Outputs<T>().ToTable();
}