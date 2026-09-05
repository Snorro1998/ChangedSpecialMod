using ChangedSpecialMod.Utilities;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{
    public enum BiomeType2 : byte
    {
        None,
        Latex,
        Corrupt,
        Crimson,
        Hallow
    }

    public static class WorldBlockAmounts
    {
        public static int nTotalBlocks;
        public static int nHallow;
        public static int nCorrupt;
        public static int nCrimson;
        public static int nLatex;

        public static bool WorldIsPure()
        {
            return nHallow == 0 && nCorrupt == 0 && nCrimson == 0 && nLatex == 0;
        }

        public static void Recalculate()
        {
            nTotalBlocks = 0;
            nHallow = 0;
            nCorrupt = 0;
            nCrimson = 0;
            nLatex = 0;

            BiomeType2[] TileLookup = new BiomeType2[TileLoader.TileCount];

            HashSet<int> LatexCountCollection =
            [

            ];

            var blocks = BiomeConversionSystem.GetLatexBlocks();

            foreach (var block in blocks)
            {
                LatexCountCollection.Add(block);
            }

            HashSet<ushort> CorruptCountCollection =
            [
                TileID.CorruptGrass,
                TileID.CorruptPlants,
                TileID.Ebonstone,
                TileID.CorruptThorns,
                TileID.Ebonsand,
                TileID.CorruptIce,
                TileID.CorruptHardenedSand,
                TileID.CorruptSandstone,
                TileID.CorruptVines,
                TileID.CorruptJungleGrass
            ];

            HashSet<ushort> CrimsonCountCollection =
            [
                TileID.CrimsonGrass,
                TileID.FleshIce,
                TileID.CrimsonPlants,
                TileID.Crimstone,
                TileID.CrimsonVines,
                TileID.Crimsand,
                TileID.CrimsonThorns,
                TileID.CrimsonHardenedSand,
                TileID.CrimsonSandstone,
                TileID.CrimsonJungleGrass
            ];

            HashSet<ushort> HallowCountCollection =
            [
                TileID.HallowedGrass,
                TileID.HallowedPlants,
                TileID.HallowedPlants2,
                TileID.HallowedVines,
                TileID.Pearlsand,
                TileID.Pearlstone,
                TileID.HallowedIce,
                TileID.HallowHardenedSand,
                TileID.HallowSandstone
            ];

            foreach (ushort id in CorruptCountCollection)
                TileLookup[id] = BiomeType2.Corrupt;

            foreach (ushort id in CrimsonCountCollection)
                TileLookup[id] = BiomeType2.Crimson;

            foreach (ushort id in HallowCountCollection)
                TileLookup[id] = BiomeType2.Hallow;

            foreach (int id in LatexCountCollection)
                TileLookup[id] = BiomeType2.Latex;

            for (var x = 0; x < Main.maxTilesX; x++)
            {
                for (var y = 0; y < Main.maxTilesY; y++)
                {
                    var tile = Main.tile[x, y];
                    if (!tile.HasTile)
                        continue;
                    nTotalBlocks++;

                    switch (TileLookup[tile.TileType])
                    {
                        case BiomeType2.Latex:
                            nLatex++;
                            break;
                        case BiomeType2.Corrupt:
                            nCorrupt++;
                            break;
                        case BiomeType2.Crimson:
                            nCrimson++;
                            break;
                        case BiomeType2.Hallow:
                            nHallow++;
                            break;
                    }
                }
            }

            int tGood = (byte)Math.Round((double)nHallow / (double)WorldGen.totalSolid * 100.0);
            if (tGood == 0 && nHallow > 0)
                tGood = 1;
            int tEvil = (byte)Math.Round((double)nCorrupt / (double)WorldGen.totalSolid * 100.0);
            if (tEvil == 0 && nCorrupt > 0)
                tEvil = 1;
            int tBlood = (byte)Math.Round((double)nCrimson / (double)WorldGen.totalSolid * 100.0);
            if (tBlood == 0 && nCrimson > 0)
                tBlood = 1;
            int tLatex = (byte)Math.Round((double)nLatex / (double)WorldGen.totalSolid * 100.0);
            if (tLatex == 0 && nLatex > 0)
                tLatex = 1;
        }
    }

    public class TimeSystem : ModSystem
    {
        private bool lastUpdateWasDay = false;
        private bool currentUpdateWasDay = true;

        public override void PostUpdateWorld()
        {
            currentUpdateWasDay = Main.IsItDay();

            if (currentUpdateWasDay != lastUpdateWasDay)
            {
                // Day started
                if (currentUpdateWasDay)
                {
                    WorldBlockAmounts.Recalculate();
                }
                // Night started
                else
                {
                    WorldBlockAmounts.Recalculate();
                }
                lastUpdateWasDay = currentUpdateWasDay;
            }
        }

        public override void PostWorldLoad()
        {
            lastUpdateWasDay = false;
            currentUpdateWasDay = true;
        }
    }
}
