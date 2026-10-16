using ChangedSpecialMod.Common.Systems.TagObjects;
using System.Linq;
using Terraria;
using static ChangedSpecialMod.Common.Systems.BookQuestSystem;

namespace ChangedSpecialMod.Common.Systems.QuestBook
{
    public class BookSentenceBuilderFrench : BookSentenceBuilder
    {
        public override string GetSillyTitle(BookTitleContext context)
        {
            var animals = context.Animal.GetPlural();

            var placeInHouse = Choose(
                "sous-sol",
                "chambre à coucher",
                "jardin",
                "maison",
                "cuisine"
            );

            var familyMemberMan = Choose(
                "frère",
                "père",
                "grand-père",
                "oncle"
            );

            var familyMemberWoman = Choose(
                "tante",
                "grand-mère",
                "mère",
                "sœur"
            );

            return Choose(
                Choose(
                    $"Au secours ! Mon {familyMemberMan} s’est transformé en {context.Animal}",
                    $"Au secours ! Ma {familyMemberWoman} s’est transformé en {context.Animal}"
                ),
                $"le {context.Animal} de soutien émotionnel",
                $"Pourquoi y a-t-il un {context.Animal} dans ma {placeInHouse} ?",
                $"J'ai acheté par accident {context.Number} {animals} sur Internet"
            );
        }

        public override string GetUndergroundAdventure(string location, bool isCountry = false)
        {
            var introduction = Choose(
                "explorer les",
                "descendre dans les",
                "s’enfoncer plus profondément dans les"
            );

            var destination = !isCountry ?
            Choose(
                $"catacombes de {location}",
                $"égouts sous {location}"
            ) :
            Choose(
                $"grottes de {location}",
                $"mines de {location}"
            );

            return $"{introduction} {destination}";
        }

        public override string GetCountryOfWorld(string countryEurope, string countryAfrica, string countryAsia, string country1)
        {
            var continentIndex = Main.rand.Next(0, 4);
            var continent = "du monde";
            var country = country1;

            switch (continentIndex)
            {
                case 0:
                    continent = "d’Europe";
                    country = countryEurope;
                    break;
                case 1:
                    continent = "d’Afrique";
                    country = countryAfrica;
                    break;
                case 2:
                    continent = "d’Asie";
                    country = countryAsia;
                    break;
            }

            return $"pays {continent}: {country}";
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
                $"Comment se lier d'amitié avec ses voisins dans {videoGameWithNeighbors}",
                Choose(
                    $"Comment rencontrer de nouvelles personnes dans {videoGameSocial}",
                    $"{number1} things you can do with other players in {videoGameSocial}"
                ),
                $"Comment gagner la course dans {videoGameRacer}",
                $"Pourquoi {videoGameCharacter1} est le meilleur personnage dans {videoGameWithCharacters}",
                $"{number1} secrets cachés dans {videoGame}",
                $"{number1} astuces de speedrunning pour {videoGameNotSocial}",
                $"Les meilleures armes de {videoGameShooter}",
                $"Comment vaincre {videoGameBoss} dans {videoGameWithBoss}"
            );
        }

        public override string GetSentencePlanet(PlanetTagObjectList planetTagObjectList)
        {
            var planet1 = planetTagObjectList.PickRandom();
            var planetWithSolidSurface = PickRandom(planetTagObjectList.elements.Where(x => !x.isGasPlanet).ToList());
            var planetWithMoons = PickRandom(planetTagObjectList.elements.Where(x => x.hasMoons).ToList());

            return Choose(
                $"l’expédition spatiale vers {planet1}",
                $"planètes au-delà de {planet1}",
                $"la vie sur {planetWithSolidSurface}",
                $"les mystères de {planet1}",
                $"le premier astronaute sur {planetWithSolidSurface}",
                $"Ce que nous savons sur {planet1}",
                $"Comment survivre sur {planetWithSolidSurface}",
                $"les lunes de {planetWithMoons}"
            );
        }

        public override string GetSentenceConflict(string country1, string city1, string number2, string country2)
        {
            var professions1 = Choose(
                "forgerons",
                "conducteurs de bus",
                "charpentiers",
                "ouvriers du bâtiment",
                "médecins",
                "gardes forestiers",
                "verriers",
                "commerçants",
                "peintres",
                "plombiers",
                "scientifiques",
                "sorciers"
            );

            return Choose(
                $"la lutte pour la {country1}",
                $"la bataille de {city1}",
                $"La guerre de {number2} ans entre la {country1} et la {country2}",
                $"la bataille entre les {professions1}"
            );
        }

        public override string GetSentenceCity(string number1, string city1)
        {
            return Choose(
                $"flâner dans {city1}",
                $"Le long chemin vers {city1}",
                $"{number1} lieux à voir à {city1}",
                $"Guide du voyageur à {city1}",
                $"L'histoire de {city1}",
                //$"the battle of {city1}",
                $"Perdu à {city1}"
            );
        }

