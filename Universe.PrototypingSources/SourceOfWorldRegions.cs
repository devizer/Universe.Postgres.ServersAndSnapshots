namespace Universe.PrototypingSources
{
    using System.Collections.Generic;

    public class SourceOfWorldRegions
    {
        static SourceOfWorldRegions()
        {
            foreach (WorldRegion worldRegion in WorldRegions)
            {
                foreach (WorldRegion parent in WorldRegions)
                    if (parent.Id == worldRegion.Id)
                        worldRegion.Parent = parent;

                worldRegion.CountriesWithFlag = new List<CountryWithFlag>();
                worldRegion.NotFoundCountryNames = new List<string>();
                foreach (string name in worldRegion.AllCountryNames)
                {
                    var country = SourceOfCountriesWithFlags.GetCountryByName(name);
                    if (country != null)
                        worldRegion.CountriesWithFlag.Add(country);
                    else
                        worldRegion.NotFoundCountryNames.Add(name);
                }
            }
        }


        // Three fields will be calculated by static ctor:
        //   .Parent
        //   .NotFoundCountryNames
        //   .CountriesWithFlag
        //
        // By the way, .AllCountryNames and .NotFoundCountryNames are 
        // for internal use only
        public static readonly List<WorldRegion> WorldRegions = new List<WorldRegion>(new WorldRegion[]
        {
            new WorldRegion()
            {
                // 19 of 26 countries from a list below has correspoing CountryWithFlag instance
                Id = @"/EUROPE",
                IdParent = null,
                Name = @"EUROPE",
                AllCountryNames = new List<string>(new string[]
                {
                    @"Albania", @"Andorra", @"Belarus", @"Bosnia and Herzegovina", @"Croatia", @"Faroe Islands", @"Gibraltar", @"Guerney and Alderney",
                    @"Iceland", @"Jersey", @"Kosovo", @"Liechtenstein", @"Macedonia", @"The Island of Man", @"Moldova", @"Monaco", @"Montenegro",
                    @"Norway", @"Russia", @"San Marino", @"Serbia", @"Svalbard and Jan Mayen Islands", @"Switzerland", @"Turkey", @"Ukraine",
                    @"Vatican City State"
                }),
            },
            new WorldRegion()
            {
                // 27 of 27 countries from a list below has correspoing CountryWithFlag instance
                Id = @"/EUROPE/EUROPEAN UNION",
                IdParent = @"/EUROPE",
                Name = @"EUROPEAN UNION",
                AllCountryNames = new List<string>(new string[]
                {
                    @"Austria", @"Belgium", @"Bulgaria", @"Cyprus", @"Czech Republic", @"Denmark", @"Estonia", @"Finland", @"France", @"Germany",
                    @"Greece", @"Hungary", @"Ireland", @"Italy", @"Latvia", @"Lithuania", @"Luxembourg", @"Malta", @"Netherlands", @"Poland",
                    @"Portugal", @"Romania", @"Slovakia", @"Slovenia", @"Spain", @"Sweden", @"United Kingdom"
                }),
            },
            new WorldRegion()
            {
                Id = @"/THE AMERICAS",
                IdParent = null,
                Name = @"THE AMERICAS",
                AllCountryNames = new List<string>(new string[]
                {
                }),
            },
            new WorldRegion()
            {
                // 4 of 5 countries from a list below has correspoing CountryWithFlag instance
                Id = @"/THE AMERICAS/Northern America",
                IdParent = @"/THE AMERICAS",
                Name = @"Northern America",
                AllCountryNames = new List<string>(new string[]
                {
                    @"Bermuda", @"Canada", @"Greenland", @"Saint Pierre and Miquelon", @"United States"
                }),
            },
            new WorldRegion()
            {
                // 8 of 8 countries from a list below has correspoing CountryWithFlag instance
                Id = @"/THE AMERICAS/Central America",
                IdParent = @"/THE AMERICAS",
                Name = @"Central America",
                AllCountryNames = new List<string>(new string[]
                {
                    @"Belize", @"Costa Rica", @"El Salvador", @"Guatemala", @"Honduras", @"Mexico", @"Nicaragua", @"Panama"
                }),
            },
            new WorldRegion()
            {
                // 12 of 14 countries from a list below has correspoing CountryWithFlag instance
                Id = @"/THE AMERICAS/South America",
                IdParent = @"/THE AMERICAS",
                Name = @"South America",
                AllCountryNames = new List<string>(new string[]
                {
                    @"Argentina", @"Bolivia", @"Brazil", @"Chile", @"Colombia", @"Ecuador", @"Falkland Islands (Malvinas)", @"French Guiana",
                    @"Guyana", @"Paraguay", @"Peru", @"Suriname", @"Uruguay", @"Venezuela"
                }),
            },
            new WorldRegion()
            {
                // 20 of 28 countries from a list below has correspoing CountryWithFlag instance
                Id = @"/THE AMERICAS/The Caribbean",
                IdParent = @"/THE AMERICAS",
                Name = @"The Caribbean",
                AllCountryNames = new List<string>(new string[]
                {
                    @"Anguilla", @"Antigua and Barbuda", @"Aruba", @"Bahamas", @"Barbados", @"Bonaire, Saint Eustatius and Saba",
                    @"British Virgin Islands", @"Cayman Islands", @"Cuba", @"Curaçao", @"Dominica", @"Dominican Republic", @"Grenada", @"Guadeloupe",
                    @"Haiti", @"Jamaica", @"Martinique", @"Monserrat", @"Puerto Rico", @"Saint-Barthélemy", @"Saint Kitts and Nevis", @"Saint Lucia",
                    @"Saint Martin", @"Saint Vincent and the Grenadines", @"Sint Maarten", @"Trinidad and Tobago", @"Turks and Caicos Islands",
                    @"Virgin Islands (US)"
                }),
            },
            new WorldRegion()
            {
                // 32 of 35 countries from a list below has correspoing CountryWithFlag instance
                Id = @"/ASIA",
                IdParent = null,
                Name = @"ASIA",
                AllCountryNames = new List<string>(new string[]
                {
                    @"Afghanistan", @"Armenia", @"Azerbaijan", @"Bangladesh", @"Bhutan", @"Brunei Darussalam", @"Cambodia", @"China", @"Georgia",
                    @"Hong Kong", @"India", @"Indonesia", @"Japan", @"Kazakhstan", @"North Korea", @"South Korea", @"Kyrgyzstan", @"Laos", @"Macao",
                    @"Malaysia", @"Maldives", @"Mongolia", @"Myanmar", @"Nepal", @"Pakistan", @"Philippines", @"Singapore", @"Sri Lanka", @"Taiwan",
                    @"Tajikistan", @"Thailand", @"Timor-Leste", @"Turkmenistan", @"Uzbekistan", @"Vietnam"
                }),
            },
            new WorldRegion()
            {
                Id = @"/AFRICA",
                IdParent = null,
                Name = @"AFRICA",
                AllCountryNames = new List<string>(new string[]
                {
                }),
            },
            new WorldRegion()
            {
                // 17 of 19 countries from a list below has correspoing CountryWithFlag instance
                Id = @"/AFRICA/Eastern Africa",
                IdParent = @"/AFRICA",
                Name = @"Eastern Africa",
                AllCountryNames = new List<string>(new string[]
                {
                    @"Burundi", @"Comoros", @"Djibouti", @"Eritrea", @"Ethiopia", @"Kenya", @"Madagascar", @"Malawi", @"Mauritius", @"Mayotte",
                    @"Mozambique", @"Reunion", @"Rwanda", @"Seychelles", @"Somalia", @"United Republic of Tanzania", @"Uganda", @"Zambia", @"Zimbabwe"
                }),
            },
            new WorldRegion()
            {
                // 9 of 9 countries from a list below has correspoing CountryWithFlag instance
                Id = @"/AFRICA/Middle Africa",
                IdParent = @"/AFRICA",
                Name = @"Middle Africa",
                AllCountryNames = new List<string>(new string[]
                {
                    @"Angola", @"Cameroon", @"Central African Republic", @"Chad", @"Republic of the Congo", @"Democratic Republic of the Congo",
                    @"Equatorial Guinea", @"Gabon", @"Sao Tome and Principe"
                }),
            },
            new WorldRegion()
            {
                // 5 of 8 countries from a list below has correspoing CountryWithFlag instance
                Id = @"/AFRICA/Northern Africa",
                IdParent = @"/AFRICA",
                Name = @"Northern Africa",
                AllCountryNames = new List<string>(new string[]
                {
                    @"Algeria", @"Egypt", @"Libyan Arab Jamahiriya", @"Morroco", @"South Sudan", @"Sudan", @"Tunisia", @"Western Sahara"
                }),
            },
            new WorldRegion()
            {
                // 5 of 5 countries from a list below has correspoing CountryWithFlag instance
                Id = @"/AFRICA/Southern Africa",
                IdParent = @"/AFRICA",
                Name = @"Southern Africa",
                AllCountryNames = new List<string>(new string[]
                {
                    @"Botswana", @"Lesotho", @"Namibia", @"South Africa", @"Swaziland"
                }),
            },
            new WorldRegion()
            {
                // 14 of 17 countries from a list below has correspoing CountryWithFlag instance
                Id = @"/AFRICA/Western Africa",
                IdParent = @"/AFRICA",
                Name = @"Western Africa",
                AllCountryNames = new List<string>(new string[]
                {
                    @"Benin", @"Burkina Faso", @"Cape Verde", @"Cote d'Ivoire (Ivory Coast)", @"Gambia", @"Ghana", @"Guinea", @"Guinea-Bissau",
                    @"Liberia", @"Mali", @"Mauritania", @"Niger", @"Nigeria", @"Saint Helena", @"Senegal", @"Sierra Leone", @"Togo"
                }),
            },
            new WorldRegion()
            {
                // 12 of 14 countries from a list below has correspoing CountryWithFlag instance
                Id = @"/MIDDLE EAST",
                IdParent = null,
                Name = @"MIDDLE EAST",
                AllCountryNames = new List<string>(new string[]
                {
                    @"Bahrain", @"Iraq", @"Iran", @"Israel", @"Jordan", @"Kuwait", @"Lebanon", @"Oman", @"Palestine", @"Qatar", @"Saudi Arabia",
                    @"Syria", @"United Arab Emirates", @"Yemen"
                }),
            },
            new WorldRegion()
            {
                // 11 of 15 countries from a list below has correspoing CountryWithFlag instance
                Id = @"/OCEANIA",
                IdParent = null,
                Name = @"OCEANIA",
                AllCountryNames = new List<string>(new string[]
                {
                    @"Australia", @"Fiji", @"French Polynesia", @"Guam", @"Kiribati", @"Marshall Islands", @"Micronesia", @"New Caledonia",
                    @"New Zealand", @"Papua New Guinea", @"Samoa", @"Samoa, American", @"Solomon, Islands", @"Tonga", @"Vanuatu"
                }),
            },
        });
    }
}