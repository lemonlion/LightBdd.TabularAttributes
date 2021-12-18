using System.Runtime.CompilerServices;
using LightBDD.Framework.Parameters;
using LightBDD.TabularAttributes.Utils;

namespace LightBDD.TabularAttributes;

public class VerifiableTableFrom
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static VerifiableDataTable<T> Outputs<T>()
        => Specified.Outputs<T>().ToVerifiableDataTable();
}