using System;
using System.Collections.Generic;
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
        public void Seed()
        {
            var countries = SourceOfCountriesWithFlags.Countries;
            foreach (var countryWithFlag in countries)
            {
                Db.Country.Add(new Country()
                {
                    EnglishName = countryWithFlag.Name,
                    Flag = Utf8.GetBytes(countryWithFlag.FlagAsSvg),
                });
            }

            Db.SaveChanges();
        }
    }
}
