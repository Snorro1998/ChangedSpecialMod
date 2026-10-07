using ChangedSpecialMod.Common.Systems.ObjectLists;
using ChangedSpecialMod.Common.Systems.TagObjects;
using ChangedSpecialMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;

namespace ChangedSpecialMod.Common.Systems
{
    public class BookQuestSystem
    {

        private static List<string> optionsVerbs = new List<string>()
        {
            "cook",
            "cuddle",
            "build",
            "defeat",
            "destroy",
            "devour",
            "drink",
            "eat",
            "grow",
            "pet",
            "raise",
            //"seduce",
            "slay",
            "tame",
            "teach"
        };

        private static List<string> optionsAdjectives = new List<string>()
        {
            "big",
            "brave",
            "dangerous",
            "deranged",
            "giant",
            "great",
            "haunted",
            "hungry",
            "mighty",
            "muscular",
            "naughty",
            "possessed",
            "scary",
            "small",
            "smelly"
        };

        private static List<string> optionsObjects = new List<string>()
        {
            "bed",
            "book",
            "brick",
            "computer",
            "dishwasher",
            "mug",
            "pencil",
            "shoe",
            "stone"
        };

        private static List<string> optionsNounWeapon = new List<string>()
        {
            "axe",
            "club",
            "crossbow",
            "dagger",
            "halberd",
            "mace",
            "spear",
            "sword"
        };

        private static List<string> optionsPlants = new List<string>()
        {
            "bonsai trees",
            "butterfly-bushes",
            "cacti",
            "dandelions",
            "foxgloves",
            "poppies",
            "roses",
            "sunflowers",
            "water lilies"
        };

        private static List<string> optionsFoodDiary = new List<string>()
        {
            "cheese"
        };

        private static List<string> optionsNumbers = new List<string>()
        {
            "100",
            "one hundred",
            "200",
            "two hundred",
            "300",
            "three hundred",
            "400",
            "four hundred",
            "500",
            "five hundred",
            "600",
            "six hundred",
            "700",
            "seven hundred",
            "800",
            "eight hundred",
            "900",
            "nine hundred",
            "1000",
            "one thousand",
            "2000",
            "two thousand",
        };

        // the ... Empire
        private static List<string> optionsEmpires = new List<string>()
        {
            "Byzantine",
            "Carthaginian",
            "Chola",
            "Cyrus's",
            "Ghaznavid",
            "Gupta",
            "Hittite",
            "Inca",
            "Kushan",
            "Achaemenid",
            "Maurya",
            "Assyrian",
            "Aksumite",
            "Akkadian",
            "Roman",
            "Sassanian",
            "Seleucid",
            "Seljuk",
            "Songhai",
            "Xiongnu",
            "Srivijaya",
            "Timurid",
            "Mongol",
            "Mughal",
            "Neo-Babylonian",
            "Ottoman",
            "Parthian",
        };

        // the ... kingdom
        private static List<string> optionsKingdoms = new List<string>
        {
            "Anglo-Saxon",  //kingdoms
            "Armenian",
            "Bosporan",
            "Celtic",       //kingdoms
            "Gothic",       //kingdoms
            "Ptolemaic",
            "Spartan",
            "Viking",       //kingdoms
        };

        private static List<string> optionsProfessions = new List<string>()
        {
            "barber",
            "blacksmith",
            "celebrity",
            "cook",
            "detective",
            "explorer",
            "farmer",
            "god",
            "goddess",
            "hero",
            "influencer",
            "king",
            "knight",
            "lawyer",
            "mage",
            "merchant",
            "musician",
            "nurse",
            "pirate",
            "queen",
            "scientist",
            "soldier",
            "spirit",
            "stranger",
            "streamer",
            "teacher",
            "warrior",
            "wizard",

            "animal lover",
            //"content creator",
            //"furry",
            "tree hugger",
            //"v-tuber"
        };

