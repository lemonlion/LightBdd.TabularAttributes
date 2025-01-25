using System.Collections.Generic;
using System.Linq;
using LightBDD.TabularAttributes.Examples.Models;

namespace LightBDD.TabularAttributes.Examples.Services;

public static class PhoneFindingService
{
    private static readonly Dictionary<PhoneSpecs, PhoneResult> PhonesBySpecs = new()
    {
        { new PhoneSpecs ("Samsung", 2001, 191, false), new PhoneResult ("Galaxy S20", true) },
        { new PhoneSpecs ("Samsung", 2000, 189, true), new PhoneResult("Galaxy S10", true) },
        { new PhoneSpecs ("Apple", 2020, 150, false), new PhoneResult("Iphone 10", false) },
        { new PhoneSpecs ("Apple", 1986, 190, true), new PhoneResult("Iphone 2", false) }
    };

    public static PhoneResult FindPhone(PhoneSpecs specs) => PhonesBySpecs.SingleOrDefault(x =>
        x.Key.HasHeadphoneJack == specs.HasHeadphoneJack &&
        x.Key.ReleaseYear == specs.ReleaseYear &&
        x.Key.Manufacturer == specs.Manufacturer &&
        x.Key.WeightInGrams == specs.WeightInGrams).Value;
}