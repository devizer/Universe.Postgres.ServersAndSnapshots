using NUnit.Framework;
using Universe.NUnitTests;
using Universe.PrototypingSources;

namespace ErgoFab.DataAccess.Tests;


public class SimpleSourceTests : NUnitTestsBase
{

    [Test]
    [TestCase("Warmup")]
    [TestCase("Run")]
    public void ShowWorldRegions(string id)
    {
        var worldRegions = SourceOfWorldRegions.WorldRegions;
        int regionCounter = 0;
        foreach (var worldRegion in worldRegions)
        {
            List<CountryWithFlag> countries = worldRegion.CountriesWithFlag;
            Console.WriteLine($"{++regionCounter} {worldRegion}, Countries: {countries.Count}");
            if (!AllowLongListsOutput) continue;
            var countryCounter = 0;
            foreach (var country in countries)
            {
                Console.WriteLine($" {regionCounter}.{++countryCounter} " + country.Name);
            }

            Console.WriteLine();
        }
    }

    [Test]
    [TestCase("Warmup")]
    [TestCase("Run")]
    public void ShowOrganizations(string id)
    {
        var organizations = SourceOfOrganizations.Orgs;
        Console.WriteLine($"SourceOfOrganizations.Orgs Count: {organizations.Count}");
        if (!AllowLongListsOutput) return;
        int orgCounter = 0;
        foreach (var organization in organizations)
        {
            Console.WriteLine($"{++orgCounter} {organization}");
        }
    }


}