        private static List<string> optionsFamilyMembers = new List<string>()
        {
            "brother", 
            "father", 
            "mother", 
            "sister", 
            "uncle", 
            "grandmother", 
            "grandfather"
        };

        private static List<string> optionsNameMan = new List<string>()
        {
            "Aaron",
            "Adam",
            "Adrian",
            "Alexander",
            "Andrew",
            "Arthur",
            "Benjamin",
            "Caleb",
            "Charlie",
            "Christopher",
            "Daniel",
            "David",
            "Dominic",
            "Dylan",
            "Edward",
            "Elias",
            "Ethan",
            "Felix",
            "Gabriel",
            "George",
            "Henry",
            "Isaac",
            "Jack",
            "Jacob",
            "James",
            "Jonathan",
            "Joseph",
            "Julian",
            "Laurel",
            "Leo",
            "Liam",
            "Lucas",
            "Marcus",
            "Matthew",
            "Max",
            "Michael",
            "Nathan",
            "Nathaniel",
            "Nicholas",
            "Noah",
            "Oliver",
            "Oscar",
            "Ryan",
            "Samuel",
            "Sebastian",
            "Simon",
            "Theodore",
            "Thomas",
            "Vincent",
            "William",
            "Abdul",
            "Arnold",
            "Boris",
            "Colin",
            "Dave",
            "Donald",
            "John",
            "Marc",
            "Michael",
            "Puro",
            "Steven",
            "Theodore",
            "Vladimir"
        };

        private static List<string> optionsNameWoman = new List<string>()
        {
            "Alice",
            "Amalia",
            "Amara",
            "Amelia",
            "Anita",
            "Anna",
            "Aria",
            "Aurora",
            "Ava",
            "Charlotte",
            "Chloe",
            "Clara",
            "Cora",
            "Elena",
            "Eliza",
            "Ella",
            "Ellie",
            "Emily",
            "Emma",
            "Eva",
            "Evelyn",
            "Freya",
            "Grace",
            "Hannah",
            "Isabella",
            "Isabelle",
            "Ivy",
            "Julia",
            "Lara",
            "Layla",
            "Leah",
            "Lily",
            "Lucy",
            "Luna",
            "Lydia",
            "Maria",
            "Maya",
            "Mia",
            "Mila",
            "Monica",
            "Naomi",
            "Natalie",
            "Nora",
            "Olivia",
            "Rose",
            "Ruby",
            "Sarah",
            "Scarlett",
            "Sinbad",
            "Sofia",
            "Sophia",
            "Sophie",
            "Violet",
            "Zoe"
        };

        private static List<string> optionsDishes = new List<string>
        {
            "bread", 
            "broth", 
            "brownies", 
            "cake", 
            "cookies", 
            "cupcakes",
            "muffins",
            "pancakes", 
            "pie", 
            "pizza", 
            "soup", 
            "stew",
            "wraps"
        };

        private static List<string> optionsHobby = new List<string>()
        {
            "beekeeping", 
            "bodybuilding", 
            "candlemaking",
            "clockmaking",
            "cooking", 
            "dancing", 
            "farming",
            "gambling",
            "gaming",
            "glassblowing",
            "knitting", 
            //"necromancy", 
            "painting", 
            "programming", 
            "sculpting", 
            "sewing", 
            "singing",
            "swimming",
            //"witchcraft", 
            "woodworking",
            "wrestling",
            "writing"
        };

        private static List<string> optionsInstruments = new List<string>()
        {
            "cello",
            "drum",
            "flute",
            "guitar",
            "harmonica",
            "trombone",
            "trumpet",
            "tuba",
            "violin",
            "xylophone"
        };

        private static List<string> optionsBoardGames = new List<string>()
        {
            "Checkers",
            "Chess",
            "Magic: The Gathering",
            "Monopoly",
            "The Werewolves of Millers Hollow"
        };

