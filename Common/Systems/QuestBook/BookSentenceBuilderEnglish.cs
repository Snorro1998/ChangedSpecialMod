using ChangedSpecialMod.Common.Systems.TagObjects;
using System.Linq;
using Terraria;
using static ChangedSpecialMod.Common.Systems.BookQuestSystem;

namespace ChangedSpecialMod.Common.Systems.QuestBook
{
    public class BookSentenceBuilderEnglish : BookSentenceBuilder
    {
        public override string GetSillyTitle(BookTitleContext context)
        {
            var animals = context.Animal.GetPlural();

            var placeInHouse = Choose(
                "basement",
                "bedroom",
                "garden",
                "house",
                "kitchen"
            );

            var familyMemberMan = Choose(
                "Brother",
                "Father",
                "Grandpa",
                "Uncle"
            );

            var familyMemberWoman = Choose(
                "Aunt",
                "Grandma",
                "Mother",
                "Sister"
            );

            return Choose(
                Choose(
                    $"Help! My {familyMemberMan} turned himself into a {context.Animal}",
                    $"Help! My {familyMemberWoman} turned herself into a {context.Animal}"
                ),
                $"the emotional support {context.Animal}",
                $"why is there a {context.Animal} in my {placeInHouse}?",
                $"I accidentally bought {context.Number} {animals} on the internet"
            );
        }

        public override string GetUndergroundAdventure(string location, bool isCountry = false)
        {
            var introduction = Choose(
                "exploring the",
                "going down into the",
                "delving deeper into the"
            );

            var destination = !isCountry ?
            Choose(
                $"catacombs of {location}",
                $"sewers beneath {location}"
            ) :
            Choose(
                $"caves of {location}",
                $"mines of {location}"
            );

            return $"{introduction} {destination}";
        }

        public override string GetCountryOfWorld(string countryEurope, string countryAfrica, string countryAsia, string country1)
        {
            var continentIndex = Main.rand.Next(0, 4);
            var continent = "the world";
            var country = country1;

            switch (continentIndex)
            {
                case 0:
                    continent = "Europe";
                    country = countryEurope;
                    break;
                case 1:
                    continent = "Africa";
                    country = countryAfrica;
                    break;
                case 2:
                    continent = "Asia";
                    country = countryAsia;
                    break;
            }

            return $"Countries of {continent}: {country}";
        }

        public override string GetSentenceVideoGame(string number1, VideoGameTagObjectList tagsVideoGames)
        {
            var videoGame = tagsVideoGames.PickRandom();
            var videoGameSocial = PickRandom(tagsVideoGames.elements.Where(x => x.isSocialGame).ToList());
            var videoGameRacer = PickRandom(tagsVideoGames.elements.Where(x => x.isRacingGame).ToList());
            var videoGameWithCharacters = PickRandom(tagsVideoGames.elements.Where(x => x.characterNames.Any()).ToList());
            var videoGameCharacter1 = PickRandom(videoGameWithCharacters.characterNames);
            var videoGameShooter = PickRandom(tagsVideoGames.elements.Where(x => x.isShootingGame).ToList());
            var videoGameWithNeighbors = PickRandom(tagsVideoGames.elements.Where(x => x.hasNeighborsHouse).ToList());

            var videoGameWithBoss = PickRandom(tagsVideoGames.elements.Where(x => x.bossNames.Any()).ToList());
            var videoGameBoss = PickRandom(videoGameWithBoss.bossNames);

            return Choose(
                $"How to befriend your neighbors in {videoGameWithNeighbors}",
                $"How to meet new people in {videoGameSocial}",
                $"How to win the race in {videoGameRacer}",
                $"Why {videoGameCharacter1} is the best character in {videoGameWithCharacters}",
                $"{number1} easter eggs in {videoGame}",
                $"{number1} speedrunning tricks for {videoGame}",
                $"The best weapons from {videoGameShooter}",
                $"How to defeat {videoGameBoss} in {videoGameWithBoss}"
            );
        }

        public override string GetSentencePlanet()
        {
            var planet1 = Choose(
                "Jupiter",
                "Mars",
                "Mercury",
                "Neptune",
                "Pluto",
                "Saturn",
                "Uranus",
                "Venus",
                "the Moon"
            );

            return Choose(
                $"the space expedition to {planet1}",
                $"planets beyond {planet1}",
                $"life on {planet1}",
                $"the mysteries of {planet1}",
                $"the first astronaut on {planet1}",
                $"what we know about {planet1}",
                $"how to survive on {planet1}"
            );
        }

