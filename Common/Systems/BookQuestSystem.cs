using ChangedSpecialMod.Common.Systems.ObjectLists;
using ChangedSpecialMod.Common.Systems.QuestBook;
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

        public sealed class BookTitleContext
        {
            public required AnimalTagObject Animal { get; init; }
            public required string City { get; init; }
            public required string Country { get; init; }
            public required string Number { get; init; }
        }

        public sealed class BookTitleTagLists
        {
            public required AnimalTagObjectList AnimalTagObjectList { get; init; }
            public required CountryTagObject CountryTagObject { get; init; }
            public required FruitTagObjectList FruitTagObjectList { get; init; }
            public required VegetableTagObjectList VegetableTagObjectList { get; init; }
        }

        public static string GetBookName()
        {
            BookSentenceBuilder builder = null;

            var culture = Language.ActiveCulture.Name;
            switch (culture)
            {
                case "en-US":
                    builder = new BookSentenceBuilderEnglish();
                    break;
                case "de-DE":
                    builder = new BookSentenceBuilderGerman();
                    break;
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

            if (builder == null)
                return "lost book";

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
            var tagsVideoGames = new VideoGameTagObjectList();
            tagsVideoGames.Init();
            var tagsCities = new CityTagObjectList();
            tagsCities.Init();

            var number1 = (Main.rand.Next(1, 20) * 100).ToString();
            var number2 = (Main.rand.Next(1, 10) * 10).ToString();
            var fruit1 = tagsFruit.PickRandom();
            var fruits1 = fruit1.GetPlural();

            var vegetable1 = tagsVegetable.PickRandom();
            var vegetables1 = vegetable1.GetPlural();

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
                "Ca",
                "Terra"
            );

            var deviceBrandName2 = Choose(
                "wie",
                "sing",
                "ny",
                "bi",
                "na",
                "men",
                "co",
                "tools"
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
                //"diplomacy",
                //"electronics",
                "gardening",
                //"hydraulics",
                //"navigation",
                //"negotiation",
                //"origami",
                "painting",
                //"persuasion",
                "photography",
                "poetry",
                //"sculpture",
                //"storytelling",
                //"teaching",
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
            var animalWithFur1 = PickRandom(tagsAnimals.elements.Where(x => x.hasFur).ToList()).GetName();

            var vehicleTag1 = tagsVehicles.PickRandom();
            var vehicleWithAdj1 = $"{vehicleTag1.GetAdjective()} {vehicleTag1.name}";

            // keywords
            var city1 = tagsCities.PickRandom().GetName();
            var countryObj1 = tagsCountry.PickRandom();
            var country1 = countryObj1.GetName();
            var country2 = tagsCountry.PickRandom(new List<string>() { countryObj1.name }).GetName();
            var countryEurope = PickRandom(tagsCountry.countriesEurope).GetName();
            var countryAfrica = PickRandom(tagsCountry.countriesAfrica).GetName();
            var countryAsia = PickRandom(tagsCountry.countriesAsia).GetName();
            var applianceWithInside1 = PickRandom(listApplicancesWithInside);
            var appliance1 = PickRandom(listAppliances);

            var nameMan = PickRandom(optionsNameMan);
            var nameWoman = PickRandom(optionsNameWoman);
            var nameManWoman = Choose(nameMan, nameWoman);

            var humanOrAnimalTribe = Choose(tribe, $"{animal1} tribe");
            
            var profession1 = 
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
                "Oracle",
                "Scout",
                "Shaman",
                "Warrior"
            );

            BookTitleContext bookContext = new BookTitleContext()
            {
                Animal = animal1,
                City = city1,
                Country = country1,
                Number = number1
            };

            var options = new List<string>()
            {
                builder.GetSillyTitle(bookContext),

                builder.GetSentenceCity(number1, city1),
                builder.GetSentenceCountry(country1, countryEurope, countryAfrica, countryAsia),

                builder.GetSentencePlanet(),
                builder.GetSentenceConflict(country1, city1, number2, country2),
                builder.GetSentenceVideoGame(number1, tagsVideoGames),
                builder.GetSentenceHowTo(animalWithFur1, thingToKnit, animal1.GetName()),
                builder.GetSentenceAnimal(animal1.GetName(), animals1, city1, country1),
                builder.GetSentenceMythology(country1),
                builder.GetSentenceFood(number1, vegetable1.GetName(), vegetable1.GetPlural(), country1, fruit1.GetPlural())
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