        public override string GetSentenceCountry(string country1, string countryEurope, string countryAfrica, string countryAsia)
        {
            return Choose(
                GetUndergroundAdventure(country1, true),
                Choose("gravir ", "monter dans ") + $"les montagnes de {country1}",
                $"L’ascension et la chute de la {country1}",
                $"un voyage à travers la {country1}",
                $"L’aventure ultime en {country1}",
                $"Le trésor perdu de la {country1}",
                $"le voyage en voiture à travers la {country1}",
                $"la course à travers la {country1}",
                GetCountryOfWorld(countryEurope, countryAfrica, countryAsia, country1)
            );
        }

        public override string GetSentenceHowTo(string animalWithFur1, string animal1)
        {
            var thingToKnit = Choose(
                "une écharpe",
                "un bonnet",
                "des chaussettes",
                "des mitaines",
                "un pull",
                "des vêtements pour poupée"
            );

            var invention1 = Choose(
                "avion",
                "ordinateur",
                "réacteur nucléaire",
                "fusée",
                "train à vapeur"
            );

            return Choose(
                $"Comment réaliser d’adorables peluches à partir de fourrure de {animalWithFur1}",
                $"Comment tricoter {thingToKnit}",
                $"Comment plier un {animal1} en origami",
                $"Comment construire un {invention1} à partir d’objets du quotidien",
                $"Comment " +
                Choose(
                    "caresser un",
                    "élever un",
                    "prendre soin d’un",
                    "apprivoiser"
                ) + $" {animal1}"
            );
        }

        public override string GetSentenceAnimal(string animal1, string animals1, string city1, string country1)
        {
            var applianceWithInside1 = Choose(
                "lave-vaisselle",
                "séchoir",
                "micro-ondes",
                "four",
                "grille-pain",
                "réfrigérateur",
                "machine à laver"
            );

            return Choose(
                $"le périple d’un {animal1}",
                $"le {animal1} très affamé",
                $"la planète des {animals1}",
                $"vie d’un {animal1}",
                $"La vie secrète des {animals1}",
                $"Une journée dans la vie d’un {animal1}",
                $"les aventures d’un {animal1}",
                $"Le {animal1} qui " +
                Choose(
                    $"a sauvé la mise",
                    $"est devenu un héros",
                    $"s’est retrouvé coincé dans un {applianceWithInside1}",
                    $"est allé à {city1}",
                    $"a voyagé en {country1}"
                ),
                $"le {animal1} perdu",
                $"Tout ce qu'il faut savoir sur les {animals1}"
            );
        }

        public override string GetSentenceMythology(string country1)
        {
            var mythicalCreature1 = Choose(
                "Dragon",
                "Fée",
                "Géant",
                "Ogre",
                "Vampire",
                "Loup-garou"
            );

            var profession1 = Choose(
                "forgeron",
                "conducteur de bus",
                "charpentier",
                "ouvrier du bâtiment",
                "médecin",
                "garde forestier",
                "souffleur de verre",
                "marchand",
                "peintre",
                "plombier",
                "scientifique",
                "sorcier"
            );


            // Mythology and Folklore
            return Choose(
                $"Le {mythicalCreature1} de {country1}",
                $"Comment se transformer en {mythicalCreature1}",
                //$"everything you need to know about {mythology1}",
                //$"the secret history of the {mythicalCreature1}",
                //$"why {mythicalCreatures1} disappeared from {country1}",
                $"Comment dresser un {mythicalCreature1}",
                $"Le {mythicalCreature1} qui voulait devenir {profession1}",
                //$"what mythical creatures do on their day off",
                //$"the world's strangest legends about {animals1}",
                $"Comment invoquer un {mythicalCreature1}"
            //$"how to date a {mythicalCreature1}"
            );
        }

        public override string GetSentenceFood(string number1, string vegetable1, string vegetables1, string country1, string fruits1)
        {
            return Choose(
                $"{number1} recettes à base de {vegetable1}",
                $"Comment préparer la {vegetable1} parfaite",
                $"Recettes de {country1}",
                $"Le livre de cuisine ultime sur la {vegetable1}",
                $"Comment préparer des smoothies aux {vegetables1}",
                $"{number1} façons de cuisiner les {vegetables1}",
                $"{number1} choses délicieuses que vous pouvez préparer avec des {fruits1}"
            //$"forgotten {vegetable1} recipes",
            //$"{number1} easy meals for beginners",
            //$"the lost recipes of {country1}",
            //$"{number1} foods you have to try"
            );
        }

        public override string GetSentenceTribe(string tribeWarrior, string tribe, string animal1)
        {
            var humanOrAnimalTribe = Choose(tribe, $"tribu des {animal1}");

            return Choose(
                $"le premier contact avec les {humanOrAnimalTribe}",
                $"Tout ce qui concerne les {humanOrAnimalTribe}",
                $"Comment vivent les {humanOrAnimalTribe}"
            );
        }

        public override string GetSentencePerson(string nameManWoman, string tribeWarrior, string tribe1, string animal1)
        {
            var tribeProfession = Choose(
                "Chef",
                "Oracle",
                "Éclaireur",
                "Chaman",
                "Guerrier"
            );

            var humanOrAnimalTribe = Choose(tribe1, $"tribu des {animal1}");
            var personName = Choose(nameManWoman, tribeWarrior);

            return Choose(
                // Likulau: Warrior of the Rukai (bestest boi)
                $"{tribeWarrior}: {tribeProfession} des {humanOrAnimalTribe}",
                $"Journal de {personName}"
            );
        }
    }
}
