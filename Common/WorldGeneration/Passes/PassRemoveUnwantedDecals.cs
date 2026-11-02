using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Latex.Black;
using ChangedSpecialMod.Content.Tiles.Latex.White;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.WorldGeneration.Passes
{
    public class PassRemoveUnwantedDecals : WorldGenerationPass
    {
        public PassRemoveUnwantedDecals()
        {
            PassName = "ChangedRemoveUnwantedDecals";
            GenerationMessage = "RemoveUnwantedDecals";
        }

        public override void Generate()
        {
            var tileIDs = new HashSet<int>()
            {
                TileID.Plants,
                TileID.Plants2,

                TileID.Pumpkins,

                TileID.JunglePlants,
                TileID.JunglePlants2,

                TileID.CorruptPlants,
                TileID.CrimsonPlants,

                // Desert plants
                TileID.OasisPlants,
                TileID.SeaOats,
                TileID.PalmTree,

                TileID.SmallPiles,
                TileID.LargePiles,
                TileID.LargePiles2,
            };

            var tileIDSand = new HashSet<int>()
            {

            };

            var latexTileIDs = BiomeConversionSystem.GetLatexBlocks();

            foreach (var latexTileID in latexTileIDs)
            {
                tileIDSand.Add(latexTileID);
            }

            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.worldSurface; y++)
                {
                    var tile = Framing.GetTileSafely(x, y);
                    var tileBelow = Framing.GetTileSafely(x, y + 1);
                    var tileAbove = Framing.GetTileSafely(x, y - 1);

                    if (tile.HasTile && tileIDs.Contains(tile.TileType) &&
                    tileBelow.HasTile && tileIDSand.Contains(tileBelow.TileType))
                    {
                        WorldGen.KillTile(x, y, false, false, true);
                    }

                    // Spread grass on dry dirt
                    if (tile.HasTile && tile.TileType == ModContent.TileType<DryDirt>() && !tileAbove.HasTile)
                        tile.TileType = (ushort)ModContent.TileType<DryDirtGrassTile>();
                }
            }
        }
    }
}