        private static List<string> optionsVideoGames = new List<string>()
        {
            "Age of Empires",
            "Bubsy 3D",
            "Changed",
            "Diablo",
            "Fortnite",
            "Habbo Hotel",
            "Minecraft",
            "Roblox",
            "Rollercoaster Tycoon",
            "RuneScape",
            "Spore",
            "Stardew Valley",
            "Terraria",
            "World of Goo"
        };

        private static List<string> optionsNounTimePeriod = new List<string>()
        {
            "moments", 
            "hours", 
            "days", 
            "weeks", 
            "months", 
            "years"
        };

        private static List<string> optionsNounDevice = new List<string>()
        {
            "dishwasher",
            "refrigerator",
            "washing machine"
        };

        private static T Choose<T>(params T[] items)
        {
            return ChangedUtils.Choose(items);
        }

        public static T PickRandom<T>(List<T> input)
        {
            return Utils.SelectRandom(Main.rand, input.ToArray());
        }

        public static int countNumber = 0;

        public static string GetBookNameGerman()
        {
            // Setup
            var tagsFruit = new FruitTagObjectList();
            tagsFruit.Init();
            var tagsVegetable = new VegetableTagObjectList();
            tagsVegetable.Init();
            var tagsVehicles = new VehicleTagObjectList();
            tagsVehicles.Init();
            var tagsCountry = new CountryTagObjectList();
            tagsCountry.Init();
            var tagsAnimals = new AnimalTagObjectList();
            tagsAnimals.Init();

            var number1 = (Main.rand.Next(1, 20) * 100).ToString();
            var number2 = (Main.rand.Next(1, 10) * 10).ToString();
            var fruit1 = tagsFruit.PickRandom().GetName();

            var vegetableTag1 = tagsVegetable.PickRandom();
            var vegetable1 = vegetableTag1.GetName();
            var vegetables1 = vegetableTag1.GetPlural();

            var listPlanets = new List<string>()
            {
                "Jupiter",
                "Mars",
                "Merkur",
                "Neptun",
                "Pluto",
                "Saturn",
                "Uranus",
                "Venus"
            };

            var listApplicancesWithInside = new List<string>()
            {
                "Spülmaschine",
                "Wäschetrockner",
                "Mikrowelle",
                "Backofen",
                "Toaster",
                "Kühlschrank",
                "Waschmaschine"
            };

            var listApplicancesWithoutInside = new List<string>()
            {
                "Grill",
                "Mischer",
                "Kocher"
            };

            var listAppliances = new List<string>();
            listAppliances.AddRange(listApplicancesWithInside);
            listAppliances.AddRange(listApplicancesWithoutInside);

            var deviceBrandName1 = Choose(
                "Hau",      // Hua Huawei
                "Sum",      // Sam Samsung
                "Sa",       // So Sony
                "Motshi",   // Mithsi Mitshubishi
                "Ko",
                "Sua",      // Suavemente
                "Ca"
            );

            var deviceBrandName2 = Choose(
                "wie",
                "sing",
                "ny",
                "bi",
                "na",
                "men",
                "co"
            );

            var deviceBrandName = $"{deviceBrandName1}{deviceBrandName2}";

            var deviceModelName = Choose(
                "Maxi",
                "Mini",
                "Super",
                "Turbo",
                "Variotech"
            );

            var placeToGrowFood = Choose(
                "ein Gewächshaus",
                "kalte Umgebungen",
                "warme Umgebungen"
            );

            var fieldOfStudy = Choose(
                "Alchimie",
                "Astronomie",
                "Kalligraphie",
                "Kartographie",
                "Kochen",
                "Diplomatie",
                "Elektronik",
                "Gartenarbeit",
                "Hydraulik",
                "Navigation",
                "Verhandlung",
                "Origami",
                "Malerei",
                "Überzeugung",
                "Fotografie",
                "Poesie",
                "Bildhauerei",
                "Geschichtenerzählen",
                "Unterricht",
                "Holzbearbeitung"
            );

            var thingToKnit = Choose(
                "ein Schal",
                "eine Strickmütze",
                "Socken",
                "Fäustlinge",
                "ein Pullover",
                "Puppenkleidung"
            );

            var brandAndModelName = $"{deviceBrandName} {deviceModelName} {number1}";

            var animalTag1 = tagsAnimals.PickRandom();
            var animal1 = animalTag1.GetName();
            var animals1 = animalTag1.GetPlural();
            var animalWithFur1 = PickRandom(tagsAnimals.elements.Where(x => x.hasFur).ToList()).GetName();

            var vehicleTag1 = tagsVehicles.PickRandom();
            var vehicleWithAdj1 = $"{vehicleTag1.GetAdjective()} {vehicleTag1.GetName()}";

            // keywords
            var city1 = PickRandom(CityObjectList.GetElements()).name;
            var country1 = tagsCountry.PickRandom().GetName();
            var country2 = tagsCountry.PickRandom(new List<string>() { country1 }).GetName();
            var countryEurope = PickRandom(tagsCountry.countriesEurope).GetName();
            var countryAfrica = PickRandom(tagsCountry.countriesAfrica).GetName();
            var countryAsia = PickRandom(tagsCountry.countriesAsia).GetName();
            var planet1 = PickRandom(listPlanets);
            var applianceWithInside1 = PickRandom(listApplicancesWithInside);
            var appliance1 = PickRandom(listAppliances);

            var nameMan = PickRandom(optionsNameMan);
            var nameWoman = PickRandom(optionsNameWoman);
            var nameManWoman = Choose(nameMan, nameWoman);

            var options = new List<string>() 
            {
                // Animals
                Choose(
                    $"Die Reise eines {animal1}",
                    $"Wie man einen {animal1} aufzieht",
                    $"Der sehr hungrige {animal1}",
                    $"Planet der {animals1}",
                    Choose(
                        $"Der {animal1}, der in einem {applianceWithInside1} stecken blieb",
                        $"Der {animal1}, der nach {city1} ging"
                    )
                ),

                // Fruits and vegetables
                Choose(
                    $"{number1} {vegetable1}rezepte",
                    $"Warum {vegetables1} gut für die Gesundheit sind",
                    $"So bereitet man {vegetable1}-Smoothies zu",
                    $"Wie man einen {fruit1}baum anbaut",
                    $"Anbau von {vegetables1} in {placeToGrowFood}"
                ),

                // City
                Choose(
                    $"{number1} Orte, die Sie in {city1} besuchen sollten",
                    Choose(
                        $"Die Geheimnisse {city1}s",
                        $"Die Minen von {city1}",
                        $"Die Katakomben von {city1}",
                        $"Die Schlacht um {city1}"
                    )
                ),

                // Countries
                Choose(
                    $"Der Kampf um {country1}",
                    $"Der {number2}-jährige Krieg zwischen {country1} und {country2}",
                    Choose(
                        $"Länder Europas: {countryEurope}",
                        $"Länder Afrikas: {countryAfrica}",
                        $"Länder Asiens: {countryAsia}",
                        $"Länder der Welt: {country1}"
                    )
                ),

                // Possessive
                Choose(
                    $"{nameManWoman}'s Tagebuch",
                    $"{nameManWoman}'s {animal1}",
                    $"{nameManWoman}'s {vehicleWithAdj1}"
                ),

                // Practical
                $"Bedienungsanleitung für den {brandAndModelName} {appliance1}",
                $"Die Kunst der {fieldOfStudy}",

                // Crafts
                Choose(
                    $"Wie du aus {animalWithFur1}fell niedliche Plüschtiere machen kannst",
                    $"Wie man einen {thingToKnit} strickt",
                    $"Wie man einen Origami-{animal1} faltet"
                ),

                // Travel
                Choose(
                    $"Die Reisen von {nameManWoman}",
                    $"die Weltraumexpedition zum {planet1}",
                    $"Der lange Weg nach {city1}"
                ),

                /*
                // Misc
                Choose(
                    $"the battle for the {vegetables1}"
                )
                */
            };

            countNumber++;
            countNumber = countNumber % options.Count;
            var sentence = options[countNumber];
            // Make first letter uppercase
            if (sentence != null && sentence.Length > 0)
                sentence = sentence.Substring(0, 1).ToUpper() + sentence.Substring(1);
            return sentence;
        }

