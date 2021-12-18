using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using TabularAttributes.Attributes;
using TabularAttributes.Examples.Models;

namespace TabularAttributes.Examples.TestFeatures;

public partial class TabularTableExamplesFeature
{
    [Scenario]
    [HeadIn("Manufacturer", "ReleaseYear", "WeightInGrams", "HasHeadphoneJack")][HeadOut("PhoneName",  "IsStillAvailable")]
    [Inputs("Samsung",      2001,          191,             false             )][Outputs("Galaxy S20", true              )]
    [Inputs("Samsung",      2000,          189,             true              )][Outputs("Galaxy S10", true              )]
    [Inputs("Apple",        2020,          150,             false             )][Outputs("Iphone 10",  false             )]
    [Inputs("Apple",        1986,          190,             true              )][Outputs("Iphone 2",   false             )]
    public void Test_Normal()
    {
        Runner.RunScenario(
            given => The_specified_phone_specs(TableFrom.Inputs<PhoneSpecs>()),
            when => The_find_phone_endpoint_is_called(),
            then => I_should_receive_results_matching(VerifiableTableFrom.Outputs<PhoneResult>()));
    }

        
    [Scenario]
    [HeadIn("Manufacturer", "Release Year", "Weight In Grams", "Has Headphone Jack")][HeadOut("Phone Name", "Is Still Available")]
    [Inputs("Samsung",      2001,           191,               false               )][Outputs("Galaxy S20", true                )]
    [Inputs("Samsung",      2000,           189,               true                )][Outputs("Galaxy S10", true                )]
    [Inputs("Apple",        2020,           150,               false               )][Outputs("Iphone 10",  false               )]
    [Inputs("Apple",        1986,           190,               true                )][Outputs("Iphone 2",   false               )]
    public void Test_With_Spaces_In_Header_Names()
    {
        Runner.RunScenario(
            given => The_specified_phone_specs(TableFrom.Inputs<PhoneSpecs>()),
            when => The_find_phone_endpoint_is_called(),
            then => I_should_receive_results_matching(VerifiableTableFrom.Outputs<PhoneResult>()));
    }
        
    [Scenario]
    [HeadIn("ReleaseYear", "Manufacturer", "WeightInGrams", "HasHeadphoneJack")][HeadOut("PhoneName",  "IsStillAvailable")]
    [Inputs(2001,          "Samsung",      191,             false             )][Outputs("Galaxy S20", true              )]
    [Inputs(2000,          "Samsung",      189,             true              )][Outputs("Galaxy S10", true              )]
    [Inputs(2020,          "Apple",        150,             false             )][Outputs("Iphone 10",  false             )]
    [Inputs(1986,          "Apple",        190,             true              )][Outputs("Iphone 2",   false             )]
    public void Test_Out_Of_Order_Headers()
    {
        Runner.RunScenario(
            given => The_specified_phone_specs(TableFrom.Inputs<PhoneSpecs>()),
            when => The_find_phone_endpoint_is_called(),
            then => I_should_receive_results_matching(VerifiableTableFrom.Outputs<PhoneResult>()));
    }
}