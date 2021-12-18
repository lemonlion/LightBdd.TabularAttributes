namespace TabularAttributes.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class TabularAttribute : Attribute
{
    public object[] Values { get; init; }

    public TabularAttribute(params object[] values)
    {
        Values = values;
    }
}