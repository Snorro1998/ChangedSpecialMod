using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ChangedSpecialMod
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public partial class ChangedSpecialMod : Mod
	{
        internal static ChangedSpecialMod Instance => _Instance ??= ModContent.GetInstance<ChangedSpecialMod>();
        private static ChangedSpecialMod _Instance;

        public override void Unload()
        {
            _Instance = null;
            base.Unload();
        }

        public override object Call(params object[] args)
        {
            if (args ==  null || args.Length == 0)
                return null;

            int argsLength = args.Length;

            try
            {
                if (args[0] is not string)
                    return new ArgumentNullException("ERROR: First argument must be a string.");

                string methodName = args[0] as string;
                methodName = methodName.ToLower();

                switch (methodName)
                {
                    default: break;
                    case "getbossdowned":
                    case "bossdowned":
                        if (argsLength < 2)
                            return new ArgumentNullException("ERROR: Must specify a boss or event name as a string.");
                        if (args[1] is not string)
                            return new ArgumentException("ERROR: The argument to \"Downed\" must be a string.");
                        return GetBossDowned(args[1].ToString());
                    case "orang":
                    case "orange":
                    case "oranges":
                        ChangedUtils.SpawnOrangesOnAllPlayers();
                        return true;
                    case "spawnall":
                    case "spawnallnpcs":
                        ChangedUtils.SpawnAllNPCs(null);
                        return true;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Call Error: {e.StackTrace} {e.Message}");
            }

            return new ArgumentException("ERROR: Invalid method name.");
        }

        private bool GetBossDowned(string bossName)
        {
            bossName = bossName.ToLower();

            switch(bossName)
            {
                case "whitetail":
                    return DownedBossSystem.DownedWhiteTail;
                case "wolfking":
                    return DownedBossSystem.DownedWolfKing;
                case "behemoth":
                    return DownedBossSystem.DownedBehemoth;
            }

            return false;
        }

        public string BTitlesHook_BiomeChecker(Player player)
        {
            if (player.InModBiome<BlackLatexSurfaceBiome>()) 
                return "blacklatexsurfacebiome";
            if (player.InModBiome<WhiteLatexSurfaceBiome>())
                return "whitelatexsurfacebiome";
            if (player.InModBiome<CityRuinsSurfaceBiome>())
                return "cityruinssurfacebiome";

            if (player.InModBiome<BlackLatexUndergroundBiome>())
                return "blacklatexundergroundbiome";
            if (player.InModBiome<WhiteLatexUndergroundBiome>())
                return "whitelatexundergroundbiome";

            return "";
        }

        public string BTitlesHook_MiniBiomeChecker(Player player)
        {
            //if (player.IsInBiome2()) return "biome2";
            //if (player.IsInBiome3()) return "biome3";

            return "";
        }

        public IEnumerable<dynamic> BTitlesHook_GetBiomes()
        {
            yield return new
            {
                Key = "blacklatexsurfacebiome",
                Title = "Black Latex Area",
                SubTitle = "Changed",
                TitleColor = new Color(50, 50, 50),
                TitleStroke = Color.Black,
            };
            yield return new
            {
                Key = "whitelatexsurfacebiome",
                Title = "White Latex Area",
                SubTitle = "Changed",
                TitleColor = Color.White,
                TitleStroke = Color.Black,
            };
            yield return new
            {
                Key = "cityruinssurfacebiome",
                Title = "City Ruins",
                SubTitle = "Changed",
                TitleColor = Color.Orange,
                TitleStroke = Color.Black,
            };
            yield return new
            {
                Key = "blacklatexundergroundbiome",
                Title = "Black Latex Cave",
                SubTitle = "Changed",
                TitleColor = new Color(50, 50, 50),
                TitleStroke = Color.Black,
            };
            yield return new
            {
                Key = "whitelatexundergroundbiome",
                Title = "White Latex Cave",
                SubTitle = "Changed",
                TitleColor = Color.White,
                TitleStroke = Color.Black,
            };
        }
    }
}
