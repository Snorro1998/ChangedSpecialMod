using ChangedSpecialMod.Common.Systems.ObjectLists;
using ChangedSpecialMod.Common.Systems.TagObjects;
using ChangedSpecialMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        private static List<string> optionsNamesOther = new List<string>()
        {
            // Minecraft
            "Herobrine",
            
            // Mod of Redemption
            "Calavia",
            "Daerel",
            "Erhan",
            "Kari Johansson",    // Patient zero
            "Zephos"
        };

        private static List<string> optionsNamesTribeWarrior = new List<string>()
        {
            "Aruk",
            "Aruvok",
            "Auralok",
            "Blaidd",   // Elden Ring
            "Bratok",
            "Drakanu",
            "Dravok",
            "Eyvind",
            "Kirik",    // Moontouched
            "Likulau",  // Nekojishi. Ofc we have to include the bestest boi
            "Lyall",    // Lyre
            "Noakai",   // Moontouched
            "Ranok",    // FBTW
            "Rheel",    // Kekswolf
            "Roku",
            "Vulgor",   // FBTW
            "Zuko"      // ATLA
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
                Choose(
                    $"Bedienungsanleitung für den {brandAndModelName} {appliance1}",
                    $"Die Kunst der {fieldOfStudy}"
                ),

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
            var fruit1 = tagsFruit.PickRandom();
            var fruits1 = fruit1.GetPlural();

            var vegetable1 = tagsVegetable.PickRandom();
            var vegetables1 = vegetable1.GetPlural();

            var listPlanets = new List<string>()
            {
                "Jupiter",
                "Mars",
                "Mercury",
                "Neptune",
                "Pluto",
                "Saturn",
                "Uranus",
                "Venus",
                "the Moon"
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

            // Not all tribes, but to easily organize them
            var tribe = Choose(
                "Awá",
                "Korubo",
                "Yanomami",
                "Piripkura",
                "Kawahiva",
                "Nukak",
                "Ayoreo",
                "Tagaeri",
                "Taromenane",
                "Rukai"
            );

            var brandAndModelName = $"{deviceBrandName} {deviceModelName} {number1}";

            var animal1 = tagsAnimals.PickRandom();
            var animals1 = animal1.GetPlural();
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

            var humanOrAnimalTribe = Choose(tribe, $"{animal1} tribe");
            var videoGame = PickRandom(optionsVideoGames);

            var profession1 = "a " + 
            Choose(
                "blacksmith",
                "bus driver",
                "carpenter",
                "glasblower",
                "merchant",
                "plumber",
                "wizard"
            );

            var naturalDisaster1 = Choose(
                "earthquake",
                "meteorite",
                "tsunami"
                //"volcanic eruption"
            );

            var tribeWarrior = PickRandom(optionsNamesTribeWarrior);

            var tribeProfession = Choose(
                "Chief",
                "Scout",
                "Warrior"
            );

            var options = new List<string>() 
            {
                // tribes
                Choose(
                    $"the first contact with the {humanOrAnimalTribe}",
                    $"everything about the {humanOrAnimalTribe}",
                    $"how the {humanOrAnimalTribe} live"
                ),

                // Likulau: Warrior of the Rukai (bestest boi)
                $"{tribeWarrior}: {tribeProfession} of the {humanOrAnimalTribe}",

                // Silly
                Choose(
                    $"Help! My sister turned herself into a {animal1}",
                    $"the emotional support {animal1}",
                    $"why is there a {animal1} in my kitchen?",
                    $"I accidentally bought {number1} {animals1} on the internet",
                    $"death, taxes and {nameWoman}"
                ),

                // Crafts
                Choose(
                    $"how you can make adorable plushies from {animalWithFur1} fur",
                    $"how to knit {thingToKnit}",
                    $"how to fold an origami {animal1}"
                ),

                // Animals
                Choose(
                    $"the journey of a {animal1}",
                    $"how to " +
                    Choose(
                        "pet",
                        "raise",
                        //"seduce",
                        "take care of",
                        "tame"
                    ) + $" {animal1.GetArticle()} {animal1}",
                    $"the very hungry {animal1}",
                    $"planet of the {animals1}",
                    $"life of {animal1.GetArticle()} {animal1}",
                    $"the secret life of {animals1}",
                    $"a day in the life of {animal1.GetArticle()} {animal1}",
                    $"the adventures of {animal1.GetArticle()} {animal1}",
                    $"the {animal1} who " +
                    Choose(
                        "saved the day",
                        "became a hero",
                        $"got stuck in a {applianceWithInside1}",
                        $"went to {city1}",
                        $"traveled to {country1}"
                    ),
                    $"the lost {animal1}",
                    //$"why {animals1} behave the way they do",
                    $"everything you need to know about {animals1}"
                ),

                // Fruits and vegetables
                Choose(
                    $"{number1} {vegetable1} recipes",
                    $"why {vegetables1} are good for your health",
                    $"how to make {vegetable1} smoothies",
                    $"how to grow a {fruit1} tree",
                    $"growing {vegetables1} in {placeToGrowFood}",
                    $"{number1} ways to cook {vegetable1}",
                    $"a guide to growing {vegetables1}",
                    $"{number1} delicious things you can make with {fruit1}"
                ),

                // climbing and spelunking
                Choose(
                    // Going underground
                    Choose("exploring the ", "going down into the ", "delving deeper into the ") +
                    Choose(
                        $"caves of {country1}",
                        $"mines of {country1}",
                        $"catacombs of {city1}",
                        $"sewers beneath {city1}"
                    ),

                    // Mountain climbing
                    Choose("climbing the ", "going up the ") +
                    $"mountains of {country1}"
                ),

                // Video games
                Choose(
                    $"{number1} easter eggs in {videoGame}",
                    $"{number1} speedrunning tricks for {videoGame}"
                ),

                // Space
                Choose(
                    $"the first expedition to {planet1}",
                    $"life on {planet1}",
                    $"the lost colony of {planet1}",
                    $"the mysteries of {planet1}",
                    $"the first astronaut on {planet1}",
                    //$"{number1} things we know about space",
                    $"the secret mission to {planet1}"
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
                    $"{nameManWoman}'s {vehicleWithAdj1}",
                    $"{nameManWoman}'s secret",
                    $"{nameManWoman}'s adventure",
                    $"{nameManWoman}'s journey",
                    //$"{nameManWoman}'s collection",
                    $"{nameManWoman}'s mysterious house",
                    $"{nameManWoman}'s greatest discoveries",
                    $"the disappearance of {nameManWoman}",
                    $"a letter from {nameManWoman}"
                    //$"the strange case of {nameManWoman}"
                ),

                                // City
                Choose(
                    $"{number1} places you should visit in {city1}",
                    $"the secrets of {city1}",
                    $"the history of {city1}",
                    //$"a walking tour of {city1}",
                    //$"the hidden streets of {city1}",
                    $"the strangest places in {city1}",
                    $"the best food in {city1}",
                    //$"a weekend in {city1}",
                    //$"the rise and fall of {city1}",
                    //$"the forgotten history of {city1}",
                    $"the legends of {city1}",
                    //$"the underground world of {city1}",
                    $"the secrets of {city1}",
                    $"the battle of {city1}",
                    $"the ghosts of {city1}",
                    $"the lost treasure of {city1}"
                ),

                /*


                // Practical
                Choose(
                    $"Instruction manual for the {brandAndModelName} {appliance1}",
                    $"the art of {fieldOfStudy}",
                    $"a beginner's guide to {fieldOfStudy}",
                    $"how to master {fieldOfStudy}",
                    $"the complete guide to {fieldOfStudy}",
                    $"common mistakes when learning {fieldOfStudy}",
                    $"{number1} useful tips for {fieldOfStudy}",
                    $"the history of {fieldOfStudy}",
                    $"the science behind {fieldOfStudy}",
                    $"a practical introduction to {fieldOfStudy}"
                ),

                // Travel
                Choose(
                    $"the travels of {nameManWoman}",
                    $"the space expedition to {planet1}",
                    $"the long road to {city1}",
                    $"a journey across {country1}",
                    $"{number1} places to see in {city1}",
                    $"the ultimate adventure in {country1}",
                    $"a traveler’s guide to {city1}",
                    $"the road trip through {country1}",
                    $"lost in {city1}",
                    $"around the world in {number2} days",
                    $"the journey beyond {planet1}",
                    $"wandering through {city1}"
                ),

                // Food and cooking
                Choose(
                    $"forgotten {vegetable1} recipes",
                    $"{number1} easy meals for beginners",
                    $"how to cook the perfect {vegetable1}",
                    $"the history of {fruit1}",
                    $"recipes from {country1}",
                    $"the ultimate {vegetable1} cookbook",
                    $"the lost recipes of {country1}",
                    $"{number1} foods you have to try"
                ),

                // History
                Choose(
                    $"the history of {city1}",
                    $"the rise and fall of {country1}",
                    //$"the forgotten kingdom of {country1}",
                    $"the battle of {city1}"
                    //$"{number2} years that changed {country1}",
                    //$"the lost civilization of {country1}",
                    //$"a history of {fieldOfStudy}",
                    //$"{number1} historical mysteries that remain unsolved"
                ),

                // Science
                Choose(
                    $"the science of {fieldOfStudy}",
                    $"how {fieldOfStudy} changed the world",
                    $"the future of {fieldOfStudy}",
                    $"a beginner's guide to {fieldOfStudy}",
                    $"the mysteries of {planet1}",
                    $"what we know about {planet1}",
                    $"{number1} incredible facts about the universe"
                ),

                // Inventions and technology
                Choose(
                    $"the invention of the {appliance1}",
                    $"how the {appliance1} changed the world",
                    $"the future of {fieldOfStudy}",
                    $"{number1} inventions that changed everyday life"
                ),

                // Mysteries
                Choose(
                    $"the mystery of {city1}",
                    $"the mystery of the missing {animal1}",
                    $"the secret of {nameManWoman}",
                    $"the lost treasure of {country1}",
                    $"the abandoned house in {city1}",
                    $"the strange case of {nameManWoman}",
                    $"the disappearance in {city1}",
                    $"the secret beneath {city1}",
                    $"the mysterious {animal1}",
                    $"{number1} mysteries that have never been solved"
                ),

                // Adventure
                Choose(
                    $"the adventure of {nameManWoman}",
                    $"the expedition to {planet1}",
                    $"the lost treasure of {country1}",
                    $"the journey through {city1}",
                    $"the hunt for the missing {animal1}",
                    $"the race across {country1}",
                    $"the secret map of {city1}",
                    $"the quest for the lost {thingToKnit}"
                ),

                // People and biography
                Choose(
                    $"the life of {nameManWoman}",
                    $"the story of {nameManWoman}",
                    $"a day in the life of {nameManWoman}",
                    $"{nameManWoman}'s greatest achievement",
                    $"{nameManWoman}'s journey to success",
                    $"the secrets of {nameManWoman}",
                    $"how {nameManWoman} changed the world",
                    $"the rise of {nameManWoman}",
                    $"the early life of {nameManWoman}",
                    $"the greatest challenge faced by {nameManWoman}",
                    $"what we can learn from {nameManWoman}"
                ),

                // Nature
                Choose(
                    $"the secret life of {animals1}",
                    $"a journey through the forest",
                    //$"the wild world of {animals1}",
                    $"how {animals1} survive in the wild",
                    $"{number1} incredible wonders of nature"
                ),

                // Fantasy
                Choose(
                    $"the dragon of {country1}",
                    $"the {animal1} who could speak",
                    $"the secret castle of {nameManWoman}",
                    $"the wizard of {city1}",
                    //$"the last dragon",
                    $"the magical {animal1}",
                    $"the crown of {country1}"
                ),

                // Survival
                Choose(
                    $"how to survive on {planet1}",
                    $"how {animals1} survive in the wild",
                    $"surviving alone in {country1}",
                    $"{number1} essential survival skills",
                    $"lost in {city1}: a survival guide"
                ),

                // Education
                Choose(
                    $"a beginner's guide to {fieldOfStudy}",
                    $"{number1} things everyone should know about {fieldOfStudy}",
                    $"how to learn {fieldOfStudy}",
                    $"the history of {fieldOfStudy}",
                    $"fun facts about {fieldOfStudy}",
                    $"the science of {fieldOfStudy}",
                    $"common mistakes in {fieldOfStudy}",
                    $"the complete guide to {fieldOfStudy}",
                    $"{fieldOfStudy} explained simply",
                    $"everything you need to know about {fieldOfStudy}"
                ),

                // Collections / Lists
                Choose(
                    $"{number1} amazing {animals1}",
                    $"{number1} places you should visit",
                    $"{number1} things you didn't know about {country1}",
                    $"{number1} facts about {animal1}",
                    $"{number1} mysteries of the world",
                    $"{number1} greatest inventions",
                    $"{number1} strangest places on Earth",
                    $"{number1} foods you have to try",
                    $"{number1} unbelievable historical events",
                    $"{number1} things to do in {city1}",
                    $"{number1} reasons to visit {country1}"
                ),

                // Problems and How-To
                Choose(
                    $"how to fix a broken {appliance1}",
                    $"how to grow {vegetables1}",
                    $"how to prepare for a trip to {country1}",
                    $"how to learn {fieldOfStudy}",
                    $"how to survive in {city1}",
                    $"how to make your own {thingToKnit}",
                    $"how to become an expert in {fieldOfStudy}",
                    $"how to avoid common mistakes in {fieldOfStudy}"
                ),

                // Professions
                Choose(
                    $"the battle between the {profession1}s",
                    $"a day in the life of a {profession1}",
                    $"how to become a {profession1}",
                    //$"the secret life of {professions1}",
                    //$"why {profession1}s never get enough sleep",
                    $"the world's strangest {profession1}",
                    //$"what happens when {profession1}s switch jobs",
                    $"the {profession1} who accidentally saved the world",
                    $"{number1} things nobody tells you about being a {profession1}"
                ),

                // Weather and Natural Disasters
                Choose(
                    //$"surviving a {weather1} without leaving your house",
                    $"what to do when a {naturalDisaster1} hits {city1}",
                    //$"the science behind {weather1}",
                    //$"why {weather1} makes everyone act weird",
                    //$"the world's most unusual weather phenomena",
                    $"a day when it rained {fruit1}"
                    //$"how {weather1} changed the history of {country1}"
                    //$"what would happen if it never stopped raining"
                ),
                /*
                // Mythology and Folklore
                Choose(
                    $"everything you need to know about {mythology1}",
                    $"the secret history of the {mythicalCreature1}",
                    $"why {mythicalCreatures1} disappeared from {country1}",
                    $"how to train a {mythicalCreature1}",
                    $"the {mythicalCreature1} who wanted to become a {profession1}",
                    //$"what mythical creatures do on their day off",
                    $"the world's strangest legends about {animals1}",
                    $"how to accidentally summon a {mythicalCreature1}"
                ),

                // Magic and Superpowers
                Choose(
                    //$"how to use {superpower1} responsibly",
                    //$"the disadvantages of having {superpower1}",
                    //$"what happens when everyone gets {superpower1}",
                    //$"how to hide a {magicalObject1} from your parents",
                    //$"the {magicalObject1} that ruined {city1}",
                    $"{number1} magical objects you should never touch",
                    //$"why {magicalObject1} should come with an instruction manual",
                    $"I accidentally gave {nameMan} a superpower"
                ),

                // Science and Technology
                Choose(
                    //$"how {technology1} will change {country1}",
                    //$"the history of {invention1}",
                    //$"what happens when {technology1} becomes self-aware",
                    //$"how to build a {invention1} with household objects",
                    //$"why {technology1} is secretly afraid of humans",
                    //$"the world's most useless inventions",
                    $"{number1} scientific discoveries that sound completely fake"
                    //$"a beginner's guide to traveling through time",
                    //$"the day {invention1} became illegal"
                ),

                // History
                Choose(
                    $"a day in the life of someone living during {historicalPeriod1}",
                    $"the strangest laws in the history of {country1}",
                    $"what people ate during {historicalPeriod1}",
                    $"how {city1} looked {timePeriod1} ago",
                    //$"the most ridiculous historical misunderstandings",
                    $"what would happen if {nameMan} traveled back to {historicalPeriod1}",
                    //$"the invention that accidentally changed the world",
                    $"how {event1} changed the course of history"
                ),

                // Food and Cooking
                Choose(
                    $"how to make the perfect {dish1}",
                    $"{number1} ways to improve {food1}",
                    $"why {food1} tastes better at midnight",
                    $"the history of {dish1}",
                    $"how to survive a week eating only {food1}",
                    $"the world's strangest food combinations",
                    $"what your favorite {food1} says about you",
                    $"the secret life of {food1}",
                    $"how to cook {dish1} without starting a small fire",
                    $"the ultimate guide to pairing {food1} with {drink1}"
                ),

                // Travel and Exploration
                Choose(
                    $"a travel guide to {city1} for {animals1}",
                    $"how to survive a week in {country1} without a map",
                    $"the hidden secrets of {placeWithArticle1}",
                    $"{number1} places you should visit before you disappear",
                    $"what happens when you get lost in {placeWithArticle1}",
                    $"the world's most unusual tourist attractions",
                    $"how to explore {building1} without waking anything up",
                    $"a journey across {country1} using only {transport1}",
                    $"the worst possible vacation in {city1}"
                ),

                // Hobbies and Sports
                Choose(
                    $"how to get started with {hobby1}",
                    $"why {sport1} is more dangerous than it looks",
                    $"the secret world of professional {hobby1}",
                    $"how to become a champion at {sport1}",
                    $"{number1} hobbies that sound made up",
                    $"what happens when {nameMan} takes up {hobby1}",
                    //$"the most competitive people in the world",
                    $"how to turn {hobby1} into a full-time career"
                ),

                // Mysteries and Investigations
                Choose(
                    $"the mystery of {mystery1}",
                    $"who stole all the {object1}s from {city1}?",
                    $"the case of the missing {animal1}",
                    //$"how to solve a mystery with absolutely no evidence",
                    $"the strangest unsolved mysteries in {country1}",
                    $"why everyone in {building1} is acting suspiciously",
                    //$"the detective who couldn't remember their own name",
                    $"what really happened during {event1}"
                ),

                // Everyday Life
                Choose(
                    $"how to survive a family dinner with {nameMan}",
                    //$"why mornings should be illegal",
                    //$"the art of avoiding responsibilities",
                    //$"how to look busy when you're doing absolutely nothing",
                    $"{number1} ways to ruin a perfectly normal day",
                    $"why {object1} understands you better than people do",
                    $"how to explain {emotion1} to a confused {animal1}",
                    //$"the secret rules everyone follows but nobody explains"
                ),

                // Buildings and Architecture
                Choose(
                    $"the history of {building1}",
                    $"how to build {building1} on a tiny budget",
                    $"what happens inside {building1} after midnight",
                    //$"the world's strangest buildings",
                    $"why {placeWithArticle1} is impossible to find",
                    $"how to renovate {building1} without angering its ghosts",
                    //$"the architect who accidentally designed a maze",
                    $"living in {building1} for {timePeriod1}"
                ),

                // Transportation
                Choose(
                    $"the history of the {transport1}",
                    $"how to operate a {transport1} with no experience",
                    $"why {transport1}s are much stranger than you think",
                    $"the worst journey in the history of {transport1}",
                    $"what happens when {transport1} breaks down in {country1}",
                    $"how to commute to work using only {animals1}",
                    $"the future of transportation in {city1}",
                    $"the {transport1} that nobody remembers building"
                ),

                // Emotions and Psychology
                Choose(
                    $"why people experience {emotion1}",
                    $"how to explain {emotion1} to an alien",
                    $"the psychology of people who collect {object1}s",
                    //$"why everyone makes terrible decisions when hungry",
                    $"how to understand someone who loves {food1}",
                    $"{number1} strange things the human brain does",
                    //$"what happens when you stop worrying about everything",
                    //$"the secret life of your inner monologue"
                ),

                // Organizations and Bureaucracy
                Choose(
                    $"a day inside {fictionalOrganization1}",
                    $"the completely unnecessary rules of {fictionalOrganization1}",
                    $"how to file a complaint against {nameMan}",
                    $"why the government banned {object1}s",
                    $"the paperwork required to adopt a {mythicalCreature1}",
                    $"how to get promoted in {fictionalOrganization1}",
                    $"the secret department responsible for lost {objects1}",
                    $"what happens when {fictionalOrganization1} loses its own paperwork"
                ),

                // Laws and Rules
                Choose(
                    $"how {law1} affects everyday life",
                    $"{number1} laws that make absolutely no sense",
                    //$"why you should never trust a sign that says 'Do Not Press'",
                    $"the consequences of breaking the rules of {fictionalOrganization1}",
                    //$"what happens when everyone follows every rule literally",
                    //$"how to write a law that nobody can understand",
                    $"the trial of {animal1} versus {nameMan}",
                    $"why {object1}s might be plotting against humanity"
                )
                */


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
