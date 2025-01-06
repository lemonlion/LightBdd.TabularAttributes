namespace LightBDD.TabularAttributes.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class TabularAttribute(params object[] values) : Attribute
{
    public object[] Values { get; } = values;
}