using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using System;
using Terraria;

namespace ChangedSpecialMod.Common.WorldGeneration.Passes
{
    public class PassAddLatexEverywhere : WorldGenerationPass
    {
        private int bottomFalloff = 20;
        private GooType gooType;

        public PassAddLatexEverywhere(GooType gooType)
        {
            this.gooType = gooType;
            PassName = "ChangedSeedLatexEverywhere";
            GenerationMessage = "SpecialLatexEverywhere";
        }

        public override void Generate()
        {
            for (int y = 0; y < Main.worldSurface + bottomFalloff; y++)
            {
                for (int x = 0; x < Main.maxTilesX + bottomFalloff; x++)
                {
                    var tile = Main.tile[x, y];
                    if (tile != null && tile.HasTile)
                    {
                        if (y >= Main.worldSurface && !WorldGen.genRand.NextBool((int)Math.Max(1, y - Main.worldSurface)))
                            continue;

                        var tileType = WorldGenerator.GetTileType(tile, gooType);
                        if (tileType != -1)
                            tile.TileType = (ushort)tileType;
                    }
                }
            }
        }
    }
}
