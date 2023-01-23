namespace Universe.PrototypingSources
{
    using System.Collections.Generic;

    public class SourceOfCountriesWithFlags
    {
        static readonly Dictionary<string, CountryWithFlag> CountriesByName;

        static SourceOfCountriesWithFlags()
        {
            CountriesByName = new Dictionary<string, CountryWithFlag>();
            foreach (var country in Countries)
            {
                CountriesByName[country.Name.ToLower()] = country;
            }
        }

        public static CountryWithFlag GetCountryByName(string name)
        {
            CountryWithFlag ret;
            CountriesByName.TryGetValue(name.ToLower(), out ret);
            return ret;
        }

        public static IList<CountryWithFlag> Countries = new List<CountryWithFlag>(new CountryWithFlag[]
        {
            new CountryWithFlag()
            {
                // flag is 1127 bytes
                Name = @"European Union",
                WikiUrl = @"/wiki/European_Union",
                GDP = 18518430,
                FlagKey = @"Flag_of_Europe",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/b/b7/Flag_of_Europe.svg?download",
                FlagWidth = 810,
                FlagHeight = 540,
            },
            new CountryWithFlag()
            {
                // flag is 899 bytes
                Name = @"United States",
                WikiUrl = @"/wiki/United_States",
                GDP = 18036648,
                FlagKey = @"Flag_of_The_United_States",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/en/a/a4/Flag_of_the_United_States.svg?download",
                FlagWidth = 1235,
                FlagHeight = 650,
            },
            new CountryWithFlag()
            {
                // flag is 688 bytes
                Name = @"China",
                WikiUrl = @"/wiki/China",
                GDP = 11158457,
                FlagKey = @"Flag_of_The_Peoples_Republic_of_China",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/f/fa/Flag_of_the_People's_Republic_of_China.svg?dow" +
                             @"nload",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 209 bytes
                Name = @"Japan",
                WikiUrl = @"/wiki/Japan",
                GDP = 4383076,
                FlagKey = @"Flag_of_Japan",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/en/9/9e/Flag_of_Japan.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 502 bytes
                Name = @"Germany",
                WikiUrl = @"/wiki/Germany",
                GDP = 3363600,
                FlagKey = @"Flag_of_Germany",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/en/b/ba/Flag_of_Germany.svg?download",
                FlagWidth = 1000,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 522 bytes
                Name = @"United Kingdom",
                WikiUrl = @"/wiki/United_Kingdom",
                GDP = 2858003,
                FlagKey = @"Flag_of_The_United_Kingdom",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/en/a/ae/Flag_of_the_United_Kingdom.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 377 bytes
                Name = @"France",
                WikiUrl = @"/wiki/France",
                GDP = 2418945,
                FlagKey = @"Flag_of_France",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/en/c/c3/Flag_of_France.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 849 bytes
                Name = @"India",
                WikiUrl = @"/wiki/India",
                GDP = 2116239,
                FlagKey = @"Flag_of_India",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/en/4/41/Flag_of_India.svg?download",
                FlagWidth = 1350,
                FlagHeight = 900,
            },
            new CountryWithFlag()
            {
                // flag is 273 bytes
                Name = @"Italy",
                WikiUrl = @"/wiki/Italy",
                GDP = 1821580,
                FlagKey = @"Flag_of_Italy",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/en/0/03/Flag_of_Italy.svg?download",
                FlagWidth = 1500,
                FlagHeight = 1000,
            },
            new CountryWithFlag()
            {
                // flag is 5325 bytes
                Name = @"Brazil",
                WikiUrl = @"/wiki/Brazil",
                GDP = 1772591,
                FlagKey = @"Flag_of_Brazil",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/en/0/05/Flag_of_Brazil.svg?download",
                FlagWidth = 720,
                FlagHeight = 504,
            },
            new CountryWithFlag()
            {
                // flag is 1423 bytes
                Name = @"Canada",
                WikiUrl = @"/wiki/Canada",
                GDP = 1552807,
                FlagKey = @"Flag_of_Canada",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/en/c/cf/Flag_of_Canada.svg?download",
                FlagWidth = 1000,
                FlagHeight = 500,
            },
            new CountryWithFlag()
            {
                // flag is 749 bytes
                Name = @"South Korea",
                WikiUrl = @"/wiki/South_Korea",
                GDP = 1377873,
                FlagKey = @"Flag_of_South_Korea",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/0/09/Flag_of_South_Korea.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 265 bytes
                Name = @"Russia",
                WikiUrl = @"/wiki/Russia",
                GDP = 1326016,
                FlagKey = @"Flag_of_Russia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/en/f/f3/Flag_of_Russia.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 2373 bytes
                Name = @"Australia",
                WikiUrl = @"/wiki/Australia",
                GDP = 1230859,
                FlagKey = @"Flag_of_Australia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/en/b/b9/Flag_of_Australia.svg?download",
                FlagWidth = 1280,
                FlagHeight = 640,
            },
            new CountryWithFlag()
            {
                // flag is 60255 bytes
                Name = @"Spain",
                WikiUrl = @"/wiki/Spain",
                GDP = 1192955,
                FlagKey = @"Flag_of_Spain",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/en/9/9a/Flag_of_Spain.svg?download",
                FlagWidth = 750,
                FlagHeight = 500,
            },
            new CountryWithFlag()
            {
                // flag is 224945 bytes
                Name = @"Mexico",
                WikiUrl = @"/wiki/Mexico",
                GDP = 1140724,
                FlagKey = @"Flag_of_Mexico",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/f/fc/Flag_of_Mexico.svg?download",
                FlagWidth = 840,
                FlagHeight = 480,
            },
            new CountryWithFlag()
            {
                // flag is 218 bytes
                Name = @"Indonesia",
                WikiUrl = @"/wiki/Indonesia",
                GDP = 861933,
                FlagKey = @"Flag_of_Indonesia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/9/9f/Flag_of_Indonesia.svg?download",
                FlagWidth = 450,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 223 bytes
                Name = @"Netherlands",
                WikiUrl = @"/wiki/Netherlands",
                GDP = 750318,
                FlagKey = @"Flag_of_The_Netherlands",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/2/20/Flag_of_the_Netherlands.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 397 bytes
                Name = @"Turkey",
                WikiUrl = @"/wiki/Turkey",
                GDP = 717887,
                FlagKey = @"Flag_of_Turkey",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/b/b4/Flag_of_Turkey.svg?download",
                FlagWidth = 1200,
                FlagHeight = 800,
            },
            new CountryWithFlag()
            {
                // flag is 281 bytes
                Name = @"Switzerland",
                WikiUrl = @"/wiki/Switzerland",
                GDP = 670789,
                FlagKey = @"Flag_of_Switzerland",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/f/f3/Flag_of_Switzerland.svg?download",
                FlagWidth = 1000,
                FlagHeight = 1000,
            },
            new CountryWithFlag()
            {
                // flag is 26154 bytes
                Name = @"Saudi Arabia",
                WikiUrl = @"/wiki/Saudi_Arabia",
                GDP = 653219,
                FlagKey = @"Flag_of_Saudi_Arabia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/0/0d/Flag_of_Saudi_Arabia.svg?download",
                FlagWidth = 750,
                FlagHeight = 500,
            },
            new CountryWithFlag()
            {
                // flag is 286 bytes
                Name = @"Sweden",
                WikiUrl = @"/wiki/Sweden",
                GDP = 571090,
                FlagKey = @"Flag_of_Sweden",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/en/4/4c/Flag_of_Sweden.svg?download",
                FlagWidth = 1600,
                FlagHeight = 1000,
            },
            new CountryWithFlag()
            {
                // flag is 242 bytes
                Name = @"Nigeria",
                WikiUrl = @"/wiki/Nigeria",
                GDP = 568499,
                FlagKey = @"Flag_of_Nigeria",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/7/79/Flag_of_Nigeria.svg?download",
                FlagWidth = 800,
                FlagHeight = 400,
            },
            new CountryWithFlag()
            {
                // flag is 244 bytes
                Name = @"Poland",
                WikiUrl = @"/wiki/Poland",
                GDP = 544959,
                FlagKey = @"Flag_of_Poland",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/en/1/12/Flag_of_Poland.svg?download",
                FlagWidth = 1280,
                FlagHeight = 800,
            },
            new CountryWithFlag()
            {
                // flag is 4994 bytes
                Name = @"Argentina",
                WikiUrl = @"/wiki/Argentina",
                GDP = 543490,
                FlagKey = @"Flag_of_Argentina",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/1/1a/Flag_of_Argentina.svg?download",
                FlagWidth = 800,
                FlagHeight = 500,
            },
            new CountryWithFlag()
            {
                // flag is 257 bytes
                Name = @"Belgium",
                WikiUrl = @"/wiki/Belgium",
                GDP = 531547,
                FlagKey = @"Flag_of_Belgium_(civil)",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/9/92/Flag_of_Belgium_(civil).svg?download",
                FlagWidth = 450,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 939 bytes
                Name = @"Venezuela",
                WikiUrl = @"/wiki/Venezuela",
                GDP = 509968,
                FlagKey = @"Flag_of_Venezuela",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/0/06/Flag_of_Venezuela.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 543 bytes
                Name = @"Norway",
                WikiUrl = @"/wiki/Norway",
                GDP = 500519,
                FlagKey = @"Flag_of_Norway",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/d/d9/Flag_of_Norway.svg?download",
                FlagWidth = 1100,
                FlagHeight = 800,
            },
            new CountryWithFlag()
            {
                // flag is 216 bytes
                Name = @"Austria",
                WikiUrl = @"/wiki/Austria",
                GDP = 436888,
                FlagKey = @"Flag_of_Austria",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/4/41/Flag_of_Austria.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 1439 bytes
                Name = @"Iran",
                WikiUrl = @"/wiki/Iran",
                GDP = 425326,
                FlagKey = @"Flag_of_Iran",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/c/ca/Flag_of_Iran.svg?download",
                FlagWidth = 630,
                FlagHeight = 360,
            },
            new CountryWithFlag()
            {
                // flag is 273 bytes
                Name = @"Thailand",
                WikiUrl = @"/wiki/Thailand",
                GDP = 404824,
                FlagKey = @"Flag_of_Thailand",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/a/a9/Flag_of_Thailand.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 326 bytes
                Name = @"United Arab Emirates",
                WikiUrl = @"/wiki/United_Arab_Emirates",
                GDP = 399451,
                FlagKey = @"Flag_of_The_United_Arab_Emirates",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/c/cb/Flag_of_the_United_Arab_Emirates.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 261 bytes
                Name = @"Colombia",
                WikiUrl = @"/wiki/Colombia",
                GDP = 377740,
                FlagKey = @"Flag_of_Colombia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/2/21/Flag_of_Colombia.svg?download",
                FlagWidth = 450,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 643 bytes
                Name = @"South Africa",
                WikiUrl = @"/wiki/South_Africa",
                GDP = 349819,
                FlagKey = @"Flag_of_South_Africa",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/a/af/Flag_of_South_Africa.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 277 bytes
                Name = @"Denmark",
                WikiUrl = @"/wiki/Denmark",
                GDP = 346119,
                FlagKey = @"Flag_of_Denmark",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/9/9c/Flag_of_Denmark.svg?download",
                FlagWidth = 370,
                FlagHeight = 280,
            },
            new CountryWithFlag()
            {
                // flag is 1997 bytes
                Name = @"Malaysia",
                WikiUrl = @"/wiki/Malaysia",
                GDP = 326933,
                FlagKey = @"Flag_of_Malaysia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/6/66/Flag_of_Malaysia.svg?download",
                FlagWidth = 2800,
                FlagHeight = 1400,
            },
            new CountryWithFlag()
            {
                // flag is 1041 bytes
                Name = @"Singapore",
                WikiUrl = @"/wiki/Singapore",
                GDP = 307872,
                FlagKey = @"Flag_of_Singapore",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/4/48/Flag_of_Singapore.svg?download",
                FlagWidth = 4320,
                FlagHeight = 2880,
            },
            new CountryWithFlag()
            {
                // flag is 1948 bytes
                Name = @"Israel",
                WikiUrl = @"/wiki/Israel",
                GDP = 305673,
                FlagKey = @"Flag_of_Israel",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/d/d4/Flag_of_Israel.svg?download",
                FlagWidth = 660,
                FlagHeight = 480,
            },
            new CountryWithFlag()
            {
                // flag is 1787 bytes
                Name = @"Philippines",
                WikiUrl = @"/wiki/Philippines",
                GDP = 290896,
                FlagKey = @"Flag_of_The_Philippines",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/9/99/Flag_of_the_Philippines.svg?download",
                FlagWidth = 900,
                FlagHeight = 450,
            },
            new CountryWithFlag()
            {
                // flag is 23525 bytes
                Name = @"Egypt",
                WikiUrl = @"/wiki/Egypt",
                GDP = 282242,
                FlagKey = @"Flag_of_Egypt",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/f/fe/Flag_of_Egypt.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 1279 bytes
                Name = @"Hong Kong",
                WikiUrl = @"/wiki/Hong_Kong",
                GDP = 274027,
                FlagKey = @"Flag_of_Hong_Kong",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/5/5b/Flag_of_Hong_Kong.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 275 bytes
                Name = @"Finland",
                WikiUrl = @"/wiki/Finland",
                GDP = 272217,
                FlagKey = @"Flag_of_Finland",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/b/bc/Flag_of_Finland.svg?download",
                FlagWidth = 1800,
                FlagHeight = 1100,
            },
            new CountryWithFlag()
            {
                // flag is 609 bytes
                Name = @"Chile",
                WikiUrl = @"/wiki/Chile",
                GDP = 258062,
                FlagKey = @"Flag_of_Chile",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/7/78/Flag_of_Chile.svg?download",
                FlagWidth = 1500,
                FlagHeight = 1000,
            },
            new CountryWithFlag()
            {
                // flag is 707 bytes
                Name = @"Pakistan",
                WikiUrl = @"/wiki/Pakistan",
                GDP = 251255,
                FlagKey = @"Flag_of_Pakistan",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/3/32/Flag_of_Pakistan.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 282 bytes
                Name = @"Ireland",
                WikiUrl = @"/wiki/Republic_of_Ireland",
                GDP = 250814,
                FlagKey = @"Flag_of_Ireland",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/4/45/Flag_of_Ireland.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 284 bytes
                Name = @"Greece",
                WikiUrl = @"/wiki/Greece",
                GDP = 235574,
                FlagKey = @"Flag_of_Greece",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/5/5c/Flag_of_Greece.svg?download",
                FlagWidth = 600,
                FlagHeight = 400,
            },
            new CountryWithFlag()
            {
                // flag is 13160 bytes
                Name = @"Portugal",
                WikiUrl = @"/wiki/Portugal",
                GDP = 230117,
                FlagKey = @"Flag_of_Portugal",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/5/5c/Flag_of_Portugal.svg?download",
                FlagWidth = 600,
                FlagHeight = 400,
            },
            new CountryWithFlag()
            {
                // flag is 2728 bytes
                Name = @"Iraq",
                WikiUrl = @"/wiki/Iraq",
                GDP = 225422,
                FlagKey = @"Flag_of_Iraq",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/f/f6/Flag_of_Iraq.svg?download",
                FlagWidth = 411,
                FlagHeight = 274,
            },
            new CountryWithFlag()
            {
                // flag is 11069 bytes
                Name = @"Kazakhstan",
                WikiUrl = @"/wiki/Kazakhstan",
                GDP = 216036,
                FlagKey = @"Flag_of_Kazakhstan",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/d/d3/Flag_of_Kazakhstan.svg?download",
                FlagWidth = 600,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 313 bytes
                Name = @"Algeria",
                WikiUrl = @"/wiki/Algeria",
                GDP = 213518,
                FlagKey = @"Flag_of_Algeria",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/7/77/Flag_of_Algeria.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 322 bytes
                Name = @"Qatar",
                WikiUrl = @"/wiki/Qatar",
                GDP = 211817,
                FlagKey = @"Flag_of_Qatar",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/6/65/Flag_of_Qatar.svg?download",
                FlagWidth = 1400,
                FlagHeight = 550,
            },
            new CountryWithFlag()
            {
                // flag is 275 bytes
                Name = @"Czech Republic",
                WikiUrl = @"/wiki/Czech_Republic",
                GDP = 205270,
                FlagKey = @"Flag_of_The_Czech_Republic",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/c/cb/Flag_of_the_Czech_Republic.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 381 bytes
                Name = @"Peru",
                WikiUrl = @"/wiki/Peru",
                GDP = 201809,
                FlagKey = @"Flag_of_Peru",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/c/cf/Flag_of_Peru.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 297 bytes
                Name = @"Romania",
                WikiUrl = @"/wiki/Romania",
                GDP = 199045,
                FlagKey = @"Flag_of_Romania",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/7/73/Flag_of_Romania.svg?download",
                FlagWidth = 600,
                FlagHeight = 400,
            },
            new CountryWithFlag()
            {
                // flag is 2533 bytes
                Name = @"New Zealand",
                WikiUrl = @"/wiki/New_Zealand",
                GDP = 198652,
                FlagKey = @"Flag_of_New_Zealand",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/3/3e/Flag_of_New_Zealand.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 280 bytes
                Name = @"Vietnam",
                WikiUrl = @"/wiki/Vietnam",
                GDP = 186205,
                FlagKey = @"Flag_of_Vietnam",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/2/21/Flag_of_Vietnam.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 218 bytes
                Name = @"Bangladesh",
                WikiUrl = @"/wiki/Bangladesh",
                GDP = 173062,
                FlagKey = @"Flag_of_Bangladesh",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/f/f9/Flag_of_Bangladesh.svg?download",
                FlagWidth = 500,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 319 bytes
                Name = @"Kuwait",
                WikiUrl = @"/wiki/Kuwait",
                GDP = 163637,
                FlagKey = @"Flag_of_Kuwait",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/a/aa/Flag_of_Kuwait.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 2007 bytes
                Name = @"Angola",
                WikiUrl = @"/wiki/Angola",
                GDP = 146676,
                FlagKey = @"Flag_of_Angola",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/9/9d/Flag_of_Angola.svg?download",
                FlagWidth = 450,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 224 bytes
                Name = @"Hungary",
                WikiUrl = @"/wiki/Hungary",
                GDP = 138347,
                FlagKey = @"Flag_of_Hungary",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/c/c1/Flag_of_Hungary.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 182 bytes
                Name = @"Ukraine",
                WikiUrl = @"/wiki/Ukraine",
                GDP = 131806,
                FlagKey = @"Flag_of_Ukraine",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/4/49/Flag_of_Ukraine.svg?download",
                FlagWidth = 1200,
                FlagHeight = 800,
            },
            new CountryWithFlag()
            {
                // flag is 310 bytes
                Name = @"Morocco",
                WikiUrl = @"/wiki/Morocco",
                GDP = 110009,
                FlagKey = @"Flag_of_Morocco",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/2/2c/Flag_of_Morocco.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 921 bytes
                Name = @"Puerto Rico",
                WikiUrl = @"/wiki/Puerto_Rico",
                GDP = 103676,
                FlagKey = @"Flag_of_Puerto_Rico",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/2/28/Flag_of_Puerto_Rico.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 279145 bytes
                Name = @"Ecuador",
                WikiUrl = @"/wiki/Ecuador",
                GDP = 100917,
                FlagKey = @"Flag_of_Ecuador",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/e/e8/Flag_of_Ecuador.svg?download",
                FlagWidth = 1440,
                FlagHeight = 960,
            },
            new CountryWithFlag()
            {
                // flag is 2163 bytes
                Name = @"Slovakia",
                WikiUrl = @"/wiki/Slovakia",
                GDP = 100249,
                FlagKey = @"Flag_of_Slovakia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/e/e6/Flag_of_Slovakia.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 308 bytes
                Name = @"Cuba",
                WikiUrl = @"/wiki/Cuba",
                GDP = 87205,
                FlagKey = @"Flag_of_Cuba",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/b/bd/Flag_of_Cuba.svg?download",
                FlagWidth = 800,
                FlagHeight = 400,
            },
            new CountryWithFlag()
            {
                // flag is 294 bytes
                Name = @"Sudan",
                WikiUrl = @"/wiki/Sudan",
                GDP = 81894,
                FlagKey = @"Flag_of_Sudan",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/0/01/Flag_of_Sudan.svg?download",
                FlagWidth = 600,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 23171 bytes
                Name = @"Oman",
                WikiUrl = @"/wiki/Oman",
                GDP = 81797,
                FlagKey = @"Flag_of_Oman",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/d/dd/Flag_of_Oman.svg?download",
                FlagWidth = 600,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 1395 bytes
                Name = @"Belarus",
                WikiUrl = @"/wiki/Belarus",
                GDP = 76139,
                FlagKey = @"Flag_of_Belarus",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/8/85/Flag_of_Belarus.svg?download",
                FlagWidth = 900,
                FlagHeight = 450,
            },
            new CountryWithFlag()
            {
                // flag is 622 bytes
                Name = @"Azerbaijan",
                WikiUrl = @"/wiki/Azerbaijan",
                GDP = 75193,
                FlagKey = @"Flag_of_Azerbaijan",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/d/dd/Flag_of_Azerbaijan.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 22601 bytes
                Name = @"Sri Lanka",
                WikiUrl = @"/wiki/Sri_Lanka",
                GDP = 74941,
                FlagKey = @"Flag_of_Sri_Lanka",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/1/11/Flag_of_Sri_Lanka.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 650 bytes
                Name = @"Myanmar",
                WikiUrl = @"/wiki/Myanmar",
                GDP = 66478,
                FlagKey = @"Flag_of_Myanmar",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/8/8c/Flag_of_Myanmar.svg?download",
                FlagWidth = 1800,
                FlagHeight = 1200,
            },
            new CountryWithFlag()
            {
                // flag is 281 bytes
                Name = @"Luxembourg",
                WikiUrl = @"/wiki/Luxembourg",
                GDP = 64874,
                FlagKey = @"Flag_of_Luxembourg",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/d/da/Flag_of_Luxembourg.svg?download",
                FlagWidth = 1000,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 251471 bytes
                Name = @"Dominican Republic",
                WikiUrl = @"/wiki/Dominican_Republic",
                GDP = 63969,
                FlagKey = @"Flag_of_The_Dominican_Republic",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/9/9f/Flag_of_the_Dominican_Republic.svg?download",
                FlagWidth = 750,
                FlagHeight = 500,
            },
            new CountryWithFlag()
            {
                // flag is 1063 bytes
                Name = @"Uzbekistan",
                WikiUrl = @"/wiki/Uzbekistan",
                GDP = 63030,
                FlagKey = @"Flag_of_Uzbekistan",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/8/84/Flag_of_Uzbekistan.svg?download",
                FlagWidth = 500,
                FlagHeight = 250,
            },
            new CountryWithFlag()
            {
                // flag is 1343 bytes
                Name = @"Kenya",
                WikiUrl = @"/wiki/Kenya",
                GDP = 60936,
                FlagKey = @"Flag_of_Kenya",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/4/49/Flag_of_Kenya.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 86670 bytes
                Name = @"Guatemala",
                WikiUrl = @"/wiki/Guatemala",
                GDP = 58827,
                FlagKey = @"Flag_of_Guatemala",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/e/ec/Flag_of_Guatemala.svg?download",
                FlagWidth = 960,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 1998 bytes
                Name = @"Uruguay",
                WikiUrl = @"/wiki/Uruguay",
                GDP = 57471,
                FlagKey = @"Flag_of_Uruguay",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/f/fe/Flag_of_Uruguay.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 68547 bytes
                Name = @"Croatia",
                WikiUrl = @"/wiki/Croatia",
                GDP = 57137,
                FlagKey = @"Flag_of_Croatia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/1/1b/Flag_of_Croatia.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 288 bytes
                Name = @"Bulgaria",
                WikiUrl = @"/wiki/Bulgaria",
                GDP = 56718,
                FlagKey = @"Flag_of_Bulgaria",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/9/9a/Flag_of_Bulgaria.svg?download",
                FlagWidth = 1000,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 2359 bytes
                Name = @"Macau",
                WikiUrl = @"/wiki/Macau",
                GDP = 55502,
                FlagKey = @"Flag_of_Macau",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/6/63/Flag_of_Macau.svg?download",
                FlagWidth = 450,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 819 bytes
                Name = @"Ethiopia",
                WikiUrl = @"/wiki/Ethiopia",
                GDP = 53638,
                FlagKey = @"Flag_of_Ethiopia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/7/71/Flag_of_Ethiopia.svg?download",
                FlagWidth = 720,
                FlagHeight = 360,
            },
            new CountryWithFlag()
            {
                // flag is 10430 bytes
                Name = @"Lebanon",
                WikiUrl = @"/wiki/Lebanon",
                GDP = 49631,
                FlagKey = @"Flag_of_Lebanon",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/5/59/Flag_of_Lebanon.svg?download",
                FlagWidth = 750,
                FlagHeight = 500,
            },
            new CountryWithFlag()
            {
                // flag is 266 bytes
                Name = @"Costa Rica",
                WikiUrl = @"/wiki/Costa_Rica",
                GDP = 49553,
                FlagKey = @"Flag_of_Costa_Rica",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/f/f2/Flag_of_Costa_Rica.svg?download",
                FlagWidth = 1000,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 2060 bytes
                Name = @"Slovenia",
                WikiUrl = @"/wiki/Slovenia",
                GDP = 49491,
                FlagKey = @"Flag_of_Slovenia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/f/f0/Flag_of_Slovenia.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 763 bytes
                Name = @"Panama",
                WikiUrl = @"/wiki/Panama",
                GDP = 49166,
                FlagKey = @"Flag_of_Panama",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/a/ab/Flag_of_Panama.svg?download",
                FlagWidth = 450,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 266 bytes
                Name = @"Lithuania",
                WikiUrl = @"/wiki/Lithuania",
                GDP = 48392,
                FlagKey = @"Flag_of_Lithuania",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/1/11/Flag_of_Lithuania.svg?download",
                FlagWidth = 500,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 328 bytes
                Name = @"United Republic of Tanzania",
                WikiUrl = @"/wiki/Tanzania",
                GDP = 48030,
                FlagKey = @"Flag_of_Tanzania",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/3/38/Flag_of_Tanzania.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 55438 bytes
                Name = @"Turkmenistan",
                WikiUrl = @"/wiki/Turkmenistan",
                GDP = 47932,
                FlagKey = @"Flag_of_Turkmenistan",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/1/1b/Flag_of_Turkmenistan.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 618 bytes
                Name = @"Tunisia",
                WikiUrl = @"/wiki/Tunisia",
                GDP = 47423,
                FlagKey = @"Flag_of_Tunisia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/c/ce/Flag_of_Tunisia.svg?download",
                FlagWidth = 1200,
                FlagHeight = 800,
            },
            new CountryWithFlag()
            {
                // flag is 270889 bytes
                Name = @"Serbia",
                WikiUrl = @"/wiki/Serbia",
                GDP = 43866,
                FlagKey = @"Flag_of_Serbia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/f/ff/Flag_of_Serbia.svg?download",
                FlagWidth = 1350,
                FlagHeight = 900,
            },
            new CountryWithFlag()
            {
                // flag is 479 bytes
                Name = @"Libya",
                WikiUrl = @"/wiki/Libya",
                GDP = 41319,
                FlagKey = @"Flag_of_Libya",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/0/05/Flag_of_Libya.svg?download",
                FlagWidth = 960,
                FlagHeight = 480,
            },
            new CountryWithFlag()
            {
                // flag is 343 bytes
                Name = @"Ghana",
                WikiUrl = @"/wiki/Ghana",
                GDP = 37177,
                FlagKey = @"Flag_of_Ghana",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/1/19/Flag_of_Ghana.svg?download",
                FlagWidth = 450,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 242 bytes
                Name = @"Yemen",
                WikiUrl = @"/wiki/Yemen",
                GDP = 37131,
                FlagKey = @"Flag_of_Yemen",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/8/89/Flag_of_Yemen.svg?download",
                FlagWidth = 450,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 542 bytes
                Name = @"Democratic Republic of the Congo",
                WikiUrl = @"/wiki/Democratic_Republic_of_the_Congo",
                GDP = 35909,
                FlagKey = @"Flag_of_The_Democratic_Republic_of_The_Congo",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/6/6f/Flag_of_the_Democratic_Republic_of_the_Congo.s" +
                             @"vg?download",
                FlagWidth = 800,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 625 bytes
                Name = @"Jordan",
                WikiUrl = @"/wiki/Jordan",
                GDP = 35827,
                FlagKey = @"Flag_of_Jordan",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/c/c0/Flag_of_Jordan.svg?download",
                FlagWidth = 840,
                FlagHeight = 420,
            },
            new CountryWithFlag()
            {
                // flag is 257 bytes
                Name = @"CГґte d'Ivoire",
                WikiUrl = @"/wiki/Ivory_Coast",
                GDP = 34254,
                FlagKey = @"Flag_of_C%C3%B4te_dIvoire",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/f/fe/Flag_of_C%C3%B4te_d'Ivoire.svg?download",
                FlagWidth = 450,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 269 bytes
                Name = @"Bahrain",
                WikiUrl = @"/wiki/Bahrain",
                GDP = 33850,
                FlagKey = @"Flag_of_Bahrain",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/2/2c/Flag_of_Bahrain.svg?download",
                FlagWidth = 1500,
                FlagHeight = 900,
            },
            new CountryWithFlag()
            {
                // flag is 265 bytes
                Name = @"Bolivia",
                WikiUrl = @"/wiki/Bolivia",
                GDP = 32996,
                FlagKey = @"Flag_of_Bolivia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/4/48/Flag_of_Bolivia.svg?download",
                FlagWidth = 1100,
                FlagHeight = 750,
            },
            new CountryWithFlag()
            {
                // flag is 745 bytes
                Name = @"Cameroon",
                WikiUrl = @"/wiki/Cameroon",
                GDP = 32051,
                FlagKey = @"Flag_of_Cameroon",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/4/4f/Flag_of_Cameroon.svg?download",
                FlagWidth = 600,
                FlagHeight = 400,
            },
            new CountryWithFlag()
            {
                // flag is 197 bytes
                Name = @"Latvia",
                WikiUrl = @"/wiki/Latvia",
                GDP = 31286,
                FlagKey = @"Flag_of_Latvia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/8/84/Flag_of_Latvia.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 54850 bytes
                Name = @"Paraguay",
                WikiUrl = @"/wiki/Paraguay",
                GDP = 30985,
                FlagKey = @"Flag_of_Paraguay",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/2/27/Flag_of_Paraguay.svg?download",
                FlagWidth = 600,
                FlagHeight = 330,
            },
            new CountryWithFlag()
            {
                // flag is 307 bytes
                Name = @"Trinidad and Tobago",
                WikiUrl = @"/wiki/Trinidad_and_Tobago",
                GDP = 28069,
                FlagKey = @"Flag_of_Trinidad_and_Tobago",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/6/64/Flag_of_Trinidad_and_Tobago.svg?download",
                FlagWidth = 800,
                FlagHeight = 480,
            },
            new CountryWithFlag()
            {
                // flag is 6678 bytes
                Name = @"Uganda",
                WikiUrl = @"/wiki/Uganda",
                GDP = 27465,
                FlagKey = @"Flag_of_Uganda",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/4/4e/Flag_of_Uganda.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 13047 bytes
                Name = @"Zambia",
                WikiUrl = @"/wiki/Zambia",
                GDP = 26963,
                FlagKey = @"Flag_of_Zambia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/0/06/Flag_of_Zambia.svg?download",
                FlagWidth = 2100,
                FlagHeight = 1400,
            },
            new CountryWithFlag()
            {
                // flag is 253 bytes
                Name = @"Estonia",
                WikiUrl = @"/wiki/Estonia",
                GDP = 26485,
                FlagKey = @"Flag_of_Estonia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/8/8f/Flag_of_Estonia.svg?download",
                FlagWidth = 990,
                FlagHeight = 630,
            },
            new CountryWithFlag()
            {
                // flag is 180970 bytes
                Name = @"El Salvador",
                WikiUrl = @"/wiki/El_Salvador",
                GDP = 25164,
                FlagKey = @"Flag_of_El_Salvador",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/3/34/Flag_of_El_Salvador.svg?download",
                FlagWidth = 1064,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 14737 bytes
                Name = @"Cyprus",
                WikiUrl = @"/wiki/Cyprus",
                GDP = 23077,
                FlagKey = @"Flag_of_Cyprus",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/d/d4/Flag_of_Cyprus.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 301594 bytes
                Name = @"Afghanistan",
                WikiUrl = @"/wiki/Afghanistan",
                GDP = 21122,
                FlagKey = @"Flag_of_Afghanistan",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/9/9a/Flag_of_Afghanistan.svg?download",
                FlagWidth = 600,
                FlagHeight = 400,
            },
            new CountryWithFlag()
            {
                // flag is 1016 bytes
                Name = @"Honduras",
                WikiUrl = @"/wiki/Honduras",
                GDP = 19497,
                FlagKey = @"Flag_of_Honduras",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/8/82/Flag_of_Honduras.svg?download",
                FlagWidth = 1000,
                FlagHeight = 500,
            },
            new CountryWithFlag()
            {
                // flag is 1076 bytes
                Name = @"Nepal",
                WikiUrl = @"/wiki/Nepal",
                GDP = 19489,
                FlagKey = @"Flag_of_Nepal",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/9/9b/Flag_of_Nepal.svg?download",
                FlagWidth = 726,
                FlagHeight = 885,
            },
            new CountryWithFlag()
            {
                // flag is 512 bytes
                Name = @"Bosnia and Herzegovina",
                WikiUrl = @"/wiki/Bosnia_and_Herzegovina",
                GDP = 18491,
                FlagKey = @"Flag_of_Bosnia_and_Herzegovina",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/b/bf/Flag_of_Bosnia_and_Herzegovina.svg?download",
                FlagWidth = 800,
                FlagHeight = 400,
            },
            new CountryWithFlag()
            {
                // flag is 255 bytes
                Name = @"Gabon",
                WikiUrl = @"/wiki/Gabon",
                GDP = 17412,
                FlagKey = @"Flag_of_Gabon",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/0/04/Flag_of_Gabon.svg?download",
                FlagWidth = 400,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 873 bytes
                Name = @"North Korea",
                WikiUrl = @"/wiki/North_Korea",
                GDP = 17396,
                FlagKey = @"Flag_of_North_Korea",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/5/51/Flag_of_North_Korea.svg?download",
                FlagWidth = 1600,
                FlagHeight = 800,
            },
            new CountryWithFlag()
            {
                // flag is 24304 bytes
                Name = @"Brunei",
                WikiUrl = @"/wiki/Brunei",
                GDP = 17104,
                FlagKey = @"Flag_of_Brunei",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/9/9c/Flag_of_Brunei.svg?download",
                FlagWidth = 1440,
                FlagHeight = 720,
            },
            new CountryWithFlag()
            {
                // flag is 4838 bytes
                Name = @"Mozambique",
                WikiUrl = @"/wiki/Mozambique",
                GDP = 17081,
                FlagKey = @"Flag_of_Mozambique",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/d/d0/Flag_of_Mozambique.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 281 bytes
                Name = @"Iceland",
                WikiUrl = @"/wiki/Iceland",
                GDP = 17036,
                FlagKey = @"Flag_of_Iceland",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/c/ce/Flag_of_Iceland.svg?download",
                FlagWidth = 500,
                FlagHeight = 360,
            },
            new CountryWithFlag()
            {
                // flag is 13296 bytes
                Name = @"Cambodia",
                WikiUrl = @"/wiki/Cambodia",
                GDP = 16778,
                FlagKey = @"Flag_of_Cambodia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/8/83/Flag_of_Cambodia.svg?download",
                FlagWidth = 625,
                FlagHeight = 400,
            },
            new CountryWithFlag()
            {
                // flag is 8291 bytes
                Name = @"Equatorial Guinea",
                WikiUrl = @"/wiki/Equatorial_Guinea",
                GDP = 16731,
                FlagKey = @"Flag_of_Equatorial_Guinea",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/3/31/Flag_of_Equatorial_Guinea.svg?download",
                FlagWidth = 1200,
                FlagHeight = 800,
            },
            new CountryWithFlag()
            {
                // flag is 4686 bytes
                Name = @"Papua New Guinea",
                WikiUrl = @"/wiki/Papua_New_Guinea",
                GDP = 16576,
                FlagKey = @"Flag_of_Papua_New_Guinea",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/e/e3/Flag_of_Papua_New_Guinea.svg?download",
                FlagWidth = 600,
                FlagHeight = 450,
            },
            new CountryWithFlag()
            {
                // flag is 970 bytes
                Name = @"Georgia",
                WikiUrl = @"/wiki/Georgia_(country)",
                GDP = 16530,
                FlagKey = @"Flag_of_Georgia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/0/0f/Flag_of_Georgia.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 268 bytes
                Name = @"Botswana",
                WikiUrl = @"/wiki/Botswana",
                GDP = 15813,
                FlagKey = @"Flag_of_Botswana",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/f/fa/Flag_of_Botswana.svg?download",
                FlagWidth = 1200,
                FlagHeight = 800,
            },
            new CountryWithFlag()
            {
                // flag is 742 bytes
                Name = @"Senegal",
                WikiUrl = @"/wiki/Senegal",
                GDP = 15658,
                FlagKey = @"Flag_of_Senegal",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/f/fd/Flag_of_Senegal.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 19599 bytes
                Name = @"Zimbabwe",
                WikiUrl = @"/wiki/Zimbabwe",
                GDP = 14719,
                FlagKey = @"Flag_of_Zimbabwe",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/6/6a/Flag_of_Zimbabwe.svg?download",
                FlagWidth = 840,
                FlagHeight = 420,
            },
            new CountryWithFlag()
            {
                // flag is 424 bytes
                Name = @"Republic of the Congo",
                WikiUrl = @"/wiki/Republic_of_the_Congo",
                GDP = 14077,
                FlagKey = @"Flag_of_The_Republic_of_The_Congo",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/9/92/Flag_of_the_Republic_of_the_Congo.svg?download",
                FlagWidth = 600,
                FlagHeight = 400,
            },
            new CountryWithFlag()
            {
                // flag is 218 bytes
                Name = @"Jamaica",
                WikiUrl = @"/wiki/Jamaica",
                GDP = 13927,
                FlagKey = @"Flag_of_Jamaica",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/0/0a/Flag_of_Jamaica.svg?download",
                FlagWidth = 600,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 853 bytes
                Name = @"Namibia",
                WikiUrl = @"/wiki/Namibia",
                GDP = 13429,
                FlagKey = @"Flag_of_Namibia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/0/00/Flag_of_Namibia.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 14817 bytes
                Name = @"Albania",
                WikiUrl = @"/wiki/Albania",
                GDP = 13413,
                FlagKey = @"Flag_of_Albania",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/3/36/Flag_of_Albania.svg?download",
                FlagWidth = 700,
                FlagHeight = 500,
            },
            new CountryWithFlag()
            {
                // flag is 260 bytes
                Name = @"Chad",
                WikiUrl = @"/wiki/Chad",
                GDP = 12791,
                FlagKey = @"Flag_of_Chad",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/4/4b/Flag_of_Chad.svg?download",
                FlagWidth = 450,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 281 bytes
                Name = @"Arab Palestinian areas",
                WikiUrl = @"/wiki/Palestinian_National_Authority",
                GDP = 12766,
                FlagKey = @"Flag_of_Palestine",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/0/00/Flag_of_Palestine.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 672 bytes
                Name = @"Burkina Faso",
                WikiUrl = @"/wiki/Burkina_Faso",
                GDP = 12756,
                FlagKey = @"Flag_of_Burkina_Faso",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/3/31/Flag_of_Burkina_Faso.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 311 bytes
                Name = @"Mauritius",
                WikiUrl = @"/wiki/Mauritius",
                GDP = 12616,
                FlagKey = @"Flag_of_Mauritius",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/7/77/Flag_of_Mauritius.svg?download",
                FlagWidth = 450,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 1022 bytes
                Name = @"Mongolia",
                WikiUrl = @"/wiki/Mongolia",
                GDP = 12067,
                FlagKey = @"Flag_of_Mongolia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/4/4c/Flag_of_Mongolia.svg?download",
                FlagWidth = 4800,
                FlagHeight = 2400,
            },
            new CountryWithFlag()
            {
                // flag is 255 bytes
                Name = @"Mali",
                WikiUrl = @"/wiki/Mali",
                GDP = 11979,
                FlagKey = @"Flag_of_Mali",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/9/92/Flag_of_Mali.svg?download",
                FlagWidth = 450,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 40644 bytes
                Name = @"Nicaragua",
                WikiUrl = @"/wiki/Nicaragua",
                GDP = 11806,
                FlagKey = @"Flag_of_Nicaragua",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/1/19/Flag_of_Nicaragua.svg?download",
                FlagWidth = 1000,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 279 bytes
                Name = @"Laos",
                WikiUrl = @"/wiki/Laos",
                GDP = 11749,
                FlagKey = @"Flag_of_Laos",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/5/56/Flag_of_Laos.svg?download",
                FlagWidth = 600,
                FlagHeight = 400,
            },
            new CountryWithFlag()
            {
                // flag is 391 bytes
                Name = @"Macedonia",
                WikiUrl = @"/wiki/Republic_of_Macedonia",
                GDP = 11319,
                FlagKey = @"Flag_of_Macedonia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/f/f8/Flag_of_Macedonia.svg?download",
                FlagWidth = 1400,
                FlagHeight = 700,
            },
            new CountryWithFlag()
            {
                // flag is 518 bytes
                Name = @"South Sudan",
                WikiUrl = @"/wiki/South_Sudan",
                GDP = 11007,
                FlagKey = @"Flag_of_South_Sudan",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/7/7a/Flag_of_South_Sudan.svg?download",
                FlagWidth = 1000,
                FlagHeight = 500,
            },
            new CountryWithFlag()
            {
                // flag is 259 bytes
                Name = @"Armenia",
                WikiUrl = @"/wiki/Armenia",
                GDP = 10889,
                FlagKey = @"Flag_of_Armenia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/2/2f/Flag_of_Armenia.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 257 bytes
                Name = @"Madagascar",
                WikiUrl = @"/wiki/Madagascar",
                GDP = 10674,
                FlagKey = @"Flag_of_Madagascar",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/b/bc/Flag_of_Madagascar.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 22986 bytes
                Name = @"Malta",
                WikiUrl = @"/wiki/Malta",
                GDP = 10536,
                FlagKey = @"Flag_of_Malta",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/7/73/Flag_of_Malta.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 1730 bytes
                Name = @"New Caledonia",
                WikiUrl = @"/wiki/New_Caledonia",
                GDP = 10234,
                FlagKey = @"Flag_of_FLNKS",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/6/66/Flag_of_FLNKS.svg?download",
                FlagWidth = 900,
                FlagHeight = 450,
            },
            new CountryWithFlag()
            {
                // flag is 272 bytes
                Name = @"Benin",
                WikiUrl = @"/wiki/Benin",
                GDP = 9575,
                FlagKey = @"Flag_of_Benin",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/0/0a/Flag_of_Benin.svg?download",
                FlagWidth = 450,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 1792 bytes
                Name = @"Tajikistan",
                WikiUrl = @"/wiki/Tajikistan",
                GDP = 9242,
                FlagKey = @"Flag_of_Tajikistan",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/d/d0/Flag_of_Tajikistan.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 32519 bytes
                Name = @"Haiti",
                WikiUrl = @"/wiki/Haiti",
                GDP = 8599,
                FlagKey = @"Flag_of_Haiti",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/5/56/Flag_of_Haiti.svg?download",
                FlagWidth = 500,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 251 bytes
                Name = @"The Bahamas",
                WikiUrl = @"/wiki/The_Bahamas",
                GDP = 8510,
                FlagKey = @"Flag_of_The_Bahamas",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/9/93/Flag_of_the_Bahamas.svg?download",
                FlagWidth = 600,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 296 bytes
                Name = @"Niger",
                WikiUrl = @"/wiki/Niger",
                GDP = 8169,
                FlagKey = @"Flag_of_Niger",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/f/f4/Flag_of_Niger.svg?download",
                FlagWidth = 700,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 29606 bytes
                Name = @"Moldova",
                WikiUrl = @"/wiki/Moldova",
                GDP = 7944,
                FlagKey = @"Flag_of_Moldova",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/2/27/Flag_of_Moldova.svg?download",
                FlagWidth = 1800,
                FlagHeight = 900,
            },
            new CountryWithFlag()
            {
                // flag is 831 bytes
                Name = @"Rwanda",
                WikiUrl = @"/wiki/Rwanda",
                GDP = 7903,
                FlagKey = @"Flag_of_Rwanda",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/1/17/Flag_of_Rwanda.svg?download",
                FlagWidth = 1080,
                FlagHeight = 720,
            },
            new CountryWithFlag()
            {
                // flag is 1411 bytes
                Name = @"Kyrgyzstan",
                WikiUrl = @"/wiki/Kyrgyzstan",
                GDP = 7404,
                FlagKey = @"Flag_of_Kyrgyzstan",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/c/c7/Flag_of_Kyrgyzstan.svg?download",
                FlagWidth = 750,
                FlagHeight = 450,
            },
            new CountryWithFlag()
            {
                // flag is 14973 bytes
                Name = @"Kosovo",
                WikiUrl = @"/wiki/Kosovo",
                GDP = 7387,
                FlagKey = @"Flag_of_Kosovo",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/1/1f/Flag_of_Kosovo.svg?download",
                FlagWidth = 840,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 208 bytes
                Name = @"Monaco",
                WikiUrl = @"/wiki/Monaco",
                GDP = 7060,
                FlagKey = @"Flag_of_Monaco",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/e/ea/Flag_of_Monaco.svg?download",
                FlagWidth = 750,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 304 bytes
                Name = @"Guinea",
                WikiUrl = @"/wiki/Guinea",
                GDP = 6579,
                FlagKey = @"Flag_of_Guinea",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/e/ed/Flag_of_Guinea.svg?download",
                FlagWidth = 450,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 23762 bytes
                Name = @"Liechtenstein",
                WikiUrl = @"/wiki/Liechtenstein",
                GDP = 5855,
                FlagKey = @"Flag_of_Liechtenstein",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/4/47/Flag_of_Liechtenstein.svg?download",
                FlagWidth = 1000,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 993 bytes
                Name = @"Malawi",
                WikiUrl = @"/wiki/Malawi",
                GDP = 5720,
                FlagKey = @"Flag_of_Malawi",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/d/d1/Flag_of_Malawi.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 7692 bytes
                Name = @"French Polynesia",
                WikiUrl = @"/wiki/French_Polynesia",
                GDP = 5623,
                FlagKey = @"Flag_of_French_Polynesia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/d/db/Flag_of_French_Polynesia.svg?download",
                FlagWidth = 600,
                FlagHeight = 400,
            },
            new CountryWithFlag()
            {
                // flag is 45744 bytes
                Name = @"Bermuda",
                WikiUrl = @"/wiki/Bermuda",
                GDP = 5601,
                FlagKey = @"Flag_of_Bermuda",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/b/bf/Flag_of_Bermuda.svg?download",
                FlagWidth = 1000,
                FlagHeight = 500,
            },
            new CountryWithFlag()
            {
                // flag is 393 bytes
                Name = @"Suriname",
                WikiUrl = @"/wiki/Suriname",
                GDP = 5210,
                FlagKey = @"Flag_of_Suriname",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/6/60/Flag_of_Suriname.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 669 bytes
                Name = @"Mauritania",
                WikiUrl = @"/wiki/Mauritania",
                GDP = 5092,
                FlagKey = @"Flag_of_Mauritania",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/4/43/Flag_of_Mauritania.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 669 bytes
                Name = @"Timor-Leste",
                WikiUrl = @"/wiki/East_Timor",
                GDP = 4970,
                FlagKey = @"Flag_of_East_Timor",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/2/26/Flag_of_East_Timor.svg?download",
                FlagWidth = 900,
                FlagHeight = 450,
            },
            new CountryWithFlag()
            {
                // flag is 252 bytes
                Name = @"Sierra Leone",
                WikiUrl = @"/wiki/Sierra_Leone",
                GDP = 4893,
                FlagKey = @"Flag_of_Sierra_Leone",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/1/17/Flag_of_Sierra_Leone.svg?download",
                FlagWidth = 450,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 88634 bytes
                Name = @"Montenegro",
                WikiUrl = @"/wiki/Montenegro",
                GDP = 4588,
                FlagKey = @"Flag_of_Montenegro",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/6/64/Flag_of_Montenegro.svg?download",
                FlagWidth = 640,
                FlagHeight = 320,
            },
            new CountryWithFlag()
            {
                // flag is 769 bytes
                Name = @"Togo",
                WikiUrl = @"/wiki/Togo",
                GDP = 4576,
                FlagKey = @"Flag_of_Togo",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/6/68/Flag_of_Togo.svg?download",
                FlagWidth = 809,
                FlagHeight = 500,
            },
            new CountryWithFlag()
            {
                // flag is 54919 bytes
                Name = @"Fiji",
                WikiUrl = @"/wiki/Fiji",
                GDP = 4532,
                FlagKey = @"Flag_of_Fiji",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/b/ba/Flag_of_Fiji.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 6886 bytes
                Name = @"Swaziland",
                WikiUrl = @"/wiki/Swaziland",
                GDP = 4482,
                FlagKey = @"Flag_of_Swaziland",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/1/1e/Flag_of_Swaziland.svg?download",
                FlagWidth = 450,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 985 bytes
                Name = @"Barbados",
                WikiUrl = @"/wiki/Barbados",
                GDP = 4353,
                FlagKey = @"Flag_of_Barbados",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/e/ef/Flag_of_Barbados.svg?download",
                FlagWidth = 1500,
                FlagHeight = 1000,
            },
            new CountryWithFlag()
            {
                // flag is 6755 bytes
                Name = @"Eritrea",
                WikiUrl = @"/wiki/Eritrea",
                GDP = 3858,
                FlagKey = @"Flag_of_Eritrea",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/2/29/Flag_of_Eritrea.svg?download",
                FlagWidth = 1000,
                FlagHeight = 500,
            },
            new CountryWithFlag()
            {
                // flag is 50126 bytes
                Name = @"Cayman Islands",
                WikiUrl = @"/wiki/Cayman_Islands",
                GDP = 3480,
                FlagKey = @"Flag_of_The_Cayman_Islands",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/0/0f/Flag_of_the_Cayman_Islands.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 78007 bytes
                Name = @"Andorra",
                WikiUrl = @"/wiki/Andorra",
                GDP = 3278,
                FlagKey = @"Flag_of_Andorra",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/1/19/Flag_of_Andorra.svg?download",
                FlagWidth = 1000,
                FlagHeight = 700,
            },
            new CountryWithFlag()
            {
                // flag is 672 bytes
                Name = @"CuraГ§ao",
                WikiUrl = @"/wiki/Cura%C3%A7ao",
                GDP = 3159,
                FlagKey = @"Flag_of_Cura%C3%A7ao",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/b/b1/Flag_of_Cura%C3%A7ao.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 406 bytes
                Name = @"Guyana",
                WikiUrl = @"/wiki/Guyana",
                GDP = 3086,
                FlagKey = @"Flag_of_Guyana",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/9/99/Flag_of_Guyana.svg?download",
                FlagWidth = 500,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 322 bytes
                Name = @"Maldives",
                WikiUrl = @"/wiki/Maldives",
                GDP = 3032,
                FlagKey = @"Flag_of_Maldives",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/0/0f/Flag_of_Maldives.svg?download",
                FlagWidth = 720,
                FlagHeight = 480,
            },
            new CountryWithFlag()
            {
                // flag is 985 bytes
                Name = @"Burundi",
                WikiUrl = @"/wiki/Burundi",
                GDP = 2869,
                FlagKey = @"Flag_of_Burundi",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/5/50/Flag_of_Burundi.svg?download",
                FlagWidth = 500,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 415 bytes
                Name = @"Aruba",
                WikiUrl = @"/wiki/Aruba",
                GDP = 2664,
                FlagKey = @"Flag_of_Aruba",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/f/f6/Flag_of_Aruba.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 246 bytes
                Name = @"Greenland",
                WikiUrl = @"/wiki/Greenland",
                GDP = 2441,
                FlagKey = @"Flag_of_Greenland",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/0/09/Flag_of_Greenland.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 730 bytes
                Name = @"Liberia",
                WikiUrl = @"/wiki/Liberia",
                GDP = 2122,
                FlagKey = @"Flag_of_Liberia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/b/b8/Flag_of_Liberia.svg?download",
                FlagWidth = 1140,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 2115 bytes
                Name = @"Lesotho",
                WikiUrl = @"/wiki/Lesotho",
                GDP = 2081,
                FlagKey = @"Flag_of_Lesotho",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/4/4a/Flag_of_Lesotho.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 48736 bytes
                Name = @"Bhutan",
                WikiUrl = @"/wiki/Bhutan",
                GDP = 1965,
                FlagKey = @"Flag_of_Bhutan",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/9/91/Flag_of_Bhutan.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 988 bytes
                Name = @"Cape Verde",
                WikiUrl = @"/wiki/Cape_Verde",
                GDP = 1855,
                FlagKey = @"Flag_of_Cape_Verde",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/3/38/Flag_of_Cape_Verde.svg?download",
                FlagWidth = 510,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 132841 bytes
                Name = @"San Marino",
                WikiUrl = @"/wiki/San_Marino",
                GDP = 1845,
                FlagKey = @"Flag_of_San_Marino",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/b/b1/Flag_of_San_Marino.svg?download",
                FlagWidth = 800,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 489 bytes
                Name = @"Central African Republic",
                WikiUrl = @"/wiki/Central_African_Republic",
                GDP = 1838,
                FlagKey = @"Flag_of_The_Central_African_Republic",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/6/6f/Flag_of_the_Central_African_Republic.svg?downl" +
                             @"oad",
                FlagWidth = 450,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 91067 bytes
                Name = @"Belize",
                WikiUrl = @"/wiki/Belize",
                GDP = 1699,
                FlagKey = @"Flag_of_Belize",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/e/e7/Flag_of_Belize.svg?download",
                FlagWidth = 750,
                FlagHeight = 500,
            },
            new CountryWithFlag()
            {
                // flag is 596 bytes
                Name = @"Djibouti",
                WikiUrl = @"/wiki/Djibouti",
                GDP = 1589,
                FlagKey = @"Flag_of_Djibouti",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/3/34/Flag_of_Djibouti.svg?download",
                FlagWidth = 600,
                FlagHeight = 400,
            },
            new CountryWithFlag()
            {
                // flag is 330 bytes
                Name = @"Seychelles",
                WikiUrl = @"/wiki/Seychelles",
                GDP = 1511,
                FlagKey = @"Flag_of_Seychelles",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/f/fc/Flag_of_Seychelles.svg?download",
                FlagWidth = 900,
                FlagHeight = 450,
            },
            new CountryWithFlag()
            {
                // flag is 362 bytes
                Name = @"Saint Lucia",
                WikiUrl = @"/wiki/Saint_Lucia",
                GDP = 1406,
                FlagKey = @"Flag_of_Saint_Lucia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/9/9f/Flag_of_Saint_Lucia.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 625 bytes
                Name = @"Somalia",
                WikiUrl = @"/wiki/Somalia",
                GDP = 1375,
                FlagKey = @"Flag_of_Somalia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/a/a0/Flag_of_Somalia.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 483 bytes
                Name = @"Zanzibar",
                WikiUrl = @"/wiki/Zanzibar",
                GDP = 1289,
                FlagKey = @"Flag_of_Zanzibar",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/d/d4/Flag_of_Zanzibar.svg?download",
                FlagWidth = 900,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 769 bytes
                Name = @"Antigua and Barbuda",
                WikiUrl = @"/wiki/Antigua_and_Barbuda",
                GDP = 1248,
                FlagKey = @"Flag_of_Antigua_and_Barbuda",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/8/89/Flag_of_Antigua_and_Barbuda.svg?download",
                FlagWidth = 690,
                FlagHeight = 460,
            },
            new CountryWithFlag()
            {
                // flag is 706 bytes
                Name = @"Guinea-Bissau",
                WikiUrl = @"/wiki/Guinea-Bissau",
                GDP = 1209,
                FlagKey = @"Flag_of_Guinea-Bissau",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/0/01/Flag_of_Guinea-Bissau.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 862 bytes
                Name = @"Solomon Islands",
                WikiUrl = @"/wiki/Solomon_Islands",
                GDP = 1103,
                FlagKey = @"Flag_of_The_Solomon_Islands",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/7/74/Flag_of_the_Solomon_Islands.svg?download",
                FlagWidth = 800,
                FlagHeight = 400,
            },
            new CountryWithFlag()
            {
                // flag is 49510 bytes
                Name = @"Sint Maarten",
                WikiUrl = @"/wiki/Sint_Maarten",
                GDP = 1059,
                FlagKey = @"Flag_of_Sint_Maarten",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/d/d3/Flag_of_Sint_Maarten.svg?download",
                FlagWidth = 675,
                FlagHeight = 450,
            },
            new CountryWithFlag()
            {
                // flag is 21651 bytes
                Name = @"British Virgin Islands",
                WikiUrl = @"/wiki/British_Virgin_Islands",
                GDP = 902,
                FlagKey = @"Flag_of_The_British_Virgin_Islands",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/4/42/Flag_of_the_British_Virgin_Islands.svg?downloa" +
                             @"d",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 1505 bytes
                Name = @"Grenada",
                WikiUrl = @"/wiki/Grenada",
                GDP = 884,
                FlagKey = @"Flag_of_Grenada",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/b/bc/Flag_of_Grenada.svg?download",
                FlagWidth = 600,
                FlagHeight = 360,
            },
            new CountryWithFlag()
            {
                // flag is 883 bytes
                Name = @"Saint Kitts and Nevis",
                WikiUrl = @"/wiki/Saint_Kitts_and_Nevis",
                GDP = 852,
                FlagKey = @"Flag_of_Saint_Kitts_and_Nevis",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/f/fe/Flag_of_Saint_Kitts_and_Nevis.svg?download",
                FlagWidth = 750,
                FlagHeight = 500,
            },
            new CountryWithFlag()
            {
                // flag is 287 bytes
                Name = @"The Gambia",
                WikiUrl = @"/wiki/The_Gambia",
                GDP = 851,
                FlagKey = @"Flag_of_The_Gambia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/7/77/Flag_of_The_Gambia.svg?download",
                FlagWidth = 600,
                FlagHeight = 400,
            },
            new CountryWithFlag()
            {
                // flag is 723 bytes
                Name = @"Samoa",
                WikiUrl = @"/wiki/Samoa",
                GDP = 824,
                FlagKey = @"Flag_of_Samoa",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/3/31/Flag_of_Samoa.svg?download",
                FlagWidth = 2880,
                FlagHeight = 1440,
            },
            new CountryWithFlag()
            {
                // flag is 4007 bytes
                Name = @"Vanuatu",
                WikiUrl = @"/wiki/Vanuatu",
                GDP = 812,
                FlagKey = @"Flag_of_Vanuatu",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/b/bc/Flag_of_Vanuatu.svg?download",
                FlagWidth = 600,
                FlagHeight = 360,
            },
            new CountryWithFlag()
            {
                // flag is 20787 bytes
                Name = @"Turks and Caicos Islands",
                WikiUrl = @"/wiki/Turks_and_Caicos_Islands",
                GDP = 797,
                FlagKey = @"Flag_of_The_Turks_and_Caicos_Islands",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/a/a0/Flag_of_the_Turks_and_Caicos_Islands.svg?downl" +
                             @"oad",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 408 bytes
                Name = @"Saint Vincent and the Grenadines",
                WikiUrl = @"/wiki/Saint_Vincent_and_the_Grenadines",
                GDP = 729,
                FlagKey = @"Flag_of_Saint_Vincent_and_The_Grenadines",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/6/6d/Flag_of_Saint_Vincent_and_the_Grenadines.svg?d" +
                             @"ownload",
                FlagWidth = 450,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 767 bytes
                Name = @"Comoros",
                WikiUrl = @"/wiki/Comoros",
                GDP = 648,
                FlagKey = @"Flag_of_The_Comoros",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/9/94/Flag_of_the_Comoros.svg?download",
                FlagWidth = 1000,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 14136 bytes
                Name = @"Dominica",
                WikiUrl = @"/wiki/Dominica",
                GDP = 533,
                FlagKey = @"Flag_of_Dominica",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/c/c4/Flag_of_Dominica.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 315 bytes
                Name = @"Tonga",
                WikiUrl = @"/wiki/Tonga",
                GDP = 435,
                FlagKey = @"Flag_of_Tonga",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/9/9a/Flag_of_Tonga.svg?download",
                FlagWidth = 960,
                FlagHeight = 480,
            },
            new CountryWithFlag()
            {
                // flag is 784 bytes
                Name = @"Sao Tome and Principe",
                WikiUrl = @"/wiki/S%C3%A3o_Tom%C3%A9_and_Pr%C3%ADncipe",
                GDP = 337,
                FlagKey = @"Flag_of_Sao_Tome_and_Principe",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/4/4f/Flag_of_Sao_Tome_and_Principe.svg?download",
                FlagWidth = 2800,
                FlagHeight = 1400,
            },
            new CountryWithFlag()
            {
                // flag is 814 bytes
                Name = @"Federated States of Micronesia",
                WikiUrl = @"/wiki/Federated_States_of_Micronesia",
                GDP = 308,
                FlagKey = @"Flag_of_The_Federated_States_of_Micronesia",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/e/e4/Flag_of_the_Federated_States_of_Micronesia.svg" +
                             @"?download",
                FlagWidth = 760,
                FlagHeight = 400,
            },
            new CountryWithFlag()
            {
                // flag is 1474 bytes
                Name = @"Cook Islands",
                WikiUrl = @"/wiki/Cook_Islands",
                GDP = 311,
                FlagKey = @"Flag_of_The_Cook_Islands",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/3/35/Flag_of_the_Cook_Islands.svg?download",
                FlagWidth = 600,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 4119 bytes
                Name = @"Anguilla",
                WikiUrl = @"/wiki/Anguilla",
                GDP = 311,
                FlagKey = @"Flag_of_Anguilla",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/b/b4/Flag_of_Anguilla.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 214 bytes
                Name = @"Palau",
                WikiUrl = @"/wiki/Palau",
                GDP = 234,
                FlagKey = @"Flag_of_Palau",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/4/48/Flag_of_Palau.svg?download",
                FlagWidth = 800,
                FlagHeight = 500,
            },
            new CountryWithFlag()
            {
                // flag is 886 bytes
                Name = @"Marshall Islands",
                WikiUrl = @"/wiki/Marshall_Islands",
                GDP = 209,
                FlagKey = @"Flag_of_The_Marshall_Islands",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/2/2e/Flag_of_the_Marshall_Islands.svg?download",
                FlagWidth = 570,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 717 bytes
                Name = @"Nauru",
                WikiUrl = @"/wiki/Nauru",
                GDP = 182,
                FlagKey = @"Flag_of_Nauru",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/3/30/Flag_of_Nauru.svg?download",
                FlagWidth = 600,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 3257 bytes
                Name = @"Kiribati",
                WikiUrl = @"/wiki/Kiribati",
                GDP = 180,
                FlagKey = @"Flag_of_Kiribati",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/d/d3/Flag_of_Kiribati.svg?download",
                FlagWidth = 600,
                FlagHeight = 300,
            },
            new CountryWithFlag()
            {
                // flag is 12868 bytes
                Name = @"Montserrat",
                WikiUrl = @"/wiki/Montserrat",
                GDP = 63,
                FlagKey = @"Flag_of_Montserrat",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/d/d0/Flag_of_Montserrat.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
            new CountryWithFlag()
            {
                // flag is 1308 bytes
                Name = @"Tuvalu",
                WikiUrl = @"/wiki/Tuvalu",
                GDP = 38,
                FlagKey = @"Flag_of_Tuvalu",
                SvgFlagUrl = @"https://upload.wikimedia.org/wikipedia/commons/3/38/Flag_of_Tuvalu.svg?download",
                FlagWidth = 1200,
                FlagHeight = 600,
            },
        });
    }
}