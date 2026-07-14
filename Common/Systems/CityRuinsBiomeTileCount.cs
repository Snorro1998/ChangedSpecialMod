using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Latex;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{
	public class CityRuinsBiomeTileCount : ModSystem
	{
		public static int NBlocksNeeded = 800;

        // Underground
        public static int NBlocksNeededUnderground = 300;
        public static int RadiusFromPlayer = 20;

		public static int DryDirtBlockCount;
		public static int WhiteLatexBlockCount;
		public static int BlackLatexBlockCount;

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) 
		{
            var blackTiles = new List<int>()
            {
                ModContent.TileType<BlackLatexTile>(),
                ModContent.TileType<BlackLatexSandTile>(),
                ModContent.TileType<BlackLatexStoneTile>(),
                ModContent.TileType<BlackLatexIceTile>(),
                ModContent.TileType<BlackLatexSnowTile>()
            };

            var whiteTiles = new List<int>()
            {
                ModContent.TileType<WhiteLatexTile>(),
                ModContent.TileType<WhiteLatexSandTile>(),
                ModContent.TileType<WhiteLatexStoneTile>(),
                ModContent.TileType<WhiteLatexIceTile>(),
                ModContent.TileType<WhiteLatexSnowTile>()
            };

			DryDirtBlockCount = tileCounts[ModContent.TileType<DryDirt>()];
            BlackLatexBlockCount = 0;// tileCounts[ModContent.TileType<BlackLatexTile>()] + tileCounts[ModContent.TileType<BlackLatexSandTile>()] + tileCounts[ModContent.TileType<BlackLatexStoneTile>()];
            WhiteLatexBlockCount = 0;// tileCounts[ModContent.TileType<WhiteLatexTile>()] + tileCounts[ModContent.TileType<WhiteLatexSandTile>()] + tileCounts[ModContent.TileType<WhiteLatexStoneTile>()];

            foreach (var tile in blackTiles)
                BlackLatexBlockCount += tileCounts[tile];

            foreach (var tile in whiteTiles)
               WhiteLatexBlockCount += tileCounts[tile];
        }

        public static bool BlockNearby(Player player, List<int> tileIDs, int radius)
        {
            if (tileIDs == null || tileIDs.Count == 0)
                return false;

            Vector2 center = player.Center;

            int minX = (int)(center.X / 16f) - radius;
            int maxX = (int)(center.X / 16f) + radius;
            int minY = (int)(center.Y / 16f) - radius;
            int maxY = (int)(center.Y / 16f) + radius;

            int radiusSq = radius * radius;

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    // bounds safety
                    if (x < 0 || y < 0 || x >= Main.maxTilesX || y >= Main.maxTilesY)
                        continue;

                    // distance check (circle, not square)
                    Vector2 tileWorldPos = new Vector2(x * 16, y * 16);
                    if (Vector2.DistanceSquared(tileWorldPos, center) > radiusSq * 256)
                        continue;

                    Tile tile = Framing.GetTileSafely(x, y);

                    if (tile.HasTile && tileIDs.Contains(tile.TileType))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool BiomeActive(Player player, GooType gooType, bool surface = true)
		{
            var targetBlockIDs = new List<int>();
			var targetBlockCount = -1;
			var otherBlockCount1 = -1;
			var otherBlockCount2 = -1;

			switch(gooType)
			{
				case GooType.Black:
                    targetBlockIDs = new List<int>
                    {
                        ModContent.TileType<BlackLatexTile>(),
                        ModContent.TileType<BlackLatexSandTile>(),
                        ModContent.TileType<BlackLatexStoneTile>(),
                        ModContent.TileType<BlackLatexIceTile>(),
                        ModContent.TileType<BlackLatexSnowTile>()
                    };
                    targetBlockCount = BlackLatexBlockCount;
					otherBlockCount1 = WhiteLatexBlockCount;
					otherBlockCount2 = DryDirtBlockCount;
					break;
                case GooType.White:
                    targetBlockIDs = new List<int>
                    {
                        ModContent.TileType<WhiteLatexTile>(),
                        ModContent.TileType<WhiteLatexSandTile>(),
                        ModContent.TileType<WhiteLatexStoneTile>(),
                        ModContent.TileType<WhiteLatexIceTile>(),
                        ModContent.TileType<WhiteLatexSnowTile>()
                    };
                    targetBlockCount = WhiteLatexBlockCount;
					otherBlockCount1 = BlackLatexBlockCount;
					otherBlockCount2 = DryDirtBlockCount;
                    break;
				default:
                    targetBlockIDs = new List<int>
                    {
                        ModContent.TileType<DryDirt>()
                    };
                    targetBlockCount = DryDirtBlockCount;
					otherBlockCount1 = BlackLatexBlockCount;
					otherBlockCount2 = WhiteLatexBlockCount;
                    break;
            }

			if (targetBlockCount == 0)
				return false;

            var NNeeded = surface ? NBlocksNeeded : NBlocksNeededUnderground;
            bool enoughBlocks = targetBlockCount >= NNeeded && targetBlockCount > otherBlockCount1 && targetBlockCount > otherBlockCount2;
            bool surfaceZone = player.ZoneSkyHeight || player.ZoneOverworldHeight;
            bool undergroundZone = player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || player.ZoneUnderworldHeight;

			if (surface)
				return enoughBlocks && surfaceZone;

			return enoughBlocks && undergroundZone && BlockNearby(player, targetBlockIDs, RadiusFromPlayer);
        }
	}
}
