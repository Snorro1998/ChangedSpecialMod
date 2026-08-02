using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{
    // This works even without boss checklist installed
    public class BossProgressionSystem : ModSystem
    {
        private static float bossProgressionNumber = 0;
        private static int checkInterval = 10;
        private int time = 0;
        private bool hasChecked = false;

        public override void PostUpdateWorld()
        {
            time = (int)((Main.GlobalTimeWrappedHourly) % checkInterval);

            if (time == 0 && !hasChecked)
            {
                UpdateBossProgressionNumber();
                hasChecked = true;
            }
            else if (time != 0 && hasChecked)
                hasChecked = false;
        }

        public override void OnWorldLoad()
        {
            UpdateBossProgressionNumber();
        }

        private static readonly Dictionary<string, float> CalamityBossValues = new()
        {
            { "DesertScourge", 1.6f },
            { "GiantClam", 1.61f },
            { "AcidRain1", 2.67f },
            { "Crabulon", 2.7f },
            { "HiveMind", 3.98f },
            { "Perforators", 3.99f },
            { "SlimeGod", 6.7f },
            { "Cryogen", 8.5f },
            { "AquaticScourge", 9.5f },
            { "AcidRain2", 9.51f },
            { "CragmawMire", 9.52f },
            { "BrimstoneElemental", 10.5f },
            { "CalamitasClone", 11.7f },
            { "GreatSandShark", 12.09f },
            { "Leviathan", 12.8f },
            { "AstrumAureus", 12.81f },
            { "PlaguebringerGoliath", 14.5f },
            { "Ravager", 16.5f },
            { "AstrumDeus", 17.5f },
            { "ProfanedGuardians", 18.5f },
            { "Dragonfolly", 18.6f },
            { "Providence", 19f },
            { "CeaselessVoid", 19.6f },
            { "StormWeaver", 19.61f },
            { "Signus", 19.62f },
            { "Polterghast", 20f },
            { "AcidRain3", 20.49f },
            { "Mauler", 20.491f },
            { "NuclearTerror", 20.492f },
            { "OldDuke", 20.5f },
            { "DevourerofGods", 21f },
            { "Yharon", 22f },
            { "ExoMechs", 22.99f },
            { "Calamitas", 23f },
            { "BossRush", 25.99f }
        };

        private static readonly Dictionary<string, float> CoraliteBossValues = new()
        {
            { "rediancie", 0.9f },
            { "babyicedragon", 3.1f },
            { "slimeemperor", 3.2f },
            { "bloodiancie", 8.2f },
            { "thunderveindragon", 11.1f },
            { "zacurrentdragon", 15.1f },
            { "nightmareplantera", 18.1f }
        };

        private static readonly Dictionary<string, float> ThoriumBossValues = new()
        {
            { "TheGrandThunderBird", 0.9f },
            { "PatchWerk", 2.51f },
            { "QueenJellyfish", 3.1f },
            { "Viscount", 3.85f },
            { "CorpseBloom", 3.9f },
            { "Illusionist", 5.2f },
            { "GraniteEnergyStorm", 6.4f },
            { "BuriedChampion", 6.5f },
            { "StarScouter", 6.9f },
            { "BoreanStrider", 7.2f },
            { "FallenBeholder", 7.8f },
            { "Lich", 11.6f },
            { "ForgottenOne", 13.8f },
            { "ThePrimordials", 19.5f }
        };

        // Vanilla bosses
        private static void AddBossValue(List<float> numbers, bool condition, float bossValue)
        {
            if (condition)
                numbers.Add(bossValue);
        }

        // Bosses from external mods
        private static void AddBosses(List<float> bossNumberList, Dictionary<string, float> bosses, Mod mod, string bossCheckMethodName = "BossDowned")
        {
            if (mod == null)
                return;
            foreach (var (bossName, bossValue) in bosses)
            {
                if ((bool)mod.Call(bossCheckMethodName, bossName))
                    bossNumberList.Add(bossValue);
            }
        }

        public static float GetBossProgressionNumber()
        {
            return bossProgressionNumber;
        }

        private static void UpdateBossProgressionNumber()
        {
            var numbers = new List<float>() { 0 };

            // Vanilla vars
            AddBossValue(numbers, NPC.downedSlimeKing, 1);
            AddBossValue(numbers, NPC.downedBoss1, 2);
            AddBossValue(numbers, NPC.downedBoss2, 3);
            AddBossValue(numbers, NPC.downedQueenBee, 4);
            AddBossValue(numbers, NPC.downedBoss3, 5);
            AddBossValue(numbers, NPC.downedDeerclops, 6);
            AddBossValue(numbers, Main.hardMode, 7);
            AddBossValue(numbers, NPC.downedQueenSlime, 8);
            AddBossValue(numbers, NPC.downedMechBoss2, 9);
            AddBossValue(numbers, NPC.downedMechBoss1, 10);
            AddBossValue(numbers, NPC.downedMechBoss3, 11);
            AddBossValue(numbers, NPC.downedPlantBoss, 12);
            AddBossValue(numbers, NPC.downedGolemBoss, 13);
            AddBossValue(numbers, NPC.downedFishron, 14);
            AddBossValue(numbers, NPC.downedEmpressOfLight, 15);
            // 16 is for betsy, but it is not tracked
            AddBossValue(numbers, NPC.downedAncientCultist, 17);
            AddBossValue(numbers, NPC.downedMoonlord, 18);

            // Our mod vars
            AddBossValue(numbers, DownedBossSystem.DownedWhiteTail, 1.5f);
            AddBossValue(numbers, DownedBossSystem.DownedWolfKing, 2.5f);
            AddBossValue(numbers, DownedBossSystem.DownedBehemoth, 5.5f);

            // Other mods
            AddBosses(numbers, CalamityBossValues, ModSupportSystem.modCalamity);
            AddBosses(numbers, CoraliteBossValues, ModSupportSystem.modCoralite);
            AddBosses(numbers, ThoriumBossValues, ModSupportSystem.modThorium, "GetDownedBoss");

            bossProgressionNumber = numbers.Max();

            //Main.NewText($"Update boss progression value... It is now {bossProgressionNumber}");
        }
    }
}
