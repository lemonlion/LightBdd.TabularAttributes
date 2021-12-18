using System.Collections.Generic;
using LightBDD.Framework.Parameters;
using LightBDD.TabularAttributes.Examples.Models;
using LightBDD.TabularAttributes.Examples.Services;
using LightBDD.XUnit2;

[assembly: LightBddScope]
namespace LightBDD.TabularAttributes.Examples.TestFeatures;

public partial class TabularTableExamplesFeature : FeatureFixture
{
    private List<PhoneResult> _results = new();
    private InputTable<PhoneSpecs>? _inputs;
        
    private void The_specified_phone_specs(InputTable<PhoneSpecs> inputs)
    {
        _inputs = inputs;
    }

    private void The_find_phone_endpoint_is_called()
    {
        var phoneSpecsEnumerator = _inputs!.GetEnumerator();

        while (phoneSpecsEnumerator.MoveNext())
            _results.Add(PhoneFindingService.FindPhone(phoneSpecsEnumerator.Current)); // In real life you would call an external endpoint
    }

    private void I_should_receive_results_matching(VerifiableDataTable<PhoneResult> expectedOutputs)
    {
        expectedOutputs.SetActual(_results);
    }
}