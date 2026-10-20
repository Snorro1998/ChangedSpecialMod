using ChangedSpecialMod.Common.Systems.ModSupport;
using ReLogic.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{
    public class ExternalModData
    {
        public Mod mod;
        // Tiles
        public Dictionary<string, int> tileTypes = new Dictionary<string, int>();
        public List<int> tilesInfectedSnow = new List<int>();
        public List<int> tilesAvoidedByOrangeShrines = new List<int>();

        public Dictionary<string, int> townNPCIds = new Dictionary<string, int>();
        
        public bool isActive => mod != null;

        public ExternalModData(Mod mod) 
        {
            this.mod = mod;
        }

        public void AddTileType(string name, bool avoidedByWorldGen = false)
        {
            if (tileTypes == null)
                tileTypes = new Dictionary<string, int>();

            if (mod == null || tileTypes.ContainsKey(name))
                return;

            if (mod.TryFind(name, out ModTile modTile))
            {
                var itemType = modTile.Type;
                tileTypes.Add(name, itemType);
                if (avoidedByWorldGen)
                    tilesAvoidedByOrangeShrines.Add(itemType);
            }
        }

        public void AddTownNPC(string internalName, string localizationName)
        {
            if (townNPCIds == null)
                townNPCIds = new Dictionary<string, int>();

            if (mod == null || townNPCIds.ContainsKey(localizationName))
                return;

            if (mod.TryFind(internalName, out ModNPC modNPC))
            {
                var npcType = modNPC.Type;
                townNPCIds.Add(localizationName, npcType);
            }
        }

        public bool ShouldAvoidTileType(int tileType)
        {
            return tilesAvoidedByOrangeShrines.Contains(tileType);
        }
    }

    public class ModSupportSystem : ModSystem
    {
        public static ChangedSpecialMod changedMod = null;
        
        // Race
        public static Mod modMrPlagueRaces = null;

        // Other
        public static Mod modBoulderBackport = null;
        // Don't need to do anything for Biome Titles

        // The min and max indecis for the extra title message added in the hjson
        private static int indexTitleMessageMin = 1;
        private static int indexTitleMessageMax = 5;

        public static List<ModSupportData> externalMods;

        public override void Load()
        {
            changedMod = ChangedSpecialMod.Instance;
            externalMods = new List<ModSupportData>();

            // Informational
            AddExternalModSupport("BossChecklist", (mod, name) => new ModSupportBossChecklist(mod, name));
            AddExternalModSupport("MusicDisplay", (mod, name) => new ModSupportMusicDisplay(mod, name));
            AddExternalModSupport("Census", (mod, name) => new ModSupportCensus(mod, name));
            AddExternalModSupport("Wikithis", (mod, name) => new ModSupportWikiThis(mod, name));

            // Shops
            AddExternalModSupport("Fargowiltas", (mod, name) => new ModSupportFargosMutant(mod, name));

            // Content mods
            AddExternalModSupport("CalamityMod", (mod, name) => new ModSupportCalamity(mod, name));
            AddExternalModSupport("Coralite", (mod, name) => new ModSupportCoralite(mod, name));
            AddExternalModSupport("Redemption", (mod, name) => new ModSupportRedemption(mod, name));
            AddExternalModSupport("SpiritMod", (mod, name) => new ModSupportSpirit(mod, name));
            AddExternalModSupport("SpiritReforged", (mod, name) => new ModSupportSpiritReforged(mod, name));
            AddExternalModSupport("StarsModPack", (mod, name) => new ModSupportStarsModPack(mod, name));
            AddExternalModSupport("ThoriumMod", (mod, name) => new ModSupportThorium(mod, name));

            // Race
            modMrPlagueRaces = GetMod("MrPlagueRaces");

            // Other
            modBoulderBackport = GetMod("BoulderBackport");
            AddExternalModSupport("terraguardians", (mod, name) => new ModSupportTerraGuardian(mod, name));

            // Changed
            AddExternalModSupport("PuroCompanion", (mod, name) => new ModSupportPuroSummon(mod, name));

            //GameSupportSystem.LoadGames();

            foreach (var mod in externalMods)
            {
                try
                {
                    mod.Init();
                }
                catch (Exception ex)
                {
                    var modName = mod.modName == null ? "Unknown mod" : mod.modName;
                    Mod.Logger.Error($"{modName} Init Error: " + ex.Message);
                }
            }
        }

        private void AddExternalModSupport<T>(string modName, Func<Mod, string, T> createSupport) where T : ModSupportData
        {
            var mod = GetMod(modName);

            if (mod != null)
                externalMods.Add(createSupport(mod, modName));
        }

        private static Mod GetMod(string name)
        {
            ModLoader.TryGetMod(name, out Mod mod);
                return mod;
        }

        public static ModSupportData GetModSupportData(string name)
        {
            if (externalMods == null || externalMods.Count == 0)
                return null;

            return externalMods.FirstOrDefault(x => x.modName == name);
        }

        public static bool IsModActive(string name)
        {
            return ModLoader.TryGetMod(name, out Mod result);
        }

        public override void Unload()
        {
            changedMod = null;

            // Race
            modMrPlagueRaces = null;

            // Other
            modBoulderBackport = null;

            if (externalMods != null)
                externalMods.Clear();

            if (Platform.Current.Type == PlatformType.Windows)
                RemoveExtraTitles();
        }

        public override void PostSetupContent()
        {
            foreach (var mod in externalMods)
            {
                try
                {
                    mod.PostSetupContent(Mod.Logger);
                }
                catch(Exception ex)
                {
                    var modName = mod.modName == null ? "Unknown mod" : mod.modName;
                    Mod.Logger.Error($"{modName} PostSetupContent Error: " + ex.Message);
                }
            }

            // This might fix a startup crash on MacOS, but I can't confirm this
            if (Platform.Current.Type == PlatformType.Windows)
            {
                SetupExtraTitles();
                TryUpdateTitle();
            }
        }

        public static List<int> GetAvoidTiles()
        {
            var list = new List<int>();

            if (externalMods != null)
            {
                foreach (var mod in externalMods)
                    list.AddRange(mod.tilesAvoidedByWorldGen);
            }

            return list;
        }

        private void TryUpdateTitle()
        {
            MethodInfo setTitleMethod = typeof(Main).GetMethod("SetTitle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (setTitleMethod != null)
                setTitleMethod.Invoke(Main.instance, null);
        }

        private void SetupExtraTitles()
        {
            var gameTitleKeys = LanguageManager.Instance.GetKeysInCategory("GameTitle");
            for (int i = indexTitleMessageMin; i <= indexTitleMessageMax; i++)
            {
                if (!gameTitleKeys.Contains($"Changed{i.ToString()}"))
                    gameTitleKeys.Add($"Changed{i.ToString()}");
            }
        }

        private void RemoveExtraTitles()
        {
            var gameTitleKeys = LanguageManager.Instance.GetKeysInCategory("GameTitle");
            for (int i = indexTitleMessageMin; i <= indexTitleMessageMax; i++)
            {
                if (gameTitleKeys.Contains($"Changed{i.ToString()}"))
                    gameTitleKeys.Remove($"Changed{i.ToString()}");
            }
        }
    }
}
