using ChangedSpecialMod.Common.Systems.TagObjects;
using ChangedSpecialMod.Utilities;
using static ChangedSpecialMod.Common.Systems.BookQuestSystem;

namespace ChangedSpecialMod.Common.Systems.QuestBook
{
    public abstract class BookSentenceBuilder
    {
        protected static T Choose<T>(params T[] items)
        {
            return ChangedUtils.Choose(items);
        }

        public abstract string GetSillyTitle(BookTitleContext context);

        public abstract string GetUndergroundAdventure(string location, bool isCountry = false);

        public abstract string GetCountryOfWorld(
            string countryEurope,
            string countryAfrica,
            string countryAsia,
            string country1);

        public abstract string GetSentenceVideoGame(string number1, VideoGameTagObjectList videoGameTagObjectList);

        public abstract string GetSentencePlanet(PlanetTagObjectList planetTagObjectList);

        public abstract string GetSentenceConflict(string country1, string city1, string number2, string country2);

        public abstract string GetSentenceCity(string number1, string city1);
        
        public abstract string GetSentenceCountry(string country1, string countryEurope, string countryAfrica, string countryAsia);

        public abstract string GetSentenceHowTo(string animalWithFur1, string animal1);

        public abstract string GetSentenceAnimal(string animal1, string animals1, string city1, string country1);

        public abstract string GetSentenceMythology(string country1);

        public abstract string GetSentenceFood(string number1, string vegetable1, string vegetables1, string country1, string fruits1);

        public abstract string GetSentenceTribe(string tribeWarrior, string tribe, string animal1);

        public abstract string GetSentencePerson(string nameManWoman, string tribeWarrior, string tribe1, string animal1);
    }
}
