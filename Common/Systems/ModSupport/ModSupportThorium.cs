using System.Collections.Generic;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class ModSupportThorium : ModSupportData
    {
        public ModSupportThorium(Mod mod, string modName) : base(mod, modName) { }

        public override void Init()
        {
            bossProgressionList = new DownedBossObjectList()
            {
                mod = this.mod,
                modName = "Thorium",
                modCallMethodName = "GetDownedBoss",
                bosses = new List<DownedBossObject>()
                {
                    new DownedBossObject("TheGrandThunderBird", 0.9f),
                    new DownedBossObject("PatchWerk", 2.51f),
                    new DownedBossObject("QueenJellyfish", 3.1f),
                    new DownedBossObject("Viscount", 3.85f),
                    new DownedBossObject("CorpseBloom", 3.9f),
                    new DownedBossObject("Illusionist", 5.2f),
                    new DownedBossObject("GraniteEnergyStorm", 6.4f),
                    new DownedBossObject("BuriedChampion", 6.5f),
                    new DownedBossObject("StarScouter", 6.9f),
                    new DownedBossObject("BoreanStrider", 7.2f),
                    new DownedBossObject("FallenBeholder", 7.8f),
                    new DownedBossObject("Lich", 11.6f),
                    new DownedBossObject("ForgottenOne", 13.8f),
                    new DownedBossObject("ThePrimordials", 19.5f),
                }
            };
        }

        public override void AddTownNPCs()
        {
            AddTownNPC("Blacksmith", "ThoriumBlacksmith");
            AddTownNPC("Cobbler", "ThoriumCobbler");
            AddTownNPC("ConfusedZombie", "ThoriumConfusedZombie");
            AddTownNPC("Cook", "ThoriumCook");
            AddTownNPC("DesertAcolyte", "ThoriumDesertAcolyte");
            AddTownNPC("Diverman", "ThoriumDiverman");
            AddTownNPC("Druid", "ThoriumDruid");
            AddTownNPC("Spiritualist", "ThoriumSpiritualist");
            AddTownNPC("Tracker", "ThoriumTracker");
            AddTownNPC("WeaponMaster", "ThoriumWeaponMaster");
        }

        public override void AddItemsFruit()
        {
            AddItemFruit("Fig");
            AddItemFruit("Tamarind");
            AddItemFruit("Aril");
            AddItemFruit("Lychee");
            AddItemFruit("Kumquat");    //Main.hardMode
            AddItemFruit("Persimmon");  //Main.hardMode
            AddItemFruit("Soursop");    //NPC.downedPlantBoss
            AddItemFruit("Mangosteen"); //NPC.downedPlantBoss
            AddItemFruit("Cranberry");  //NPC.downedPlantBoss
            AddItemFruit("Raspberry");  //NPC.downedPlantBoss
        }

        public override void AddItemsHerb()
        {
            AddItemHerb("MarineKelp");
        }
    }
}
