using System.Collections.Generic;

namespace ChangedSpecialMod.Common.Systems.TagObjects
{
    public class CountryTagObject : TagObject
    {
        public string nationalityName = null;

        public CountryTagObject() : base()
        {
            tagCategory = "Country";
        }
    }

    public class CountryTagObjectList : TagObjectList<CountryTagObject>
    {
        public List<CountryTagObject> countriesEurope;
        public List<CountryTagObject> countriesAfrica;
        public List<CountryTagObject> countriesAsia;
        public List<CountryTagObject> countriesOther;

        public override void Init() 
        {
            countriesEurope = new List<CountryTagObject>()
            {
                new CountryTagObject() { name = "Austria", },
                new CountryTagObject() { name = "Belgium", },
                new CountryTagObject() { name = "Denmark", },
                new CountryTagObject() { name = "France", },
                new CountryTagObject() { name = "Hungary", },
                new CountryTagObject() { name = "Germany", },
                new CountryTagObject() { name = "Greenland", },
                new CountryTagObject() { name = "Italy", },
                new CountryTagObject() { name = "Poland",  },
                new CountryTagObject() { name = "Russia",  },
                new CountryTagObject() { name = "Spain",  },
            };

            countriesAfrica = new List<CountryTagObject>()
            {
                new CountryTagObject() { name = "Algeria", },
                new CountryTagObject() { name = "Angola", },
                new CountryTagObject() { name = "Congo", },
                new CountryTagObject() { name = "Egypt", },
                new CountryTagObject() { name = "Ethiopia", },
                new CountryTagObject() { name = "Mauritius",  },
            };

            countriesAsia = new List<CountryTagObject>()
            {
                new CountryTagObject() { name = "Afghanistan", },
                new CountryTagObject() { name = "China", },
                new CountryTagObject() { name = "India", },
                new CountryTagObject() { name = "Indonesia", },
            };

            countriesOther = new List<CountryTagObject>()
            {
                new CountryTagObject() { name = "America", },
                new CountryTagObject() { name = "Antarctica", },
                new CountryTagObject() { name = "Argentina", },
                new CountryTagObject() { name = "Aruba", },
                new CountryTagObject() { name = "Australia", },
                new CountryTagObject() { name = "Bolivia",  },
                new CountryTagObject() { name = "Brazil",  },
                new CountryTagObject() { name = "Canada",  },
                new CountryTagObject() { name = "Chad",  },
                new CountryTagObject() { name = "Chile",  },
                new CountryTagObject() { name = "Colombia",  },
                new CountryTagObject() { name = "Kazakhstan",  },
                new CountryTagObject() { name = "Libya",  },
                new CountryTagObject() { name = "Iran",  },
                new CountryTagObject() { name = "Mongolia",  },

                new CountryTagObject() { name = "Myanmar",  },
                new CountryTagObject() { name = "Pakistan",  },
                new CountryTagObject() { name = "Peru",  },
                new CountryTagObject() { name = "Mali",  },
                new CountryTagObject() { name = "Mauritania",  },
                new CountryTagObject() { name = "Mexico",  },
                new CountryTagObject() { name = "Mozambique",  },
                new CountryTagObject() { name = "Namibia",  },
                new CountryTagObject() { name = "Nigeria",  },
                new CountryTagObject() { name = "Saudi Arabia",  },
                new CountryTagObject() { name = "Somalia",  },
                new CountryTagObject() { name = "South Africa",  },
                new CountryTagObject() { name = "Sudan",  },
                new CountryTagObject() { name = "Tanzania",  },
                new CountryTagObject() { name = "Turkey",  },
                new CountryTagObject() { name = "Venezuela",  },
                new CountryTagObject() { name = "Zambia",  },

                // Pokemon
                new CountryTagObject() { name = "Kanto", },
                new CountryTagObject() { name = "Johto", },
                new CountryTagObject() { name = "Hoenn", },
                new CountryTagObject() { name = "Sinnoh", },
                new CountryTagObject() { name = "Unova", },
                new CountryTagObject() { name = "Kalos", },
                new CountryTagObject() { name = "Alola", },
                new CountryTagObject() { name = "Galar", },
                new CountryTagObject() { name = "Paldea", },

                // Digimon
                new CountryTagObject() { name = "The Digital World", },

                // Narnia
                new CountryTagObject() { name = "Narnia", },
                
                // Marvel
                new CountryTagObject() { name = "Wakanda", },
                new CountryTagObject() { name = "Latveria", },

                // LOTR
                new CountryTagObject() { name = "Gondor", },
                new CountryTagObject() { name = "Middle Earth", },
                new CountryTagObject() { name = "Mordor", },

                // GOT
                new CountryTagObject() { name = "Westeros", },

                // Dune
                new CountryTagObject() { name = "Arrakis", },

                // TES
                new CountryTagObject() { name = "Tamriel", },

                // TLOZ
                new CountryTagObject() { name = "Hyrule", },

                // WOW
                new CountryTagObject() { name = "Azeroth", },

                // Other
                new CountryTagObject() { name = "Atlantis", },
                new CountryTagObject() { name = "Hell", },
                new CountryTagObject() { name = "Oz", },
                new CountryTagObject() { name = "Wonderland", },
                
                // Ancient
                new CountryTagObject() { name = "Babylonia", },
                new CountryTagObject() { name = "Macedonia", },
                new CountryTagObject() { name = "Persia", },
            };

            elements = [
                .. countriesEurope,
                .. countriesAfrica,
                .. countriesAsia,
                .. countriesOther
            ];
        }
    }
}
