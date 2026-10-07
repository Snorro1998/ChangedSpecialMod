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
                new CountryTagObject() 
                { 
                    name = "Austria", 
                    nameGerman = "Österreich",
                    nationalityName = "Austrian" 
                },
                new CountryTagObject() 
                { 
                    name = "Belgium",
                    nameGerman = "Belgien", 
                    nationalityName = "Belgian" 
                },
                new CountryTagObject() 
                { 
                    name = "Denmark",
                    nameGerman = "Dänemark", 
                    nationalityName = "Danish" 
                },
                new CountryTagObject() 
                { 
                    name = "France",
                    nameGerman = "Frankreich", 
                    nationalityName = "French" 
                },
                new CountryTagObject() 
                { 
                    name = "Hungary",
                    nameGerman = "Ungarn", 
                    nationalityName = "Hungarian" 
                },
                new CountryTagObject() 
                { 
                    name = "Germany", 
                    nameGerman = "Deutschland",
                    nationalityName = "German" 
                },
                new CountryTagObject() 
                { 
                    name = "Greenland",
                    nameGerman = "Grönland", 
                    nationalityName = "Greenlandic" 
                },
                new CountryTagObject() 
                { 
                    name = "Italy",
                    nameGerman = "Italien", 
                    nationalityName = "Italian" 
                },
            };

            countriesAfrica = new List<CountryTagObject>()
            {
                // this will not work for "from the congo"
                new CountryTagObject() 
                { 
                    name = "Congo", 
                    nameGerman = "Kongo",
                    nationalityName = "Congolese" 
                },
                new CountryTagObject() 
                { 
                    name = "Egypt",
                    nameGerman = "Ägypten", 
                    nationalityName = "Egyptian" 
                },
                new CountryTagObject() 
                { 
                    name = "Ethiopia",
                    nameGerman = "Äthiopien", 
                    nationalityName = "Ethiopian" 
                },
            };

            countriesAsia = new List<CountryTagObject>()
            {
                new CountryTagObject() 
                { 
                    name = "China",
                    nameGerman = "China", 
                    nationalityName = "Chinese" 
                },
                new CountryTagObject() 
                { 
                    name = "India",
                    nameGerman = "Indien", 
                    nationalityName = "Indian" 
                },
                new CountryTagObject() 
                { 
                    name = "Indonesia", 
                    nameGerman = "Indonesien", 
                    nationalityName = "Indonesian" 
                },
            };

            countriesOther = new List<CountryTagObject>()
            {
                new CountryTagObject() 
                { 
                    name = "Afghanistan",
                    nameGerman = "Afghanistan", 
                    nationalityName = "Afghan" 
                },
                new CountryTagObject() 
                { 
                    name = "Algeria",
                    nameGerman = "Algerien", 
                    nationalityName = "Algerian" 
                },
                new CountryTagObject() 
                { 
                    name = "America",
                    nameGerman = "Amerika", 
                    nationalityName = "American" 
                },
                new CountryTagObject() 
                { 
                    name = "Angola",
                    nameGerman = "Angola", 
                    nationalityName = "Angolan" 
                },
                new CountryTagObject() 
                { 
                    name = "Antarctica",
                    nameGerman = "Antarktis", 
                    nationalityName = "Antarctican" 
                },
                new CountryTagObject() 
                { 
                    name = "Argentina", 
                    nameGerman = "Argentinien", 
                    nationalityName = "Argentinian" 
                },
                new CountryTagObject() 
                { 
                    name = "Aruba",
                    nameGerman = "Aruba", 
                    nationalityName = "Aruban" 
                },
                /*
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
                */
                // Pokemon
                new CountryTagObject()
                {
                    name = "Kanto",
                },
                new CountryTagObject()
                {
                    name = "Johto",
                },
                new CountryTagObject()
                {
                    name = "Hoenn",
                },
                new CountryTagObject()
                {
                    name = "Sinnoh",
                },
                new CountryTagObject()
                {
                    name = "Unova",
                },
                new CountryTagObject()
                {
                    name = "Kalos",
                },
                new CountryTagObject()
                {
                    name = "Alola",
                },
                new CountryTagObject()
                {
                    name = "Galar",
                },
                new CountryTagObject()
                {
                    name = "Paldea",
                },

                // Digimon
                new CountryTagObject()
                {
                    name = "the Digital World",
                    nameGerman = "die Digitale Welt",
                },

                // Narnia
                new CountryTagObject()
                {
                    name = "Narnia",
                },
                /*
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
