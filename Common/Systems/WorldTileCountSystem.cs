using ChangedSpecialMod.Content.Tiles.Latex.Black;
using ChangedSpecialMod.Content.Tiles.Latex.White;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{
    public class WorldTileCountSystem : ModSystem
    {
        private bool lastUpdateWasDay;

        public static byte tLatex = 0;
        public static int totalLatex = 0;
        public static int totalLatex2 = 0;

        // This gets filled with all latex blocks from the biome conversion system
        public static List<int> LatexCountCollection = new List<int>
        {

        };

        public static bool WorldIsPure()
        {
            return WorldGen.tEvil == 0 && WorldGen.tBlood == 0 && WorldGen.tGood == 0 && WorldTileCountSystem.tLatex == 0;
        }

        // Copy from vanilla with latex count added
        public static void CountTiles(int X)
        {
            if (X == 0)
            {
                WorldGen.totalEvil = WorldGen.totalEvil2;
                WorldGen.totalBlood = WorldGen.totalBlood2;
                WorldGen.totalSolid = WorldGen.totalSolid2;
                WorldGen.totalGood = WorldGen.totalGood2;
                totalLatex = totalLatex2;
                WorldGen.tGood = (byte)Math.Round((double)WorldGen.totalGood / (double)WorldGen.totalSolid * 100.0);
                WorldGen.tEvil = (byte)Math.Round((double)WorldGen.totalEvil / (double)WorldGen.totalSolid * 100.0);
                WorldGen.tBlood = (byte)Math.Round((double)WorldGen.totalBlood / (double)WorldGen.totalSolid * 100.0);
                tLatex = (byte)Math.Round((double)totalLatex / (double)WorldGen.totalSolid * 100.0);
                if (WorldGen.tGood == 0 && WorldGen.totalGood > 0)
                {
                    WorldGen.tGood = 1;
                }
                if (WorldGen.tEvil == 0 && WorldGen.totalEvil > 0)
                {
                    WorldGen.tEvil = 1;
                }
                if (WorldGen.tBlood == 0 && WorldGen.totalBlood > 0)
                {
                    WorldGen.tBlood = 1;
                }
                if (tLatex == 0 && totalLatex > 0)
                {
                    tLatex = 1;
                }
                // Should make a copy of this with latex count
                //if (Main.netMode == 2)
                //{
                //    NetMessage.SendData(57);
                //}
                WorldGen.totalEvil2 = 0;
                WorldGen.totalSolid2 = 0;
                WorldGen.totalGood2 = 0;
                WorldGen.totalBlood2 = 0;
                totalLatex2 = 0;
            }
            ushort num = 0;
            ushort num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            do
            {
                int num6;
                int num7;
                if (num4 == 0)
                {
                    num6 = 0;
                    num5 = (int)(Main.worldSurface + 1.0);
                    num7 = 5;
                }
                else
                {
                    num6 = num5;
                    num5 = Main.maxTilesY;
                    num7 = 1;
                }
                for (int i = num6; i < num5; i++)
                {
                    Tile tile = Main.tile[X, i];
                    if (tile == null)
                    {
                        Tile tile2 = Framing.GetTileSafely(X, i);// (Main.tile[X, i] = default(Tile));
                        tile = tile2;
                    }
                    num = tile.TileType;
                    if (num != 0 || tile.HasTile)
                    {
                        if (num == num2)
                        {
                            num3 += num7;
                            continue;
                        }
                        WorldGen.tileCounts[num2] += num3;
                        num2 = num;
                        num3 = num7;
                    }
                }
                WorldGen.tileCounts[num2] += num3;
                num3 = 0;
                num4++;
            }
            while (num4 < 2);
            AddUpAlignmentCounts();
        }

        // Copy from vanilla with latex count added
        public static void AddUpAlignmentCounts(bool clearCounts = false)
        {
            if (LatexCountCollection.Count == 0)
            {
                LatexCountCollection = BiomeConversionSystem.GetLatexBlocks();
            }

            if (clearCounts)
            {
                WorldGen.totalEvil2 = 0;
                WorldGen.totalSolid2 = 0;
                WorldGen.totalGood2 = 0;
                WorldGen.totalBlood2 = 0;
                totalLatex2 = 0;
            }
            for (int i = 0; i < TileID.Sets.HallowCountCollection.Count; i++)
            {
                WorldGen.totalGood2 += WorldGen.tileCounts[TileID.Sets.HallowCountCollection[i]];
            }
            for (int j = 0; j < TileID.Sets.CorruptCountCollection.Count; j++)
            {
                WorldGen.totalEvil2 += WorldGen.tileCounts[TileID.Sets.CorruptCountCollection[j]];
            }
            for (int k = 0; k < TileID.Sets.CrimsonCountCollection.Count; k++)
            {
                WorldGen.totalBlood2 += WorldGen.tileCounts[TileID.Sets.CrimsonCountCollection[k]];
            }
            for (int k = 0; k < LatexCountCollection.Count; k++)
            {
                totalLatex2 += WorldGen.tileCounts[LatexCountCollection[k]];
            }

            // Grass
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.Grass];
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.CorruptGrass];
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.HallowedGrass];
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.CrimsonGrass];
            WorldGen.totalSolid2 += WorldGen.tileCounts[ModContent.TileType<BlackLatexGrassTile>()];
            WorldGen.totalSolid2 += WorldGen.tileCounts[ModContent.TileType<WhiteLatexGrassTile>()];

            // Stone
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.Stone];
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.Ebonstone];
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.Pearlstone];
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.Crimstone];
            WorldGen.totalSolid2 += WorldGen.tileCounts[ModContent.TileType<BlackLatexStoneTile>()];
            WorldGen.totalSolid2 += WorldGen.tileCounts[ModContent.TileType<WhiteLatexStoneTile>()];

            // Sand
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.Sand];
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.Ebonsand];
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.Pearlsand];
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.Crimsand];
            WorldGen.totalSolid2 += WorldGen.tileCounts[ModContent.TileType<BlackLatexSandTile>()];
            WorldGen.totalSolid2 += WorldGen.tileCounts[ModContent.TileType<WhiteLatexSandTile>()];

            // Ice
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.IceBlock];
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.CorruptIce];
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.HallowedIce];
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.FleshIce];
            WorldGen.totalSolid2 += WorldGen.tileCounts[ModContent.TileType<BlackLatexIceTile>()];
            WorldGen.totalSolid2 += WorldGen.tileCounts[ModContent.TileType<WhiteLatexIceTile>()];

            // Jungle grass
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.JungleGrass];
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.CorruptJungleGrass];
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.CrimsonJungleGrass];
            WorldGen.totalSolid2 += WorldGen.tileCounts[ModContent.TileType<BlackLatexJungleGrassTile>()];
            WorldGen.totalSolid2 += WorldGen.tileCounts[ModContent.TileType<WhiteLatexJungleGrassTile>()];

            // Golf grass
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.GolfGrass];
            WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.GolfGrassHallowed];

            // Dirt
            WorldGen.totalSolid2 += WorldGen.tileCounts[ModContent.TileType<BlackLatexTile>()];
            WorldGen.totalSolid2 += WorldGen.tileCounts[ModContent.TileType<WhiteLatexTile>()];

            // Mud
            WorldGen.totalSolid2 += WorldGen.tileCounts[ModContent.TileType<BlackLatexMudTile>()];
            WorldGen.totalSolid2 += WorldGen.tileCounts[ModContent.TileType<WhiteLatexMudTile>()];

            // Snow
            WorldGen.totalSolid2 += WorldGen.tileCounts[ModContent.TileType<BlackLatexSnowTile>()];
            WorldGen.totalSolid2 += WorldGen.tileCounts[ModContent.TileType<WhiteLatexSnowTile>()];

            // Should we add latex to totalsolid? This will affect all percentages
            WorldGen.totalSolid2 += WorldGen.tileCounts[ModContent.TileType<BlackLatexTile>()] + WorldGen.tileCounts[ModContent.TileType<BlackLatexStoneTile>()];
            WorldGen.totalSolid2 += WorldGen.tileCounts[ModContent.TileType<WhiteLatexTile>()] + WorldGen.tileCounts[ModContent.TileType<WhiteLatexTile>()];
            Array.Clear(WorldGen.tileCounts, 0, WorldGen.tileCounts.Length);
        }

        public static void RecalculateTileCounts()
        {
            WorldGen.totalEvil2 = 0;
            WorldGen.totalSolid2 = 0;
            WorldGen.totalGood2 = 0;
            WorldGen.totalBlood2 = 0;

            for (int i = 1; i < Main.maxTilesX; i++)
                CountTiles(i);
            CountTiles(0);
        }

        // Count the tiles in the world twice per day
        public override void PostUpdateWorld() 
        { 
            bool isDay = Main.IsItDay(); 
            if (isDay != lastUpdateWasDay) 
            {
                RecalculateTileCounts(); 
                lastUpdateWasDay = isDay; 
            } 
        } 

        // Recalculate after joining the world
        public override void PostWorldLoad() 
        {
            RecalculateTileCounts(); 
            lastUpdateWasDay = Main.IsItDay(); 
        } 
    }
}
