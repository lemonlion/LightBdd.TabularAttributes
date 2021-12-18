using System.Runtime.CompilerServices;
using LightBDD.Framework.Parameters;
using LightBDD.TabularAttributes.Utils;

namespace LightBDD.TabularAttributes;

public class TableFrom
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static InputTable<T> Inputs<T>()
        => Specified.Inputs<T>().ToTable();

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static InputTable<T> Outputs<T>()
        => Specified.Outputs<T>().ToTable();
}