using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace ChangedSpecialMod.Common.WorldGeneration.Passes
{
    public class PassLatexCaves : WorldGenerationPass
    {
        public PassLatexCaves()
        {
            PassName = "ChangedLatexPools";
            GenerationMessage = "LatexCaves";
        }

        public override bool Enabled()
        {
            return ChangedSpecialModClientConfig.Instance.WorldGenStepLatexCaves;
        }

        public override void Generate()
        {
            var blackListBlockTypes = new List<int>()
            {
                TileID.Granite,
                TileID.Marble,
                TileID.BlueDungeonBrick,
                TileID.GreenDungeonBrick,
                TileID.PinkDungeonBrick,
                TileID.LihzahrdBrick,
                TileID.MushroomGrass,
                TileID.Hive
            };

            int spacing = 10;
            var minDistBetweenPools = 300;
            int xDistFromBorder = 200;

            List<(int x, int y)> possibleLocations = new();

            for (int y = (int)(Main.maxTilesY * 0.4f); y < Main.maxTilesY; y += spacing)
            {
                for (int x = xDistFromBorder; x < Main.maxTilesX - xDistFromBorder; x += spacing)
                {
                    Tile tile = Main.tile[x, y];

                    if (tile.HasTile &&
                        tile.LiquidAmount > 0 &&
                        (tile.LiquidType == LiquidID.Water || tile.LiquidType == LiquidID.Lava))
                    {
                        possibleLocations.Add((x, y));
                    }
                }
            }

            // Shuffle
            possibleLocations = possibleLocations.OrderBy(_ => ChangedUtils.WorldGenRandNext(0, int.MaxValue)).ToList();

            int maxAmount = 5;

            if (Main.maxTilesX >= WorldGen.WorldSizeLargeX)
                maxAmount = 15;
            else if (Main.maxTilesX >= WorldGen.WorldSizeMediumX)
                maxAmount = 10;

            int maxPools = Math.Min(possibleLocations.Count, maxAmount);
            int placedPools = 0;

            List<(int, int)> placedPositions = new List<(int, int)>();

            for (int i = 0; i < possibleLocations.Count; i++)
            {
                if (placedPools == maxPools)
                    return;

                var poolType = placedPools < maxPools / 2 ? GooType.Black : GooType.White;
                var x = possibleLocations[i].x;
                var y = possibleLocations[i].y;

                // Don't place it if too close to another pool
                foreach (var position in placedPositions)
                {
                    var dist = Vector2.DistanceSquared(new Vector2(position.Item1, position.Item2), new Vector2(x, y));

                    if (dist < minDistBetweenPools * minDistBetweenPools)
                        goto labelSkip;
                }

                var (bounds, waterTiles) = FloodFillWater(x, y);

                // Skip if a large body of water like the ocean or something from another mod
                if (waterTiles.Count < 100 || waterTiles.Count > 1000)
                    goto labelSkip;

                for (int j = bounds.Left; j < bounds.Left + bounds.Width; j++)
                {
                    for (int k = bounds.Top; k < bounds.Top + bounds.Height; k++)
                    {
                        var tmpTile = Framing.GetTileSafely(j, k);
                        if (tmpTile != null & tmpTile.HasTile && blackListBlockTypes.Contains(tmpTile.TileType))
                            goto labelSkip;
                    }
                }

                //padding 20 15
                MakeBiomeAroundPool(bounds, 30, 25, poolType);
                placedPositions.Add((x, y));
                placedPools++;

                labelSkip:
                var dummy = 0;
            }
        }

        private static void MakeBiomeAroundPool(Rectangle bounds, int paddingX, int paddingY, GooType gooType)
        {
            float radiusX = bounds.Width / 2f + paddingX;
            float radiusY = bounds.Height / 2f + paddingY;

            float centerX = bounds.Center.X;
            float centerY = bounds.Center.Y;


            // Generate an irregular outline. One sample every 2 degrees.
            const int Samples = 180;

            float[] offsets = new float[Samples];

            // Initial random offsets.
            for (int i = 0; i < Samples; i++)
                offsets[i] = WorldGen.genRand.NextFloat(-0.18f, 0.18f);

            // Smooth them several times so nearby angles have similar values.
            for (int pass = 0; pass < 4; pass++)
            {
                float[] temp = new float[Samples];

                for (int i = 0; i < Samples; i++)
                {
                    float prev = offsets[(i - 1 + Samples) % Samples];
                    float curr = offsets[i];
                    float next = offsets[(i + 1) % Samples];

                    temp[i] = (prev + curr * 2f + next) / 4f;
                }

                offsets = temp;
            }

            // Fill inside the irregular ellipse.
            int minX = Math.Max(0, (int)Math.Floor(centerX - radiusX * 1.3f));
            int maxX = Math.Min(Main.maxTilesX - 1, (int)Math.Ceiling(centerX + radiusX * 1.3f));
            int minY = Math.Max(0, (int)Math.Floor(centerY - radiusY * 1.3f));
            int maxY = Math.Min(Main.maxTilesY - 1, (int)Math.Ceiling(centerY + radiusY * 1.3f));

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    float dx = x - centerX;
                    float dy = y - centerY;

                    float angle = MathF.Atan2(dy, dx);
                    if (angle < 0)
                        angle += MathHelper.TwoPi;

                    int index = (int)(angle / MathHelper.TwoPi * Samples);
                    index = Math.Clamp(index, 0, Samples - 1);

                    float radiusScale = 1f + offsets[index];

                    float nx = dx / (radiusX * radiusScale);
                    float ny = dy / (radiusY * radiusScale);

                    if (nx * nx + ny * ny > 1f)
                        continue;

                    Tile tile = Main.tile[x, y];

                    if (tile.HasTile)
                    {
                        var newTileType = WorldGenerator.GetTileType(tile, gooType);
                        if (newTileType != -1)
                            tile.TileType = (ushort)newTileType;
                    }
                }
            }
        }

        private static (Rectangle Bounds, List<(int x, int y)> Tiles) FloodFillWater(int startX, int startY)
        {
            Queue<(int x, int y)> queue = new();
            HashSet<(int x, int y)> visited = new();
            List<(int x, int y)> waterTiles = new();

            queue.Enqueue((startX, startY));

            int minX = startX;
            int maxX = startX;
            int minY = startY;
            int maxY = startY;

            List<int> blackListLiquids = new List<int>
            {
                LiquidID.Honey,
                LiquidID.Shimmer
            };

            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();

                if (!visited.Add((x, y)))
                    continue;

                if (x < 0 || x >= Main.maxTilesX ||
                    y < 0 || y >= Main.maxTilesY)
                    continue;

                Tile tile = Main.tile[x, y];

                if (tile.LiquidAmount == 0 || blackListLiquids.Contains(tile.LiquidType))
                    continue;

                waterTiles.Add((x, y));

                minX = Math.Min(minX, x);
                maxX = Math.Max(maxX, x);
                minY = Math.Min(minY, y);
                maxY = Math.Max(maxY, y);

                queue.Enqueue((x + 1, y));
                queue.Enqueue((x - 1, y));
                queue.Enqueue((x, y + 1));
                queue.Enqueue((x, y - 1));
            }

            Rectangle bounds = new Rectangle(
                minX - 2,
                minY - 2,
                maxX - minX + 3,
                maxY - minY + 3);

            return (bounds, waterTiles);
        }
    }
}
