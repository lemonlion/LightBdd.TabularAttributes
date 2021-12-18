using System.Collections.Generic;
using System.Linq;
using TabularAttributes.Examples.Models;

namespace TabularAttributes.Examples.Services;

public static class PhoneFindingService
{
    private static readonly Dictionary<PhoneSpecs, PhoneResult> _phonesBySpecs = new()
    {
        { new PhoneSpecs { Manufacturer = "Samsung", ReleaseYear = 2001, WeightInGrams = 191, HasHeadphoneJack = false }, new PhoneResult { PhoneName = "Galaxy S20", IsStillAvailable = true } },
        { new PhoneSpecs { Manufacturer = "Samsung", ReleaseYear = 2000, WeightInGrams = 189, HasHeadphoneJack = true }, new PhoneResult { PhoneName = "Galaxy S10", IsStillAvailable = true } },
        { new PhoneSpecs { Manufacturer = "Apple", ReleaseYear = 2020, WeightInGrams = 150, HasHeadphoneJack = false }, new PhoneResult { PhoneName = "Iphone 10", IsStillAvailable = false } },
        { new PhoneSpecs { Manufacturer = "Apple", ReleaseYear = 1986, WeightInGrams = 190, HasHeadphoneJack = true }, new PhoneResult { PhoneName = "Iphone 2", IsStillAvailable = false } }
    };

    public static PhoneResult FindPhone(PhoneSpecs specs) => _phonesBySpecs.SingleOrDefault(x =>
        x.Key.HasHeadphoneJack == specs.HasHeadphoneJack &&
        x.Key.ReleaseYear == specs.ReleaseYear &&
        x.Key.Manufacturer == specs.Manufacturer &&
        x.Key.WeightInGrams == specs.WeightInGrams).Value;
}