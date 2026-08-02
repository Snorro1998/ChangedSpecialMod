using ChangedSpecialMod.Content.Biomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ChangedSpecialMod.Utilities
{
    public static class BiomeChecks
    {
        // Black
        #region black
        public static bool InBlackLatexSurfaceBiome(Player player)
        {
            return player.InModBiome<BlackLatexSurfaceBiome>() ||
                player.InModBiome<BlackLatexSurfaceDesertBiome>() ||
                player.InModBiome<BlackLatexSurfaceJungleBiome>() ||
                player.InModBiome<BlackLatexSurfaceSnowBiome>();
        }

        public static bool InBlackLatexUndergroundBiome(Player player) => player.InModBiome<BlackLatexUndergroundBiome>();

        public static bool InBlackLatexBiome(Player player)
        {
            return InBlackLatexSurfaceBiome(player) || InBlackLatexUndergroundBiome(player);
        }
        #endregion
        // White
        #region white
        public static bool InWhiteLatexSurfaceBiome(Player player)
        {
            return player.InModBiome<WhiteLatexSurfaceBiome>() ||
                player.InModBiome<WhiteLatexSurfaceDesertBiome>() ||
                player.InModBiome<WhiteLatexSurfaceJungleBiome>() ||
                player.InModBiome<WhiteLatexSurfaceSnowBiome>();
        }

        public static bool InWhiteLatexUndergroundBiome(Player player) => player.InModBiome<WhiteLatexUndergroundBiome>();

        public static bool InWhiteLatexBiome(Player player)
        {
            return InWhiteLatexSurfaceBiome(player) || InWhiteLatexUndergroundBiome(player);
        }
        #endregion
        // City Ruins
        public static bool InCityRuinsBiome(Player player) => player.InModBiome<CityRuinsSurfaceBiome>();

        // Any biome
        public static bool InChangedSurfaceBiome(Player player)
        {
            return InCityRuinsBiome(player) || InBlackLatexSurfaceBiome(player) || InWhiteLatexSurfaceBiome(player);
        }

        public static bool InChangedUndergroundBiome(Player player)
        {
            return InBlackLatexUndergroundBiome(player) || InWhiteLatexUndergroundBiome(player);
        }

        public static bool InChangedBiome(Player player)
        {
            return InCityRuinsBiome(player) || InBlackLatexBiome(player) || InWhiteLatexBiome(player);
        }
    }
}
