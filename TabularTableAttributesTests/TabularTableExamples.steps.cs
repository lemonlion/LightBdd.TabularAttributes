using System.Collections.Generic;
using System.Threading.Tasks;
using LightBDD.Framework.Parameters;
using LightBDD.XUnit2;

[assembly: LightBddScope]
namespace TabularAttributes.Examples
{
    public partial class TabularTableExamples : FeatureFixture
    {
        private List<PhoneResult> _results = new();
        private InputTable<PhoneSpecs> _inputs;
        
        private Task The_specified_phone_specs(InputTable<PhoneSpecs> inputs)
        {
            _inputs = inputs;

            return Task.CompletedTask;
        }

        private Task The_find_phone_endpoint_is_called()
        {
            var phoneSpecsEnumerator = _inputs.GetEnumerator();

            while (phoneSpecsEnumerator.MoveNext())
                _results.Add(PhoneFindingService.FindPhone(phoneSpecsEnumerator.Current)); // In real life you would call an external endpoint

            return Task.CompletedTask;
        }

        private Task I_should_receive_results_matching(VerifiableDataTable<PhoneResult> expectedOutputs)
        {
            expectedOutputs.SetActual(_results);
            return Task.CompletedTask;
        }
    }
}