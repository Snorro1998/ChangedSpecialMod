using System.Collections.Generic;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class ModSupportCoralite : ModSupportData
    {
        public ModSupportCoralite(Mod mod, string modName) : base(mod, modName) { }

        public override void Init()
        {
            bossProgressionList = new DownedBossObjectList()
            {
                mod = this.mod,
                modName = "Coralite",
                bosses = new List<DownedBossObject>()
                {
                    new DownedBossObject("Rediancie", 0.9f, "rediancie" ),
                    new DownedBossObject("BabyIceDragon", 3.1f, "babyicedragon"),
                    new DownedBossObject("SlimeEmperor", 3.2f, "slimeemperor"),
                    new DownedBossObject("Bloodiancie", 8.2f, "bloodiancie"),
                    new DownedBossObject("ThunderVeinDragon", 11.1f, "thunderveindragon"),
                    new DownedBossObject("ZacurrentDragon", 15.1f, "zacurrentdragon"),
                    new DownedBossObject("NightmarePlantera", 18.1f, "nightmareplantera"),
                }
            };
        }

        public override void AddTownNPCs()
        {
            AddTownNPC("CrystalRobot", "CoraliteCrystalRobot");
            AddTownNPC("ElfRanger", "CoraliteElfRanger");
        }

        public override void AddTiles()
        {
            // Crystal cave
            AddTileType("BasaltTile", true);
            AddTileType("CrystalBasaltTile", true);
            AddTileType("HardBasaltTile", true);
            AddTileType("MagicCrystalBrickTile", true);
        }
    }
}
