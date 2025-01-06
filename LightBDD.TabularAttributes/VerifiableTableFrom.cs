using LightBDD.Framework.Parameters;
using LightBDD.TabularAttributes.Utils;

namespace LightBDD.TabularAttributes;

public static class VerifiableTableFrom
{
    public static VerifiableDataTable<T> Outputs<T>() => Specified.Outputs<T>().ToVerifiableDataTable();
}