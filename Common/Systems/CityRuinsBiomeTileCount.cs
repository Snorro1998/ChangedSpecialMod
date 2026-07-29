using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Latex.Black;
using ChangedSpecialMod.Content.Tiles.Latex.White;
using ChangedSpecialMod.Utilities;
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

		public static int BlackLatexBlockCount;
		public static int BlackLatexNormalBlockCount;
        public static int BlackLatexDesertBlockCount;
        public static int BlackLatexJungleBlockCount;
        public static int BlackLatexSnowBlockCount;

        public static int WhiteLatexBlockCount;
        public static int WhiteLatexNormalBlockCount;
        public static int WhiteLatexDesertBlockCount;
        public static int WhiteLatexJungleBlockCount;
        public static int WhiteLatexSnowBlockCount;

        public enum BiomeType
        {
            Normal,
            Desert,
            Jungle,
            Snow
        }

        public static BiomeType ActiveBiomeType = BiomeType.Normal;

        private void GetBlackTileCount(ReadOnlySpan<int> tileCounts)
        {
            var blackNormalTiles = new List<int>()
            {
                ModContent.TileType<BlackLatexGrassTile>(),
                ModContent.TileType<BlackLatexTile>(),
                ModContent.TileType<BlackLatexStoneTile>()
            };

            var blackDesertTiles = new List<int>()
            {
                ModContent.TileType<BlackLatexSandTile>(),
            };

            var blackJungleTiles = new List<int>()
            {
                ModContent.TileType<BlackLatexJungleGrassTile>(),
                ModContent.TileType<BlackLatexMudTile>(),
            };

            var blackSnowTiles = new List<int>()
            {
                ModContent.TileType<BlackLatexSnowTile>(),
                ModContent.TileType<BlackLatexIceTile>(),
            };

            BlackLatexNormalBlockCount = 0;
            BlackLatexDesertBlockCount = 0;
            BlackLatexJungleBlockCount = 0;
            BlackLatexSnowBlockCount = 0;

            foreach (var tile in blackNormalTiles)
                BlackLatexNormalBlockCount += tileCounts[tile];

            foreach (var tile in blackDesertTiles)
                BlackLatexDesertBlockCount += tileCounts[tile];

            foreach (var tile in blackJungleTiles)
                BlackLatexJungleBlockCount += tileCounts[tile];

            foreach (var tile in blackSnowTiles)
                BlackLatexSnowBlockCount += tileCounts[tile];

            BlackLatexBlockCount = BlackLatexNormalBlockCount + BlackLatexDesertBlockCount + BlackLatexJungleBlockCount + BlackLatexSnowBlockCount;
        }

        private void GetWhiteTileCount(ReadOnlySpan<int> tileCounts)
        {
            var WhiteNormalTiles = new List<int>()
            {
                ModContent.TileType<WhiteLatexGrassTile>(),
                ModContent.TileType<WhiteLatexTile>(),
                ModContent.TileType<WhiteLatexStoneTile>()
            };

            var WhiteDesertTiles = new List<int>()
            {
                ModContent.TileType<WhiteLatexSandTile>(),
            };

            var WhiteJungleTiles = new List<int>()
            {
                ModContent.TileType<WhiteLatexJungleGrassTile>(),
                ModContent.TileType<WhiteLatexMudTile>(),
            };

            var WhiteSnowTiles = new List<int>()
            {
                ModContent.TileType<WhiteLatexSnowTile>(),
                ModContent.TileType<WhiteLatexIceTile>(),
            };

            WhiteLatexNormalBlockCount = 0;
            WhiteLatexDesertBlockCount = 0;
            WhiteLatexJungleBlockCount = 0;
            WhiteLatexSnowBlockCount = 0;

            foreach (var tile in WhiteNormalTiles)
                WhiteLatexNormalBlockCount += tileCounts[tile];

            foreach (var tile in WhiteDesertTiles)
                WhiteLatexDesertBlockCount += tileCounts[tile];

            foreach (var tile in WhiteJungleTiles)
                WhiteLatexJungleBlockCount += tileCounts[tile];

            foreach (var tile in WhiteSnowTiles)
                WhiteLatexSnowBlockCount += tileCounts[tile];

            WhiteLatexBlockCount = WhiteLatexNormalBlockCount + WhiteLatexDesertBlockCount + WhiteLatexJungleBlockCount + WhiteLatexSnowBlockCount;
        }

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) 
		{
            GetBlackTileCount(tileCounts);
            GetWhiteTileCount(tileCounts);
            DryDirtBlockCount = tileCounts[ModContent.TileType<DryDirt>()];
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
            {
                if (enoughBlocks && surfaceZone)
                {
                    switch (gooType)
                    {
                        case GooType.Black:
                            if (BlackLatexDesertBlockCount > BlackLatexJungleBlockCount &&
                                BlackLatexDesertBlockCount > BlackLatexSnowBlockCount &&
                                BlackLatexDesertBlockCount >= BlackLatexNormalBlockCount)
                                ActiveBiomeType = BiomeType.Desert;

                            else if (BlackLatexJungleBlockCount > BlackLatexDesertBlockCount &&
                                BlackLatexJungleBlockCount > BlackLatexSnowBlockCount &&
                                BlackLatexJungleBlockCount >= BlackLatexNormalBlockCount)
                                ActiveBiomeType = BiomeType.Jungle;

                            else if (BlackLatexSnowBlockCount > BlackLatexDesertBlockCount &&
                                BlackLatexSnowBlockCount > BlackLatexJungleBlockCount &&
                                BlackLatexSnowBlockCount >= BlackLatexNormalBlockCount)
                                ActiveBiomeType = BiomeType.Snow;

                            else
                                ActiveBiomeType = BiomeType.Normal;

                            break;
                        case GooType.White:
                            if (WhiteLatexDesertBlockCount > WhiteLatexJungleBlockCount &&
                                WhiteLatexDesertBlockCount > WhiteLatexSnowBlockCount &&
                                WhiteLatexDesertBlockCount >= WhiteLatexNormalBlockCount)
                                ActiveBiomeType = BiomeType.Desert;

                            else if (WhiteLatexJungleBlockCount > WhiteLatexDesertBlockCount &&
                                WhiteLatexJungleBlockCount > WhiteLatexSnowBlockCount &&
                                WhiteLatexJungleBlockCount >= WhiteLatexNormalBlockCount)
                                ActiveBiomeType = BiomeType.Jungle;

                            else if (WhiteLatexSnowBlockCount > WhiteLatexDesertBlockCount &&
                                WhiteLatexSnowBlockCount > WhiteLatexJungleBlockCount &&
                                WhiteLatexSnowBlockCount >= WhiteLatexNormalBlockCount)
                                ActiveBiomeType = BiomeType.Snow;

                            else
                                ActiveBiomeType = BiomeType.Normal;

                            break;
                    }
                    return true;
                }

                return false;
            }

            // Underground
			return enoughBlocks && undergroundZone && BlockNearby(player, targetBlockIDs, RadiusFromPlayer);
        }
	}
}
