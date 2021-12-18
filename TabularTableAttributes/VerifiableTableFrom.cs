using System.Runtime.CompilerServices;
using LightBDD.Framework.Parameters;
using TabularAttributes.Utils;

namespace TabularAttributes;

public class VerifiableTableFrom
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static VerifiableDataTable<T> Outputs<T>()
        => Specified.Outputs<T>().ToVerifiableDataTable();
}