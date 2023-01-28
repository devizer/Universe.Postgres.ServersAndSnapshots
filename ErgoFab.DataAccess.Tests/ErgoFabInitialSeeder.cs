using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErgoFab.DataAccess.Model;
using SampleDb.Entities;
using Universe.PrototypingSources;

namespace ErgoFab.DataAccess.Tests
{
    public class ErgoFabInitialSeeder
    {
        public readonly ErgoFabDbContext Db;

        public ErgoFabInitialSeeder(ErgoFabDbContext db)
        {
            Db = db;
        }

        private static UTF8Encoding Utf8 = new UTF8Encoding(false);
        public void Seed(int maxCountries = Int32.MaxValue, int maxOrganizations = Int32.MaxValue, int maxEmployeesPerOrg = 4)
        {
            Stopwatch startSeedAt = Stopwatch.StartNew();
            Random random = new Random(42);
            // Countries
            var countriesSources = SourceOfCountriesWithFlags.Countries;
            int nCountries = 0;
            foreach (var countryWithFlag in countriesSources)
            {
                Db.Country.Add(new Country()
                {
                    EnglishName = countryWithFlag.Name,
                    Flag = Utf8.GetBytes(countryWithFlag.FlagAsSvg),
                });
                if (++nCountries >= maxCountries) break;
            }
            Console.WriteLine($"Saving {nCountries} countries");
            Db.SaveChanges();
            var countries = Db.Country.ToList();

            // Organizations
            var organizationSource = SourceOfOrganizations.Orgs;
            int nOrganizations = 0;
            foreach (var org in organizationSource.Where(x => x.IsLeaf))
            {
                var country = countries[random.Next(countries.Count)];
                Db.Organization.Add(new Organization()
                {
                    CountryId = country.Id,
                    Title = org.Name,
                });
                if (++nOrganizations >= maxOrganizations) break;
            }
            Console.WriteLine($"Saving {nOrganizations} organizations");
            Db.SaveChanges();
            var orgs = Db.Organization.ToList();

            // Employees
            int totalNewEmployees = 0;
            foreach (var org in orgs)
            {
                for (int i = 1; i <= maxEmployeesPerOrg; i++)
                {
                    totalNewEmployees++;
                    var surname = SourceOfSurnames.Surnames[random.Next(SourceOfSurnames.Surnames.Count)].FamilyName;
                    var name = SourceOfNames.Names[random.Next(SourceOfNames.Names.Count)].Name;
                    var country = countries[random.Next(countries.Count)];
                    Db.Employee.Add(new Employee()
                    {
                        Name = name,
                        Surname = surname,
                        CountryId = country.Id,
                        OrganizationId = org.Id,
                    });
                }
            }
            Console.WriteLine($"Saving {totalNewEmployees} employees");
            Db.SaveChanges();
            var employees = Db.Employee.ToList();

            int directorsCount = 0;
            foreach (var org in orgs.Where(x => x.Id % 2 == 1))
            {
                directorsCount++;
                var director = employees[random.Next(employees.Count)];
                org.DirectorId = director.Id;
            }
            Console.WriteLine($"Saving {directorsCount} org's directors");
            Db.SaveChanges();

            Console.WriteLine($"Seeding took {startSeedAt.ElapsedMilliseconds} milliseconds");
        }
    }
}