        public static string GetBookName()
        {
            var culture = Language.ActiveCulture.Name;
            switch (culture)
            {
                //case "en-US":
                //    return name;
                case "de-DE":
                    return GetBookNameGerman();
                    /*
                case "fr-FR":
                    return nameFrench ?? name;
                case "es-ES":
                    return nameSpanish ?? name;
                case "it-IT":
                    return nameItalian ?? name;
                case "pt-BR":
                    return nameBrazilianPortuguese ?? name;
                case "ru-RU":
                    return nameRussian ?? name;
                case "zh-Hans":
                    return nameChinese ?? name;
                    */
            }

            // Setup
            var tagsFruit = new FruitTagObjectList();
            tagsFruit.Init();
            var tagsVegetable = new VegetableTagObjectList();
            tagsVegetable.Init();
            var tagsVehicles = new VehicleTagObjectList();
            tagsVehicles.Init();
            var tagsCountry = new CountryTagObjectList();
            tagsCountry.Init();
            var tagsAnimals = new AnimalTagObjectList();
            tagsAnimals.Init();

            var number1 = (Main.rand.Next(1, 20) * 100).ToString();
            var number2 = (Main.rand.Next(1, 10) * 10).ToString();
            var fruit1 = tagsFruit.PickRandom().name;
            
            var vegetableTag1 = tagsVegetable.PickRandom();
            var vegetable1 = vegetableTag1.name;
            var vegetables1 = vegetableTag1.GetPlural();

            var listPlanets = new List<string>()
            {
                "Jupiter",
                "Mars",
                "Mercury",
                "Neptune",
                "Pluto",
                "Saturn",
                "Uranus",
                "Venus"
            };

            var listApplicancesWithInside = new List<string>()
            {
                "dishwasher",
                "dryer",
                "microwave",
                "oven",
                "toaster",
                "refrigerator",
                "washing machine"
            };

            var listApplicancesWithoutInside = new List<string>()
            {
                "barbecue",
                "mixer",
                "stove"
            };

            var listAppliances = new List<string>();
            listAppliances.AddRange(listApplicancesWithInside);
            listAppliances.AddRange(listApplicancesWithoutInside);

            var deviceBrandName1 = Choose(
                "Hau",      // Hua Huawei
                "Sum",      // Sam Samsung
                "Sa",       // So Sony
                "Motshi",   // Mithsi Mitshubishi
                "Ko",
                "Sua",      // Suavemente
                "Ca"
            );

            var deviceBrandName2 = Choose(
                "wie",
                "sing",
                "ny",
                "bi",
                "na",
                "men",
                "co"
            );

            var deviceBrandName = $"{deviceBrandName1}{deviceBrandName2}";

            var deviceModelName = Choose(
                "Maxi",
                "Mini",
                "Super",
                "Turbo",
                "Variotech"
            );

            var placeToGrowFood = Choose(
                "a greenhouse",
                "cold environments",
                "warm environments"
            );

            var fieldOfStudy = Choose(
                "alchemy",
                "astronomy",
                "calligraphy",
                "cartography",
                "cooking",
                "diplomacy",
                "electronics",
                "gardening",
                "hydraulics",
                "navigation",
                "negotiation",
                "origami",
                "painting",
                "persuasion",
                "photography",
                "poetry",
                "sculpture",
                "storytelling",
                "teaching",
                "woodworking"
            );

            var thingToKnit = Choose(
                "a scarf",
                "a beanie",
                "socks",
                "mittens",
                "a sweater",
                "doll clothes"
            );

            var brandAndModelName = $"{deviceBrandName} {deviceModelName} {number1}";

            var animalTag1 = tagsAnimals.PickRandom();
            var animal1 = animalTag1.GetName();
            var animals1 = animalTag1.GetPlural();
            var animalWithFur1 = PickRandom(tagsAnimals.elements.Where(x => x.hasFur).ToList()).name;

            var vehicleTag1 = tagsVehicles.PickRandom();
            var vehicleWithAdj1 = $"{vehicleTag1.GetAdjective()} {vehicleTag1.name}";

            // keywords
            var city1 = PickRandom(CityObjectList.GetElements()).name;
            var country1 = tagsCountry.PickRandom().name;
            var country2 = tagsCountry.PickRandom(new List<string>() { country1 }).name;
            var countryEurope = PickRandom(tagsCountry.countriesEurope).name;
            var countryAfrica = PickRandom(tagsCountry.countriesAfrica).name;
            var countryAsia = PickRandom(tagsCountry.countriesAsia).name;
            var planet1 = PickRandom(listPlanets);
            var applianceWithInside1 = PickRandom(listApplicancesWithInside);
            var appliance1 = PickRandom(listAppliances);

            var nameMan = PickRandom(optionsNameMan);
            var nameWoman = PickRandom(optionsNameWoman);
            var nameManWoman = Choose(nameMan, nameWoman);

            var options = new List<string>() 
            {
                // Animals
                Choose(
                    $"the journey of a {animal1}",
                    $"how to raise a {animal1}",
                    $"the very hungry {animal1}",
                    $"planet of the {animals1}",
                    Choose(
                        $"the {animal1} who got stuck in a {applianceWithInside1}",
                        $"the {animal1} who went to {city1}"
                    )
                ),

                // Fruits and vegetables
                Choose(
                    $"{number1} {vegetable1} recipes",
                    $"why {vegetables1} are good for your health",
                    $"how to make {vegetable1} smoothies",
                    $"how to grow a {fruit1} tree",
                    $"growing {vegetables1} in {placeToGrowFood}"
                ),

                // City
                Choose(
                    $"{number1} places you should visit in {city1}",
                    Choose(
                        $"the secrets of {city1}",
                        $"the mines of {city1}",
                        $"the catacombs of {city1}",
                        $"the battle of {city1}"
                    )
                ),

                // Countries
                Choose(
                    $"the fight for {country1}",
                    $"the {number2} year war between {country1} and {country2}",
                    Choose(
                        $"Countries of Europe: {countryEurope}",
                        $"Countries of Africa: {countryAfrica}",
                        $"Countries of Asia: {countryAsia}",
                        $"Countries of the world: {country1}"
                    )
                ),

                // Possessive
                Choose(
                    $"{nameManWoman}'s diary",
                    $"{nameManWoman}'s {animal1}",
                    $"{nameManWoman}'s {vehicleWithAdj1}"
                ),

                // Practical
                $"Instruction manual for the {brandAndModelName} {appliance1}",
                $"the art of {fieldOfStudy}",

                // Crafts
                Choose(
                    $"how you can make adorable plushies from {animalWithFur1} fur",
                    $"how to knit {thingToKnit}",
                    $"how to fold an origami {animal1}"
                ),

                // Travel
                Choose(
                    $"the travels of {nameManWoman}",
                    $"the space expedition to {planet1}",
                    $"the long road to {city1}"
                ),

                /*
                // Misc
                Choose(
                    $"the battle for the {vegetables1}"
                )
                */
            };

            countNumber++;
            countNumber = countNumber % options.Count;
            var sentence = options[countNumber];
            // Make first letter uppercase
            if (sentence != null && sentence.Length > 0)
                sentence = sentence.Substring(0, 1).ToUpper() + sentence.Substring(1);
            return sentence;
        }
    }
}
