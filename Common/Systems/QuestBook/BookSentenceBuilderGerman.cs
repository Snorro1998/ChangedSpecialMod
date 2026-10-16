using ChangedSpecialMod.Common.Systems.TagObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using static ChangedSpecialMod.Common.Systems.BookQuestSystem;

namespace ChangedSpecialMod.Common.Systems.QuestBook
{
    public class BookSentenceBuilderGerman : BookSentenceBuilder
    {

        public override string GetSillyTitle(BookTitleContext context)
        {
            var animals = context.Animal.GetPlural();

            var placeInHouse = Choose(
                "Keller",
                "Schlafzimmer",
                "Garten",
                "Haus",
                "Küche"
            );

            var familyMemberMan = Choose(
                "Bruder",
                "Vater",
                "Opa",
                "Onkel"
            );

            var familyMemberWoman = Choose(
                "Tante",
                "Oma",
                "Mutter",
                "Schwester"
            );

            return Choose(
                Choose(
                    $"Hilfe! Mein {familyMemberMan} hat sich in ein {context.Animal} verwandelt",
                    $"Hilfe! Meine {familyMemberWoman} hat sich in ein {context.Animal} verwandelt"
                ),
                
                $"der {context.Animal} zur emotionalen Unterstützung",
                $"Warum ist ein {context.Animal} in meiner {placeInHouse}?",
                $"Ich habe versehentlich {context.Number} {animals} im Internet gekauft"
            );
        }

        public override string GetUndergroundAdventure(string location, bool isCountry = false)
        {
            var destination = !isCountry ?
            Choose(
                $"Katakomben von {location}",
                $"Kanalisation unter {location}"
            ) :
            Choose(
                $"Höhlen in {location}",
                $"Bergwerke in {location}"
            );

            return Choose(
                $"Erkundung der {destination}",
                $"Abstieg in die {destination}",
                $"Tiefer in die {destination} vordringen"
            );
        }

        public override string GetCountryOfWorld(string countryEurope, string countryAfrica, string countryAsia, string country1)
        {
            var continentIndex = Main.rand.Next(0, 4);

            var continent = "der Welt";
            var country = country1;

            switch (continentIndex)
            {
                case 0:
                    continent = "Europas";
                    country = countryEurope;
                    break;

                case 1:
                    continent = "Afrikas";
                    country = countryAfrica;
                    break;

                case 2:
                    continent = "Asiens";
                    country = countryAsia;
                    break;
            }

            return $"Länder {continent}: {country}";
        }

        public override string GetSentenceVideoGame(string number1, VideoGameTagObjectList tagsVideoGames)
        {
            var videoGame = tagsVideoGames.PickRandom();
            var videoGameSocial = PickRandom(tagsVideoGames.elements.Where(x => x.isSocialGame).ToList());
            var videoGameNotSocial = PickRandom(tagsVideoGames.elements.Where(x => !x.isSocialGame).ToList());
            var videoGameRacer = PickRandom(tagsVideoGames.elements.Where(x => x.isRacingGame).ToList());
            var videoGameWithCharacters = PickRandom(tagsVideoGames.elements.Where(x => x.characterNames.Any()).ToList());
            var videoGameCharacter1 = PickRandom(videoGameWithCharacters.characterNames);
            var videoGameShooter = PickRandom(tagsVideoGames.elements.Where(x => x.isShootingGame).ToList());
            var videoGameWithNeighbors = PickRandom(tagsVideoGames.elements.Where(x => x.hasNeighborsHouse).ToList());

            var videoGameWithBoss = PickRandom(tagsVideoGames.elements.Where(x => x.bossNames.Any()).ToList());
            var videoGameBoss = PickRandom(videoGameWithBoss.bossNames);

            return Choose(
                $"Wie man in {videoGameWithNeighbors} Freundschaften mit seinen Nachbarn knüpft",
                Choose(
                    $"Wie man in {videoGameSocial} neue Leute kennenlernt",
                    $"{number1} Dinge, die du in {videoGameSocial} gemeinsam mit anderen Spielern unternehmen kannst"
                ),
                $"Wie man das Rennen in {videoGameRacer} gewinnt",
                $"Warum {videoGameCharacter1} der beste Charakter in {videoGameWithCharacters} ist",
                $"{number1} verborgene Geheimnisse von {videoGame}",
                $"{number1} Speedrunning-Tricks für {videoGameNotSocial}",
                $"Die besten Waffen aus {videoGameShooter}",
                $"Wie man {videoGameBoss} in {videoGameWithBoss} besiegt",
                // Only in German. YIPEEEEE
                $"Cola trinken und {videoGame} spielen"
            );
        }

