using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace ChangedSpecialMod.Common.Systems.TagObjects
{
    public class VideoGameTagObject : TagObject
    {
        public bool isRacingGame = false;
        public bool isShootingGame = false;
        public bool isCivilizationGame = false;
        public bool isOnlineGame = false;
        public bool isSocialGame = false;

        public bool hasMonsterCatching = false;
        public bool hasBaseBuilding = false;
        public bool hasNeighborsHouse = false;

        public List<string> characterNames = new List<string>();
        public List<string> bossNames = new List<string>();
    }

    public class VideoGameTagObjectList : TagObjectList<VideoGameTagObject>
    {
        public override void Init()
        {
            /*
            "Bongo Cat",
            "Diablo",
            "Rollercoaster Tycoon",
            "Spore",
            // Bing bing wahoo is overrated
            //"Super Mario 64",
            */

            var gamesRacing = new List<VideoGameTagObject>()
            {
                new VideoGameTagObject()
                {
                    name = "Big Rigs"
                },

                new VideoGameTagObject()
                {
                    name = "Forza Horizon"
                },

                new VideoGameTagObject()
                {
                    name = "Garfield Kart"
                },

                new VideoGameTagObject()
                {
                    name = "Gran Turismo"
                },

                new VideoGameTagObject()
                {
                    name = "Grand Theft Auto V"
                },

                new VideoGameTagObject()
                {
                    name = "Need for Speed"
                },

                new VideoGameTagObject()
                {
                    name = "Trackmania"
                }
            };

            foreach (var game in gamesRacing)
                game.isRacingGame = true;

            var games = new List<VideoGameTagObject>()
            {
                new VideoGameTagObject()
                {
                    name = "Age of Empires",
                    isCivilizationGame = true
                },

                new VideoGameTagObject()
                {
                    name = "Bubsy 3D"
                },

                new VideoGameTagObject()
                {
                    name = "Capybara Spa"
                },

                new VideoGameTagObject()
                {
                    name = "Changed",
                    characterNames = new List<string>() { "Colin", "Puro", "Squid Dog", "Dr K", "Tiger Shark" }
                },

                new VideoGameTagObject()
                {
                    name = "Club Penguin",
                    isSocialGame = true
                },

                new VideoGameTagObject()
                {
                    name = "Cookie Clicker"
                },

                new VideoGameTagObject()
                {
                    name = "Dark Souls III"
                },

                new VideoGameTagObject()
                {
                    name = "Elden Ring",
                    characterNames = new List<string>() { "Blaidd" }
                },

                new VideoGameTagObject()
                {
                    name = "Empire Earth",
                    isCivilizationGame = true
                },

                new VideoGameTagObject()
                {
                    name = "Fall Guys",
                    isSocialGame = true
                },

                new VideoGameTagObject()
                {
                    name = "Farming Simulator 25"
                },

                new VideoGameTagObject()
                {
                    name = "Fortnite",
                    isShootingGame = true,
                    isOnlineGame = true,
                    characterNames = new List<string>() { "Dire", "Fennix", "Meowscles", "Oscar", "Volpez", "Wendell" }
                },

                new VideoGameTagObject()
                {
                    name = "Habbo Hotel",
                    isSocialGame = true
                },

                new VideoGameTagObject()
                {
                    name = "Half-Life 2",
                    characterNames = new List<string>() { "Alyx Vance", "Gordon Freeman", "G-Man" }
                },

                new VideoGameTagObject()
                {
                    name = "Hello Kitty Island Adventure"
                },

                new VideoGameTagObject()
                {
                    name = "League of Legends",
                    isSocialGame = true
                },

                new VideoGameTagObject()
                {
                    name = "Minecraft" ,
                    hasBaseBuilding = true,
                    characterNames = new List<string>() { "Alex", "Herobrine", "Steve" }
                },

                new VideoGameTagObject()
                {
                    name = "Palworld" ,
                    hasMonsterCatching = true,
                    hasBaseBuilding = true
                },

                new VideoGameTagObject()
                {
                    name = "Roblox",
                    isSocialGame = true
                },

                new VideoGameTagObject()
                {
                    name = "RuneScape",
                    isSocialGame = true
                },

                new VideoGameTagObject()
                {
                    name = "Stardew Valley",
                    hasNeighborsHouse = true,
                    characterNames = new List<string>()
                    { 
                        // M
                        "Alex",
                        "Clint",
                        "Demetrius",
                        "Elliott",
                        "George",
                        "Gus",
                        "Harvey",
                        "Kent",
                        "Krobus",
                        "Leo",
                        "Lewis",
                        "Linus",
                        "Pierre",
                        "Sam",
                        "Sebastian",
                        "Shane",
                        "Vincent",
                        "Willy",

                        // F
                        "Abigail",
                        "Caroline",
                        "Emily",
                        "Evelyn",
                        "Haley",
                        "Jas",
                        "Jodi",
                        "Leah",
                        "Marnie",
                        "Maru",
                        "Pam",
                        "Penny",
                        "Robin",
                        "Sandy"
                    }
                },

                new VideoGameTagObject()
                {
                    name = "Star Fox",
                    characterNames = new List<string>() { "Fox", "Falco", "Peppy", "Slippy" },
                    bossNames = new List<string>() { "Andross" }
                },

                new VideoGameTagObject()
                {
                    name = "Team Fortress 2" ,
                    isShootingGame = true,
                    isOnlineGame = true,
                    characterNames = new List<string>()
                    {
                        "Demoman",
                        "Engineer",
                        "Heavy",
                        "Medic",
                        "Pyro",
                        "Scout",
                        "Soldier",
                        "Spy"
                    }
                },

                new VideoGameTagObject()
                {
                    name = "Terraria",
                    bossNames = new List<string>() { "Moon Lord" }
                },

                new VideoGameTagObject()
                {
                    name = "The Legend of Zelda",
                    bossNames = new List<string>() { "Ganondorf" }
                },

                new VideoGameTagObject()
                {
                    name = "The Sims",
                    hasNeighborsHouse = true
                },

                new VideoGameTagObject()
                {
                    name = "Undertale",
                    characterNames = new List<string>()
                    {
                        "Asgore",
                        "Flowey",
                        "Frisk",
                        "Papyrus",
                        "Sans",
                        "Toriel"
                    }
                },

                new VideoGameTagObject()
                {
                    name = "VRChat",
                    isOnlineGame = true,
                    isSocialGame = true
                },

                new VideoGameTagObject()
                {
                    name = "World of Goo"
                },

                new VideoGameTagObject()
                {
                    name = "World of Warcraft",
                    isSocialGame = true
                },
            };

            elements =
            [
                .. gamesRacing,
                .. games
            ];
        }
    }
}