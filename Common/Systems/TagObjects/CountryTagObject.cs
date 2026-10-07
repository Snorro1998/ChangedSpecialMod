using System.Collections.Generic;

namespace ChangedSpecialMod.Common.Systems.TagObjects
{
    public class CountryTagObject : TagObject
    {
        public string nationalityName = null;
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
                new CountryTagObject() { name = "Austria", nationalityName = "Austrian" },
                new CountryTagObject() { name = "Belgium", nationalityName = "Belgian" },
                new CountryTagObject() { name = "Denmark", nationalityName = "Danish" },
                new CountryTagObject() { name = "France", nationalityName = "French" },
                new CountryTagObject() { name = "Hungary", nationalityName = "Hungarian" },
                new CountryTagObject() { name = "Germany", nationalityName = "German" },
                new CountryTagObject() { name = "Greenland", nationalityName = "Greenlandic" },
                new CountryTagObject() { name = "Italy", nationalityName = "Italian" },
            };

            countriesAfrica = new List<CountryTagObject>()
            {
                // this will not work for "from the congo"
                new CountryTagObject() { name = "Congo", nationalityName = "Congolese" },
                new CountryTagObject() { name = "Egypt", nationalityName = "Egyptian" },
                new CountryTagObject() { name = "Ethiopia", nationalityName = "Ethiopian" },
            };

            countriesAsia = new List<CountryTagObject>()
            {
                new CountryTagObject() { name = "China", nationalityName = "Chinese" },
                new CountryTagObject() { name = "India", nationalityName = "Indian" },
                new CountryTagObject() { name = "Indonesia", nationalityName = "Indonesian" },
            };

            countriesOther = new List<CountryTagObject>()
            {
                new CountryTagObject() { name = "Afghanistan", nationalityName = "Afghan" },
                new CountryTagObject() { name = "Algeria", nationalityName = "Algerian" },
                new CountryTagObject() { name = "America", nationalityName = "American" },
                new CountryTagObject() { name = "Angola", nationalityName = "Angolan" },
                new CountryTagObject() { name = "Antarctica", nationalityName = "Antarctican" },
                new CountryTagObject() { name = "Argentina", nationalityName = "Argentinian" },
                new CountryTagObject() { name = "Aruba", nationalityName = "Aruban" },
                new CountryTagObject() { name = "Australia", nationalityName = "Australian" },
                new CountryTagObject() { name = "Bolivia", nationalityName = "Bolivian" },
                new CountryTagObject() { name = "Brazil", nationalityName = "Brazilian" },
                new CountryTagObject() { name = "Canada", nationalityName = "Canadien" },
                new CountryTagObject() { name = "Chad", nationalityName = "Chadian" },
                new CountryTagObject() { name = "Chile", nationalityName = "Chilean" },
                new CountryTagObject() { name = "Colombia", nationalityName = "Colombian" },
                new CountryTagObject() { name = "Kazakhstan", nationalityName = "Kazakhstani" },
                new CountryTagObject() { name = "Libya", nationalityName = "Libyan" },
                new CountryTagObject() { name = "Iran", nationalityName = "Iranian" },
                new CountryTagObject() { name = "Mongolia", nationalityName = "Mongolian" },

                /*
                "Myanmar",
                "Pakistan",
                "Peru",
                "Poland",
                "Mali",
                "Mauritania",
                "Mauritius",
                "Mexico",
                "Mozambique",
                "Namibia",
                // Sensitive snowflakes cant handle anything anymore nowadays and might misread it
                // for the freaking hecking goofy ah n-word
                //"Niger",
                "Nigeria",
                "Russia",
                "Spain",
                "Saudi Arabia",
                "Somalia",
                "South Africa",
                "Sudan",
                "Tanzania",
                "Turkey",
                "Venezuela",
                "Zambia",

                // Pokemon
                "Kanto",
                "Johto",
                "Hoenn",
                "Sinnoh",
                "Unova",
                "Kalos",
                "Alola",
                "Galar",
                "Paldea",

                // Digimon
                "the Digital World",

                // Narnia
                "Narnia",

                // Marvel
                "Wakanda",
                "Latveria",

                // LOTR
                "Gondor",
                "Middle-earth",
                "Mordor",

                // GOT
                "Westeros",

                // Dune
                "Arrakis",

                // TES
                "Tamriel",

                // TLOZ
                "Hyrule",

                // WOW
                "Azeroth",

                "Atlantis",
                "Hell",
                "Oz",
                "Wonderland",
                */

                /*
                "Babylonia",
                "the Golden Horde",
                "Macedonia",
                "Persia",
                "the Umayyad Caliphate",
                "the Vandals",
                "the Yuan Dynasty",
                "the Maya Civilization",
                "the Zapotec Civilization",
                */
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