        public override string GetSentencePlanet(PlanetTagObjectList planetTagObjectList)
        {
            var planet1 = planetTagObjectList.PickRandom();
            var planetWithSolidSurface = PickRandom(planetTagObjectList.elements.Where(x => !x.isGasPlanet).ToList());
            var planetWithMoons = PickRandom(planetTagObjectList.elements.Where(x => x.hasMoons).ToList());

            return Choose(
                $"die Weltraumexpedition zum {planet1}",
                $"Planeten jenseits des {planet1}",
                $"Leben auf {planetWithSolidSurface}",
                $"Die Geheimnisse des {planet1}",
                $"Der erste Astronaut auf {planetWithSolidSurface}",
                $"Was wir über {planet1} wissen",
                $"Wie man auf dem {planetWithSolidSurface} überlebt",
                $"die Monde des {planetWithMoons}"
            );
        }

        public override string GetSentenceConflict(string country1, string city1, string number2, string country2)
        {
            var professions1 = Choose(
                "Schmiede",
                "Busfahrer",
                "Zimmerleute",
                "Bauarbeiter",
                "Ärzte",
                "Förster",
                "Glasbläser",
                "Händler",
                "Maler",
                "Klempner",
                "Wissenschaftler",
                "Zauberer"
            );

            return Choose(
                $"Der Kampf um {country1}",
                $"Die Schlacht um {city1}",
                $"Der {number2}-jährige Krieg zwischen {country1} und {country2}",
                $"der Kampf zwischen den {professions1}"
            );
        }

        public override string GetSentenceCity(string number1, string city1)
        {
            return Choose(
                $"durch {city1} streifen",
                $"der lange Weg nach {city1}",
                $"{number1} Orte, die man in {city1} gesehen haben muss",
                $"Ein Reiseführer für {city1}",
                $"Die Geschichte {city1}s",
                //$"the battle of {city1}",
                $"Verloren in {city1}"
            );
        }

        public override string GetSentenceCountry(string country1, string countryEurope, string countryAfrica, string countryAsia)
        {
            return Choose(
                GetUndergroundAdventure(country1, true),
                Choose(
                    $"die Berge {country1}s besteigen", 
                    $"hinauf in die Berge {country1}s"
                ),
                $"Aufstieg und Fall {country1}s",
                $"eine Reise durch {country1}",
                $"Das ultimative Abenteuer in {country1}",
                $"Der verlorene Schatz {country1}s",
                $"die Reise durch {country1}",
                $"das Rennen quer durch {country1}",
                GetCountryOfWorld(countryEurope, countryAfrica, countryAsia, country1)
            );
        }

        public override string GetSentenceHowTo(string animalWithFur1, string animal1)
        {
            var thingToKnit = Choose(
                "ein Schal",
                "eine Mütze",
                "Socken",
                "Fäustlinge",
                "ein Pullover",
                "Puppenkleidung"
            );

            var invention1 = Choose(
                "Flugzeug",
                "Computer",
                "Kernreaktor",
                "Raketenschiff",
                "Dampfzug"
            );

            return Choose(
                $"Wie man aus {animalWithFur1}sfell entzückende Plüschtiere herstellt",
                $"Wie man {thingToKnit} strickt",
                $"Wie man einen Origami-{animal1} faltet",
                $"Wie man aus Haushaltsgegenständen ein {invention1} baut",
                $"Wie man einen {animal1} " +
                Choose(
                    $"streichelt",
                    $"aufzieht",
                    $"pflegt",
                    $"zähmt"
                )
            );
        }

