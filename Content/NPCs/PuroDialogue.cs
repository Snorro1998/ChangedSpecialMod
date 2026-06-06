using System.Collections.Generic;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
    public partial class Puro : ModNPC
    {
        public static DialogueObject DialogueNormal = new DialogueObject(
            "Puro",
            new List<DialogueElement>
            {
				// NPC
                new DialogueElement("NPCAngler", "Angry"),
                new DialogueElement("NPCDryad", "Question"),
                new DialogueElement("NPCGuide", "Question"),
                new DialogueElement("NPCNurse", "Happy"),
                new DialogueElement("NPCMerchant", "Happy"),
                new DialogueElement("NPCTavernKeep"),
                new DialogueElement("NPCTaxCollector", "Question"),
                new DialogueElement("NPCZoologist", "Happy"),
                
				// Books
				new DialogueElement("Book1"),
                new DialogueElement("Book2"),
                new DialogueElement("Book3"),
                new DialogueElement("Book4", "Happy"),
                new DialogueElement("Book5", "Happy"),
                new DialogueElement("Book6"),
                new DialogueElement("Book7", "Derp"),
                
				// Orange
                new DialogueElement("Orange1"),
                new DialogueElement("Orange2", "Derp"),
                new DialogueElement("Orange3"),
                new DialogueElement("Orange4", "Neutral"),
                new DialogueElement("Orange5"),
                new DialogueElement("Orange6"),

				// Normal
				new DialogueElement("Normal1", "Happy"),
				new DialogueElement("Normal2", "Happy"),

				// World Evil
                new DialogueElement("Crimson1"),
                new DialogueElement("Crimson2", "Shocked"),
                new DialogueElement("Corruption1", "Shocked"),
                
				// Changed
                new DialogueElement("Changed1"),
                new DialogueElement("Changed2"),
                
				//Rain
                new DialogueElement("Rain1"),
                new DialogueElement("Rain2"),

				//Thunder
                new DialogueElement("Thunder1"),
                new DialogueElement("Thunder2", "Shocked"),

				//Windy
				new DialogueElement("Windy1"),
                new DialogueElement("Windy2", "Shocked"),
                new DialogueElement("Windy3"),

                //Valentine
                new DialogueElement("Valentine1"),
                new DialogueElement("Valentine2"),

                //Oktoberfest
                new DialogueElement("Oktoberfest1"),
                new DialogueElement("Oktoberfest2"),
                new DialogueElement("Oktoberfest3"),
                new DialogueElement("Oktoberfest4"),

                //Halloween
                new DialogueElement("Halloween1"),
                new DialogueElement("Halloween2"),
                new DialogueElement("Halloween3", "Evil"),
                new DialogueElement("Halloween4", "Evil"),
                new DialogueElement("Halloween5"),
                
                //Xmas
                new DialogueElement("Xmas1"),
                new DialogueElement("Xmas2", "Question"),
                new DialogueElement("Xmas3", "Happy"),
                new DialogueElement("Xmas4"),
                new DialogueElement("Xmas5"),

				//Items
				new DialogueElement("PlayerHasOrange"),
                new DialogueElement("PlayerHasBook"),
                new DialogueElement("PlayerHasBookOfSkulls"),
                new DialogueElement("PlayerHasWaterBolt"),
                new DialogueElement("PlayerHasGoldenShower"),
                new DialogueElement("PlayerIsWearingBalloon"),
                new DialogueElement("PlayerIsWearingWeddingDress"),
                new DialogueElement("PlayerHasPurrpurr", "Shocked"),

                //Transfurs
                new DialogueElement("TransfurCub")
            }
        );

        public static DialogueObject DialoguePirates = new DialogueObject(
            "Puro.Pirates",
            new List<DialogueElement>
            {
                new DialogueElement("Pirates1"),
                new DialogueElement("Pirates2"),
                new DialogueElement("Pirates3"),
                new DialogueElement("Pirates4"),
                new DialogueElement("Pirates5")
            }
        );

        public static DialogueObject DialogueMartians = new DialogueObject(
            "Puro.Martians",
            new List<DialogueElement>
            {
                new DialogueElement("Martians1"),
                new DialogueElement("Martians2"),
                new DialogueElement("Martians3")
            }
        );

        public static DialogueObject DialogueBloodMoon = new DialogueObject(
            "Puro.BloodMoon",
            new List<DialogueElement>
            {
                new DialogueElement("BloodMoon1", "Question"),
                new DialogueElement("BloodMoon2", "Evil"),
                new DialogueElement("BloodMoon3", "Evil"),
                new DialogueElement("BloodMoon4", "Evil"),
                new DialogueElement("BloodMoon5", "Evil"),
            }
        );

        public static DialogueObject DialogueEclipse = new DialogueObject(
            "Puro.Eclipse",
            new List<DialogueElement>
            {
                new DialogueElement("Eclipse1"),
                new DialogueElement("Eclipse2"),
                new DialogueElement("Eclipse3"),
            }
        );

        public static DialogueObject DialogueParty = new DialogueObject(
            "Puro.Party",
            new List<DialogueElement>
            {
                new DialogueElement("Party1", "Shocked"),
                new DialogueElement("Party2"),
                new DialogueElement("Party3", "Question"),
                new DialogueElement("Party4", "Happy"),
                new DialogueElement("Party5"),
                new DialogueElement("Party6", "Embarrassed"),
                new DialogueElement("Party7", "Question"),
                new DialogueElement("Party8", "Derp"),
                new DialogueElement("Party9", "Happy"),
                new DialogueElement("Party10"),
                new DialogueElement("Party11"),
                new DialogueElement("Party12", "Happy")
            }
        );
    }
}
