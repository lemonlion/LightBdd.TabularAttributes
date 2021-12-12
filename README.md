# Tabular Attributes

Designed to be use with LightBdd Tables to allow for specifying input and output data for tests, in a truth table format.

## Differences between Tabular Attributes and InlineData

- InlineData doesn't work with LightBdd Tables
- InlineData doesn't clearly differentiate between inputs and outputs
- InlineData doesn't inline column names so it's difficult to know what value is referring to what variable at a glance
- InlineData doesn't allow for column/variable names with spaces
- InlineData gives long messy constructors with variables that then need to be fed in to your test
- InlineData doesn't automatically convert data into input and output classes

## Why have test inputs and outputs specified inline with attributes

- Allows a reader to see at a glance what the inputs and outputs of a test are at compile time
- Allows the reader to easily match up each input to each output at compile time

## Examples

### Standard

```c#
[Scenario]
[HeadIn("Manufacturer", "ReleaseYear", "WeightInGrams", "HasHeadphoneJack")][HeadOut("PhoneName",    "IsStillAvailable")]
[Inputs("Samsung",      2001,          191,             false             )][Outputs("Galaxy S20" ,  true              )]
[Inputs("Samsung",      2000,          189,             true              )][Outputs("Galaxy S10" ,  true              )]
[Inputs("Apple",        2020,          150,             false             )][Outputs("Iphone 10",    false             )]
[Inputs("Apple",        1986,          190,             true              )][Outputs("Iphone 2",     false             )]
public void Test_Normal()
{
	Runner.RunScenario(
		given => The_specified_phone_specs(TableFrom.Inputs<PhoneSpecs>()),
		when => The_find_phone_endpoint_is_called(),
		then => I_should_receive_results_matching(VerifiableTableFrom.Outputs<PhoneResult>()));
}
```

### With Spaces In Header Names

```c#
[Scenario]
[HeadIn("Manufacturer", "Release Year", "Weight In Grams", "Has Headphone Jack")][HeadOut("Phone Name",    "Is Still Available")]
[Inputs("Samsung",      2001,           191,               false               )][Outputs("Galaxy S20",    true                )]
[Inputs("Samsung",      2000,           189,               true                )][Outputs("Galaxy S10",    true                )]
[Inputs("Apple",        2020,           150,               false               )][Outputs("Iphone 10",     false               )]
[Inputs("Apple",        1986,           190,               true                )][Outputs("Iphone 2",      false               )]
public void Test_With_Spaces_In_Header_Names()
{
	Runner.RunScenario(
		given => The_specified_phone_specs(TableFrom.Inputs<PhoneSpecs>()),
		when => The_find_phone_endpoint_is_called(),
		then => I_should_receive_results_matching(VerifiableTableFrom.Outputs<PhoneResult>()));
}
```

### With Out Of Order Headers

```c#
[Scenario]
[HeadIn("ReleaseYear", "Manufacturer", "WeightInGrams", "HasHeadphoneJack")][HeadOut("PhoneName",    "IsStillAvailable")]
[Inputs(2001,          "Samsung",      191,             false             )][Outputs("Galaxy S20" ,  true              )]
[Inputs(2000,          "Samsung",      189,             true              )][Outputs("Galaxy S10" ,  true              )]
[Inputs(2020,          "Apple",        150,             false             )][Outputs("Iphone 10",    false             )]
[Inputs(1986,          "Apple",        190,             true              )][Outputs("Iphone 2",     false             )]
public void Test_Out_Of_Order_Headers()
{
	Runner.RunScenario(
		given => The_specified_phone_specs(TableFrom.Inputs<PhoneSpecs>()),
		when => The_find_phone_endpoint_is_called(),
		then => I_should_receive_results_matching(VerifiableTableFrom.Outputs<PhoneResult>()));
}
```