        public override string GetSentenceAnimal(string animal1, string animals1, string city1, string country1)
        {
            var applianceWithInside1 = Choose(
                "Spülmaschine",
                "Wäschetrockner",
                "Mikrowelle",
                "Backofen",
                "Toaster",
                "Kühlschrank",
                "Waschmaschine"
            );

            return Choose(
                $"die Reise eines {animal1}es",
                $"der sehr hungrige {animal1}",
                $"Planet der {animals1}",
                $"Leben eines {animal1}s",
                $"Das geheime Leben eines {animals1}s",
                $"Ein Tag im Leben eines {animal1}s",
                $"Die Abenteuer eines {animal1}s",
                $"Der {animal1}, " +
                Choose(
                    "der die Situation rettete",
                    "der zum Helden wurde",
                    $"der in einer {applianceWithInside1} stecken blieb",
                    $"der nach {city1} reiste",
                    $"der nach {country1} reiste"
                ),
                $"Der verlorene {animal1}",
                $"Alles, was du über {animals1} wissen musst"
            );
        }

        public override string GetSentenceMythology(string country1)
        {
            var mythicalCreature1 = Choose(
                "Drachen",
                "Fee",
                "Riese",
                "Oger",
                "Vampir",
                "Werwolf"
            );

            var profession1 = Choose(
                "Schmied",
                "Busfahrer",
                "Zimmermann",
                "Bauarbeiter",
                "Arzt",
                "Förster",
                "Glasbläser",
                "Händler",
                "Maler",
                "Klempner",
                "Wissenschaftler",
                "Zauberer"
            );

            // Mythology and Folklore
            return Choose(
                $"Der {mythicalCreature1} aus {country1}",
                $"Wie man sich in einen {mythicalCreature1} verwandelt",
                //$"everything you need to know about {mythology1}",
                //$"the secret history of the {mythicalCreature1}",
                //$"why {mythicalCreatures1} disappeared from {country1}",
                $"Wie man einen {mythicalCreature1} trainiert",
                $"der {mythicalCreature1}, der {profession1} werden wollte",
                //$"what mythical creatures do on their day off",
                //$"the world's strangest legends about {animals1}",
                $"Wie man einen {mythicalCreature1} herbeiruft"
                //$"Wie man mit einem {mythicalCreature1} ausgeht"
            );
        }

        public override string GetSentenceFood(string number1, string vegetable1, string vegetables1, string country1, string fruits1)
        {
            return Choose(
                $"{number1} {vegetable1}rezepte",
                $"Wie man die perfekte {vegetable1} zubereitet",
                $"Rezepte aus {country1}",
                $"Das ultimative {country1}-Kochbuch",
                $"Wie man {vegetable1}-Smoothies zubereitet",
                $"{number1} Arten, {vegetables1} zuzubereiten",
                $"{number1} köstliche Dinge, die man aus {fruits1} zubereiten kann"
            //$"forgotten {vegetable1} recipes",
            //$"{number1} easy meals for beginners",
            //$"the lost recipes of {country1}",
            //$"{number1} foods you have to try"
            );
        }

        public override string GetSentenceTribe(string tribeWarrior, string tribe, string animal1)
        {
            var humanOrAnimalTribe = Choose(tribe, $"{animal1}sclan");

            return Choose(
                $"der erste Kontakt mit den {humanOrAnimalTribe}",
                $"Alles über die {humanOrAnimalTribe}",
                $"Wie der {humanOrAnimalTribe} lebt"
            );
        }

        public override string GetSentencePerson(string nameManWoman, string tribeWarrior, string tribe1, string animal1)
        {
            var tribeProfession = Choose(
                "Chief",
                "Oracle",
                "Scout",
                "Shaman",
                "Warrior"
            );

            var humanOrAnimalTribe = Choose(tribe1, $"{animal1} tribe");
            var personName = Choose(nameManWoman, tribeWarrior);

            return Choose(
                // Likulau: Warrior of the Rukai (bestest boi)
                $"{tribeWarrior}: {tribeProfession} der {humanOrAnimalTribe}",
                $"{personName}s Tagebuch"
            );
        }
    }
}
