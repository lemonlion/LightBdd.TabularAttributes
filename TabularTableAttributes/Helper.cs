using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using CsvHelper;
using LightBDD.Framework.Parameters;

namespace TabularAttributes
{
    public class TableFrom
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static InputTable<T> Inputs<T>()
            => Specified.Inputs<T>().ToTable();
    }

    public class VerifiableTableFrom
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static VerifiableDataTable<T> Outputs<T>()
            => Specified.Outputs<T>().ToVerifiableDataTable();
    }

    internal static class Reflector<T>
    {
        private static ConstructorInfo? _parameterlessConstructor { get; set; }
        private static Dictionary<string, MethodInfo?> _setters { get; set; }
        private static Dictionary<int, string> _namesByIndex { get; set; }

        static Reflector()
        {
            var typeT = typeof(T);
            _parameterlessConstructor = typeT.GetConstructor(Type.EmptyTypes);
            var properties = typeT.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(ø => ø.CanRead && ø.CanWrite)
                .Where(ø => ø.GetGetMethod(true).IsPublic)
                .Where(ø => ø.GetSetMethod(true).IsPublic)
                .ToArray();

            _setters = properties.ToDictionary(x => x.Name, x => x.GetSetMethod());
            _namesByIndex = properties.Select((x, i) => (i, x.Name)).ToDictionary(x => x.i, x => x.Name);
        }

        public static T Construct()
            => (T)_parameterlessConstructor.Invoke(new object?[] { });
        public static void SetProp(T item, string propertyName, object value, bool ignoreSpacesAndCase = true, bool compareViaString = false)
            => (ignoreSpacesAndCase 
                ? _setters.Single(x => x.Key.ToLower().Replace(" ", "") == propertyName.ToLower().Replace(" ", "")).Value 
                : _setters[propertyName])?.Invoke(item, new []{ value });
        
        public static int NumberOfProps() => _setters.Count;

        public static bool IsProp(T item, string propertyName, bool ignoreSpacesAndCase = true)
            => (ignoreSpacesAndCase
                ? _setters.Any(x => x.Key.ToLower().Replace(" ", "") == propertyName.ToLower().Replace(" ", ""))
                : _setters.ContainsKey(propertyName));
        public static string GetPropNameByIndex(int index)
            => _namesByIndex[index];
        public static IEnumerable<string> GetPropNames()
            => _namesByIndex.Values;
    }

    public class Specified
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IEnumerable<T> Inputs<T>()
            => GetValuesFromAttributes<T, HeadInAttribute, InputsAttribute>(new StackTrace(1, false));

        
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IEnumerable<T> Outputs<T>()
            => GetValuesFromAttributes<T, HeadOutAttribute, OutputsAttribute>(new StackTrace(1, false));

        public static string[] GetDefaultHeaders<T>()
        {
            return typeof(T).GetProperties().Select(x => x.Name).ToArray();
        }

        private static IEnumerable<T> GetValuesFromAttributes<T, THeaders, TValues>(StackTrace stackTrace)
            where THeaders : TabularAttribute
            where TValues : TabularAttribute
        {
            var values = GetValuesFrom<TValues>(stackTrace).ToArray();
            var headers = GetValuesFrom<THeaders>(stackTrace).FirstOrDefault()?.Select(x => x.ToString()).ToArray();
            headers ??= Reflector<T>.GetPropNames().ToArray();

            var numberOfColumns = values.First().Length;
            var numberOfRows = values.Length;

            var valuesByHeaderCollection = new List<T> { Reflector<T>.Construct() };
            for (int rowNumber = 0; rowNumber < numberOfRows; rowNumber++)
            {
                for (int columnNumber = 0; columnNumber < numberOfColumns; columnNumber++)
                {
                    var key = headers[columnNumber];
                    var value = values[rowNumber][columnNumber];

                    Reflector<T>.SetProp(valuesByHeaderCollection[rowNumber], key, value);
                }
                if (rowNumber != numberOfRows - 1)
                    valuesByHeaderCollection.Add(Reflector<T>.Construct());
            }

            return valuesByHeaderCollection;
        }

        private static IEnumerable<object[]> GetValuesFrom<T>(StackTrace stackTrace) where T : TabularAttribute
        {
            var method = GetMethodWithAttribute<T>(stackTrace);
            var inputAttributes = method?.GetCustomAttributes<T>();
            foreach (var inputAttribute in inputAttributes ?? Enumerable.Empty<T>())
                yield return inputAttribute.Values;
        }

        private static MethodBase? GetMethodWithAttribute<T>(StackTrace stackTrace) where T : Attribute
        {
            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                var method = stackTrace.GetFrame(i).GetMethod();
                var attributes = method?.GetCustomAttributes<T>();
                var containsAttribute = attributes?.Any() ?? false;
                if (containsAttribute)
                    return method;
            }

            return null;
        }
    }
}