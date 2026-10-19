using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{
    public class DownedBossObject
    {
        public string bossName = null;
        public string bossCallName = null;
        public string bossReflectionName = null;
        public float progressionNumber = 1f;
        public bool downed = false;

        public DownedBossObject(string bossName, float progressionNumber, string bossCallName = null, string bossReflectionName = null)
        {
            this.bossName = bossName;
            this.progressionNumber = progressionNumber;
            this.bossCallName = bossCallName;
            this.bossReflectionName = bossReflectionName;
        }
    }

    public class DownedBossObjectList
    {
        public Mod mod = null;
        public string modName = "Unknown mod";
        // Mod.Call
        public string modCallMethodName = "BossDowned";
        // If the mod does not have Mod.Call but does have a public static class with bools
        public string reflectionPath = null;
        public bool dontCheck = false;

        public List<DownedBossObject> bosses = new List<DownedBossObject>();

        public void UpdateBosses()
        {
            if (modName == "Terraria")
            {
                UpdateTerrariaBosses();
                return;
            }

            if (modName == "Changed")
            {
                UpdateChangedBosses();
                return;
            }

            if (mod == null || bosses == null || bosses.Count == 0)
                return;
            
            try
            {
                if (reflectionPath != null)
                {
                    Type redeBossDownedType = mod.Code.GetType(reflectionPath);

                    if (redeBossDownedType == null)
                        return;

                    foreach (var boss in bosses)
                    {
                        FieldInfo field = redeBossDownedType.GetField(
                            boss.bossReflectionName,
                            BindingFlags.Public | BindingFlags.Static
                        );

                        if (field == null)
                            continue;

                        boss.downed = (bool)field.GetValue(null);
                    }
                }
                else if (modCallMethodName != null)
                {
                    foreach (var boss in bosses)
                    {
                        var bossCallName = !string.IsNullOrEmpty(boss.bossCallName) ? boss.bossCallName : boss.bossName;
                        boss.downed = (bool)mod.Call(modCallMethodName, bossCallName);
                    }
                }
            }
            catch
            {
                // Don't keep checking this boss list if it throws errors
                dontCheck = true;
            }
        }

        private void UpdateTerrariaBosses()
        {
            foreach (var boss in bosses)
            {
                switch(boss.bossName)
                {
                    case "KingSlime":
                        boss.downed = NPC.downedSlimeKing;
                        break;
                    case "EyeOfCthulhu":
                        boss.downed = NPC.downedBoss1;
                        break;
                    case "BrainWorm":
                        boss.downed = NPC.downedBoss2;
                        break;
                    case "QueenBee":
                        boss.downed = NPC.downedQueenBee;
                        break;
                    case "Skeletron":
                        boss.downed = NPC.downedBoss3;
                        break;
                    case "Deerclops":
                        boss.downed = NPC.downedDeerclops;
                        break;
                    case "WallOfFlesh":
                        boss.downed = Main.hardMode;
                        break;
                    case "QueenSlime":
                        boss.downed = NPC.downedQueenSlime;
                        break;
                    case "Twins":
                        boss.downed = NPC.downedMechBoss2;
                        break;
                    case "Destroyer":
                        boss.downed = NPC.downedMechBoss1;
                        break;
                    case "SkeletronPrime":
                        boss.downed = NPC.downedMechBoss3;
                        break;
                    case "Plantera":
                        boss.downed = NPC.downedPlantBoss;
                        break;
                    case "Golem":
                        boss.downed = NPC.downedGolemBoss;
                        break;
                    case "DukeFishron":
                        boss.downed = NPC.downedFishron;
                        break;
                    case "EmpressOfLight":
                        boss.downed = NPC.downedEmpressOfLight;
                        break;
                    // 16: betsy is not tracked
                    case "Cultist":
                        boss.downed = NPC.downedAncientCultist;
                        break;
                    case "Moonlord":
                        boss.downed = NPC.downedMoonlord;
                        break;
                }
            }
        }

        private void UpdateChangedBosses()
        {
            foreach (var boss in bosses)
            {
                switch (boss.bossName)
                {
                    case "WhiteTail":
                        boss.downed = DownedBossSystem.DownedWhiteTail;
                        break;
                    case "WolfKing":
                        boss.downed = DownedBossSystem.DownedWolfKing;
                        break;
                    case "Behemoth":
                        boss.downed = DownedBossSystem.DownedBehemoth;
                        break;
                    case "Shark":
                        boss.downed = DownedBossSystem.DownedShark;
                        break;
                }
            }
        }
    }

    // This works even without boss checklist installed
    public class BossProgressionSystem : ModSystem
    {
        private static float bossProgressionNumber = 0;
        private static int checkInterval = 10;
        private int time = 0;
        private bool hasChecked = false;

        public static List<DownedBossObjectList> bossProgressionLists = new List<DownedBossObjectList>();

        public static void PrintDefeatedBosses()
        {
            UpdateBossProgressionNumber();
            Main.NewText("Bosses defeated:");
            foreach (var list in bossProgressionLists)
            {
                var txt = list.modName + ": ";
                foreach (var boss in list.bosses)
                {
                    if (boss.downed)
                        txt += boss.bossName + ", ";
                }
                Main.NewText(txt);
            }

            foreach (var mod in ModSupportSystem.externalMods)
            {
                if (mod.bossProgressionList == null)
                    continue;

                var txt = mod.modName + ": ";
                foreach (var boss in mod.bossProgressionList.bosses)
                {
                    if (boss.downed)
                        txt += boss.bossName + ", ";
                }
                Main.NewText(txt);
            }
        }

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

        public override void PostWorldLoad()
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

        public static void Init()
        {
            if (bossProgressionLists == null || bossProgressionLists.Count == 0)
            {
                bossProgressionLists = new List<DownedBossObjectList>()
                {
                    new DownedBossObjectList()
                    {
                        modName = "Terraria",
                        bosses = new List<DownedBossObject>
                        {
                            new DownedBossObject("KingSlime", 1),
                            new DownedBossObject("EyeOfCthulhu", 2),
                            new DownedBossObject("BrainWorm", 3),
                            new DownedBossObject("QueenBee", 4),
                            new DownedBossObject("Skeletron", 5),
                            new DownedBossObject("Deerclops", 6),
                            new DownedBossObject("WallOfFlesh", 7),
                            new DownedBossObject("QueenSlime", 8),
                            new DownedBossObject("Twins", 9),
                            new DownedBossObject("Destroyer", 10),
                            new DownedBossObject("SkeletronPrime", 11),
                            new DownedBossObject("Plantera", 12),
                            new DownedBossObject("Golem", 13),
                            new DownedBossObject("DukeFishron", 14),
                            new DownedBossObject("EmpressOfLight", 15),
                            // 16 is for betsy, but it is not tracked
                            new DownedBossObject("Cultist", 17),
                            new DownedBossObject("Moonlord", 18)
                        }
                    },

                    new DownedBossObjectList()
                    {
                        modName = "Changed",
                        bosses = new List<DownedBossObject>()
                        {
                            new DownedBossObject("WhiteTail", 1.5f),
                            new DownedBossObject("WolfKing", 2.5f),
                            new DownedBossObject("Behemoth", 5.5f)
                        }
                    }
                };
            }
        }

        public static float GetBossProgressionNumber()
        {
            return bossProgressionNumber;
        }

        private static void UpdateBossProgressionNumber()
        {
            if (bossProgressionLists == null || bossProgressionLists.Count == 0)
                Init();

            var maxProgressionNumber = 0f;

            // Old. Only used for Terraria and Changed bosses
            foreach (var progressionList in bossProgressionLists)
            {
                if (progressionList.dontCheck)
                    continue;
                progressionList.UpdateBosses();

                if (progressionList.bosses != null)
                {
                    var downedBosses = progressionList.bosses.Where(x => x.downed).ToList();
                    if (downedBosses.Any())
                        maxProgressionNumber = Math.Max(maxProgressionNumber, downedBosses.ToList().Select(x => x.progressionNumber).ToList().Max());
                }
            }

            // New. For external mods
            foreach (var mod in ModSupportSystem.externalMods)
            {
                var bossProgressionList = mod.bossProgressionList;
                if (bossProgressionList != null)
                {
                    if (bossProgressionList.dontCheck)
                        continue;
                    bossProgressionList.UpdateBosses();

                    if (bossProgressionList.bosses != null)
                    {
                        var downedBosses = bossProgressionList.bosses.Where(x => x.downed).ToList();
                        if (downedBosses.Any())
                        {
                            var numb2 = Math.Max(maxProgressionNumber, downedBosses.ToList().Select(x => x.progressionNumber).ToList().Max());
                            maxProgressionNumber = Math.Max(numb2, maxProgressionNumber);
                        }
                    }
                }
            }

            bossProgressionNumber = maxProgressionNumber;
        }
    }
}