        public override string GetSentenceConflict(string country1, string city1, string number2, string country2)
        {
            var professions1 = Choose(
                "blacksmiths",
                "bus drivers",
                "carpenters",
                "construction workers",
                "doctors",
                "forest rangers",
                "glassblowers",
                "merchants",
                "painters",
                "plumbers",
                "scientists",
                "wizards"
            );

            return Choose(
                $"the fight for {country1}",
                $"the battle of {city1}",
                $"the {number2} year war between {country1} and {country2}",
                $"the battle between the {professions1}"
            );
        }

        public override string GetSentenceCity(string number1, string city1)
        {
            return Choose(
                $"wandering through {city1}",
                $"the long road to {city1}",
                $"{number1} places to see in {city1}",
                $"a traveler’s guide to {city1}",
                $"the history of {city1}",
                //$"the battle of {city1}",
                $"lost in {city1}"
            );
        }

        public override string GetSentenceCountry(string country1, string countryEurope, string countryAfrica, string countryAsia)
        {
            return Choose(
                GetUndergroundAdventure(country1, true),
                Choose("climbing the ", "going up the ") + $"mountains of {country1}",
                $"the rise and fall of {country1}",
                $"a journey across {country1}",
                $"the ultimate adventure in {country1}",
                $"the lost treasure of {country1}",
                $"the road trip through {country1}",
                $"the race across {country1}",
                GetCountryOfWorld(countryEurope, countryAfrica, countryAsia, country1)
            );
        }

        public override string GetSentenceHowTo(string animalWithFur1, string thingToKnit, string animal1)
        {
            var invention1 = Choose(
                "airplane",
                "computer",
                "nuclear reactor",
                "rocket ship",
                "steam train"
            );

            return Choose(
                $"how you can make adorable plushies from {animalWithFur1} fur",
                $"how to knit {thingToKnit}",
                $"how to fold an origami {animal1}",
                $"How to build a {invention1} out of household items",
                $"how to " +
                Choose(
                    "pet",
                    "raise",
                    "care for",
                    "tame"
                ) + $" a {animal1}"
            );
        }

        public override string GetSentenceAnimal(string animal1, string animals1, string city1, string country1)
        {
            var applianceWithInside1 = Choose(
                "dishwasher",
                "dryer",
                "microwave",
                "oven",
                "toaster",
                "refrigerator",
                "washing machine"
            );

            return Choose(
                $"the journey of a {animal1}",
                $"the very hungry {animal1}",
                $"planet of the {animals1}",
                $"life of a {animal1}",
                $"the secret life of {animals1}",
                $"a day in the life of a {animal1}",
                $"the adventures of a {animal1}",
                $"the {animal1} who " +
                Choose(
                    "saved the day",
                    "became a hero",
                    $"got stuck in a {applianceWithInside1}",
                    $"went to {city1}",
                    $"traveled to {country1}"
                ),
                $"the lost {animal1}",
                $"everything you need to know about {animals1}"
            );
        }

        public override string GetSentenceMythology(string country1)
        {
            var mythicalCreature1 = Choose(
                "Dragon",
                "Fairy",
                "Giant",
                "Ogre",
                "Vampire",
                "Werewolf"
            );

            var profession1 = Choose(
                "blacksmith",
                "bus driver",
                "carpenter",
                "construction worker",
                "doctor",
                "forest ranger",
                "glassblower",
                "merchant",
                "painter",
                "plumber",
                "scientist",
                "wizard"
            );


            // Mythology and Folklore
            return Choose(
                $"The {mythicalCreature1} from {country1}",
                $"How to turn yourself into a {mythicalCreature1}",
                //$"everything you need to know about {mythology1}",
                //$"the secret history of the {mythicalCreature1}",
                //$"why {mythicalCreatures1} disappeared from {country1}",
                $"how to train a {mythicalCreature1}",
                $"the {mythicalCreature1} who wanted to become a {profession1}",
                //$"what mythical creatures do on their day off",
                //$"the world's strangest legends about {animals1}",
                $"how to summon a {mythicalCreature1}"
                //$"how to date a {mythicalCreature1}"
            );
        }

        public override string GetSentenceFood(string number1, string vegetable1, string vegetables1, string country1, string fruits1)
        {
            return Choose(
                $"{number1} {vegetable1} recipes",
                $"how to cook the perfect {vegetable1}",
                $"recipes from {country1}",
                $"the ultimate {vegetable1} cookbook",
                $"how to make {vegetable1} smoothies",
                $"{number1} ways to cook {vegetables1}",
                $"{number1} delicious things you can make with {fruits1}"
                //$"forgotten {vegetable1} recipes",
                //$"{number1} easy meals for beginners",
                //$"the lost recipes of {country1}",
                //$"{number1} foods you have to try"
            );
        }
    }
}
