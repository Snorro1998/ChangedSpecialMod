using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Content.Liquids;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.Tiles;
using Microsoft.Xna.Framework;
using ModLiquidLib.ModLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace ChangedSpecialMod.Utilities
{
    public enum LabType
    {
        CityRuins,
        Black,
        White
    }

    public class LabStruct
    {
        public Vector2 Position = Vector2.Zero;
        public LabType labType;
        public Building Lab = null;

        public LabStruct(Vector2 position)
        {
            Position = position;
            labType = LabType.CityRuins;
            Lab = new Building();
        }
    }

    public class WorldMaker : ModSystem
    {
        public const int distFromBeach = 50;
        public const int distFromWorldCenter = 400;
        public List<LabStruct> newLabs = new List<LabStruct>();

        // TODO: Check for protected structures
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            // If another mod removes this task, nothing will be generated
            int taskIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Smooth World")); //Floating Islands Smooth World
            if (taskIndex != -1)
            {
                tasks.Insert(taskIndex + 1, new PassLegacy("ChangedStructures", (progress, config) =>
                {
                    progress.Message = "Adding Thunder Science Buildings";

                    // Reset the Changed world generation variables. If this didn't happen, you would get variations
                    // when generating the world twice with the same seed
                    WorldGenerator.OnGenerateNewWorld();

                    // The amount of buildings on each side of the world
                    int nBuildings = 0;
                    switch (Main.maxTilesX)
                    {
                        case WorldGen.WorldSizeLargeX:
                            nBuildings = 2;
                            break;
                        case WorldGen.WorldSizeMediumX:
                        case WorldGen.WorldSizeSmallX:
                            nBuildings = 1;
                            break;
                        // Anything else
                        default:
                            nBuildings = Math.Max(1, Main.maxTilesX / 4000);
                            break;
                    }

                    var configNLabs = ChangedSpecialModClientConfig.Instance.NumberOfLabs;
                    if (configNLabs != 1)
                        nBuildings = configNLabs;

                    if (nBuildings == 0)
                        return;

                    var leftBeachPosition = GenVars.leftBeachEnd;
                    var rightBeachPosition = GenVars.rightBeachStart;
                    var worldCenterPosition = Main.maxTilesX / 2;

                    // Left half of the world
                    var xStartLeft = leftBeachPosition + distFromBeach;
                    var xEndLeft = worldCenterPosition - distFromWorldCenter;
                    var widthLeft = xEndLeft - xStartLeft;

                    // Right half of the world
                    var xStartRight = worldCenterPosition + distFromWorldCenter;
                    var xEndRight = rightBeachPosition - distFromBeach;
                    var widthRight = xEndRight - xStartRight;

                    newLabs.Clear();

                    // Create lab classes and calculate their layouts
                    InitLabs(nBuildings, xStartLeft, widthLeft);
                    InitLabs(nBuildings, xStartRight, widthRight);

                    // Update positions
                    UpdateLabPositions();

                    int nLabs = newLabs.Count;
                    int nLabTypes = Enum.GetValues<LabType>().Length;
                    var nBuildingsPerType = Math.Max(1, newLabs.Count / nLabTypes);

                    // By default all labs are the city ruins type, where all latex types can spawn
                    // If there are 2 labs, make one white and the other one black
                    if (nLabs == 2)
                    {
                        FindWhiteLatexLabNearSnow();
                        if (newLabs[0].labType == LabType.White)
                            newLabs[1].labType = LabType.Black;
                        else
                            newLabs[0].labType = LabType.Black;
                    }
                    else if (nLabs > 2)
                    {
                        FindWhiteLatexLabNearSnow();
                        FindOtherWhiteLatexLabs(nBuildingsPerType - 1);
                        FindBlackLatexLabs(nBuildingsPerType);
                    }

                    GenerateLabs();
                }));

                MakeLatexPools(ref tasks, taskIndex + 1);
                HandleSpecialSeeds(ref tasks, taskIndex + 2);
            }
        }

        private void MakeLatexPools(ref List<GenPass> tasks, int taskIndex)
        {
            var lastIndex = tasks.Count - 1;

            tasks.Insert(lastIndex, new PassLegacy("ChangedLatexPools", (progress, config) =>
            {
                progress.Message = "Adding Latex Pools";

                int spacing = 20;

                List<(int x, int y)> possibleLocations = new();

                for (int y = Main.maxTilesY / 2; y < Main.maxTilesY; y += spacing)
                {
                    for (int x = 0; x < Main.maxTilesX; x += spacing)
                    {
                        Tile tile = Main.tile[x, y];

                        if (tile.HasTile &&
                            tile.LiquidAmount > 0 &&
                            tile.LiquidType == LiquidID.Water)
                        {
                            possibleLocations.Add((x, y));
                        }
                    }
                }

                // Shuffle
                possibleLocations = possibleLocations.OrderBy(_ => ChangedUtils.WorldGenRandNext(0, int.MaxValue)).ToList();
                int maxPools = Math.Min(possibleLocations.Count, 20);
                int placedPools = 0;

                for (int i = 0; i < maxPools; i++)
                {
                    if (placedPools == maxPools)
                        return;

                    var poolType = placedPools < maxPools / 2 ? GooType.Black : GooType.White;
                    var liquidType = poolType == GooType.Black ? LiquidLoader.LiquidType<BlackLatexLiquid>() : LiquidLoader.LiquidType<WhiteLatexLiquid>();

                    var x = possibleLocations[i].x;
                    var y = possibleLocations[i].y;
                    var (bounds, waterTiles) = FloodFillWater(x, y);

                    // Skip if a large body of water like the ocean or something from another mod
                    if (waterTiles.Count > 1000)
                        continue;

                    foreach (var (tx, ty) in waterTiles)
                    {
                        var tile = Main.tile[tx, ty];
                        tile.LiquidType = liquidType;
                    }

                    MakeBiomeAroundPool(bounds, 20, 15, poolType);
                    placedPools++;
                }
            }));
        }


        private static void MakeBiomeAroundPool(Rectangle bounds, int paddingX, int paddingY, GooType gooType)
        {
            var blockType = gooType == GooType.Black ? ModContent.TileType<BlackLatexTile>() : ModContent.TileType<WhiteLatexTile>();

            float radiusX = bounds.Width / 2f + paddingX;
            float radiusY = bounds.Height / 2f + paddingY;

            float centerX = bounds.Center.X;
            float centerY = bounds.Center.Y;

            // --------------------------------------------------------------------
            // Generate an irregular outline.
            // --------------------------------------------------------------------

            const int Samples = 180; // One sample every 2 degrees.

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

            // --------------------------------------------------------------------
            // Fill inside the irregular ellipse.
            // --------------------------------------------------------------------

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
                            tile.TileType = (ushort)blockType;
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

            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();

                if (!visited.Add((x, y)))
                    continue;

                if (x < 0 || x >= Main.maxTilesX ||
                    y < 0 || y >= Main.maxTilesY)
                    continue;

                Tile tile = Main.tile[x, y];

                if (tile.LiquidAmount == 0 ||
                    tile.LiquidType != LiquidID.Water)
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
                minX,
                minY,
                maxX - minX + 1,
                maxY - minY + 1);

            return (bounds, waterTiles);
        }
        /*
        private static void FloodFillLiquid(int startX, int startY)
        {
            Queue<(int x, int y)> queue = new();
            HashSet<(int x, int y)> visited = new();

            queue.Enqueue((startX, startY));

            while (queue.Count > 0)
            {
                // Stop if it is a large body of water, like the ocean or something from another mod
                if (visited.Count > 300)
                    break;

                var (x, y) = queue.Dequeue();

                if (!visited.Add((x, y)))
                    continue;

                if (x < 0 || x >= Main.maxTilesX ||
                    y < 0 || y >= Main.maxTilesY)
                    continue;

                Tile tile = Main.tile[x, y];

                if (tile.LiquidAmount == 0 ||
                    tile.LiquidType != LiquidID.Water)
                    continue;

                tile.LiquidType = LiquidLoader.LiquidType<BlackLatexLiquid>();

                queue.Enqueue((x + 1, y));
                queue.Enqueue((x - 1, y));
                queue.Enqueue((x, y + 1));
                queue.Enqueue((x, y - 1));
            }
        }
        */
        private void HandleSpecialSeeds(ref List<GenPass> tasks, int taskIndex)
        {
            var worldSeedName = WorldGen.currentWorldSeed.ToLower();
            var seedBlackLatex = worldSeedName.Contains("blackgoozone");
            var seedWhiteLatex = worldSeedName.Contains("whitegoojungle");
            var seedDryDirt = worldSeedName.Contains("drydrydirt");
            var seedOrange = worldSeedName.Contains("muchorange");

            if (seedBlackLatex)
                AddLatexEverywhereTask(ref tasks, taskIndex, Content.NPCs.GooType.Black);
            else if (seedWhiteLatex)
                AddLatexEverywhereTask(ref tasks, taskIndex, Content.NPCs.GooType.White);
            else if (seedDryDirt)
                AddLatexEverywhereTask(ref tasks, taskIndex, Content.NPCs.GooType.None);
            else if (seedOrange)
            {
                var lastIndex = tasks.Count - 1;
                var color = PaintID.DeepOrangePaint;
                tasks.Insert(lastIndex, new PassLegacy("ChangedSeedOrange", (progress, config) =>
                {
                    progress.Message = "Orange";

                    for (int y = 0; y < Main.maxTilesY; y++)
                    {
                        for (int x = 0; x < Main.maxTilesX; x++)
                        {
                            var tile = Main.tile[x, y];
                            if (tile != null)
                            {
                                if (tile.HasTile)
                                    tile.TileColor = color;
                                if (tile.WallType != WallID.None)
                                    tile.WallColor = color;
                            }
                        }
                    }
                }));
            }
        }

        private void AddLatexEverywhereTask(ref List<GenPass> tasks, int passIndex, Content.NPCs.GooType gooType)
        {
            tasks.Insert(passIndex, new PassLegacy("ChangedSeedLatexEverywhere", (progress, config) =>
            {
                progress.Message = "Spreading too much latex";

                for (int y = 0; y < Main.worldSurface; y++)
                {
                    for (int x = 0; x < Main.maxTilesX; x++)
                    {
                        var tile = Main.tile[x, y];
                        if (tile != null && tile.HasTile)
                        {
                            var tileType = WorldGenerator.GetTileType(tile, gooType);
                            if (tileType != -1)
                                tile.TileType = (ushort)tileType;
                        }
                    }
                }
            }));
        }

        private void GenerateLabs()
        {
            var biomeWidth = 100;
            switch (Main.maxTilesX)
            {
                case WorldGen.WorldSizeLargeX:
                    biomeWidth = 100;
                    break;
                case WorldGen.WorldSizeMediumX:
                    biomeWidth = 76;
                    break;
                case WorldGen.WorldSizeSmallX:
                    biomeWidth = 50;
                    break;
                // Custom world size
                default:
                    biomeWidth = Math.Max(50, Main.maxTilesX / 84);
                    break;
            }

            for (int i = 0; i < newLabs.Count; i++)
            {
                var lab = newLabs[i];
                var pos = lab.Position;
                var latexType = lab.labType;
                lab.Lab.Generate((int)pos.X, (int)pos.Y, latexType);
                WorldGenerator.CreateLatexBiome((int)pos.X, (int)pos.Y, lab.Lab.totalWidth, lab.Lab.totalHeight, biomeWidth, 50, latexType);
            }
        }

        private void FindWhiteLatexLabNearSnow()
        {
            var snowCenter = GenVars.snowOriginLeft + (GenVars.snowOriginRight - GenVars.snowOriginLeft) / 2;
            var minDist = int.MaxValue;
            var snowLabIndex = -1;
            for (var i = 0; i < newLabs.Count; i++)
            {
                var lab = newLabs[i];
                var dist = Math.Abs((int)lab.Position.X - snowCenter);
                if (dist < minDist)
                {
                    minDist = dist;
                    snowLabIndex = i;
                }
            }
            if (snowLabIndex != -1)
            {
                newLabs[snowLabIndex].labType = LabType.White; 
            }
        }

        private void FindOtherWhiteLatexLabs(int nBuildingsPerType)
        {
            if (nBuildingsPerType < 1)
                return;
            var labList = new List<int>();
            for (int i = 0; i < newLabs.Count; i++)
            {
                var labType = newLabs[i].labType;
                if (labType == LabType.CityRuins)
                    labList.Add(i);
            }
            labList = labList.OrderBy(_ => ChangedUtils.WorldGenRandNext(0, Int32.MaxValue)).ToList();
            for (int i = 0; i < nBuildingsPerType; i++)
            {
                newLabs[labList[i]].labType = LabType.White;
            }
        }

        private void FindBlackLatexLabs(int nBuildingsPerType)
        {
            if (nBuildingsPerType < 1)
                return;
            var labList = new List<int>();
            for (int i = 0; i < newLabs.Count; i++)
            {
                var labType = newLabs[i].labType;
                if (labType == LabType.CityRuins)
                    labList.Add(i);
            }
            labList = labList.OrderBy(_ => ChangedUtils.WorldGenRandNext(0, Int32.MaxValue)).ToList();
            for (int i = 0; i < nBuildingsPerType; i++)
            {
                newLabs[labList[i]].labType = LabType.Black;
            }
        }

        private void InitLabs(int nBuildings, int xStart, int widthTotal)
        {
            var isLeftSide = xStart < Main.maxTilesX / 2;
            var spacing = widthTotal / nBuildings;

            for (int i = 0; i < nBuildings; i++)
            {
                var halfSpace = spacing / 2;
                int x = xStart + i * spacing + halfSpace;
                int randomXShift = ChangedUtils.WorldGenRandNext(-halfSpace / 2, halfSpace / 2);
                x += randomXShift;

                var pos = new Vector2(x, (int)GenVars.worldSurfaceLow);
                newLabs.Add(new LabStruct(pos));
            }
        }

        private void UpdateLabPositions()
        {
            for (int i = 0; i < newLabs.Count; i++)
            {
                var lab = newLabs[i];
                var pos = lab.Position;
                var isLeftSide = pos.X < Main.maxTilesX / 2;

                // Move the position if it is too close to the dungeon
                if (Math.Abs(pos.X - GenVars.dungeonX) < 100)
                {
                    lab.Position.X = isLeftSide ? pos.X + 200 : pos.X - 200;
                }

                UpdateLabPositionInner(lab);
            }
        }

        private void UpdateLabPositionInner(LabStruct lab)
        {
            var x = (int)lab.Position.X;

            // Block on which a lab should not generate on
            var IgnoreBlockTypes = new List<int>()
            {
                TileID.LivingWood,
                TileID.LeafBlock,
                TileID.LivingMahogany,
                TileID.LivingMahoganyLeaves
            };

            int y = (int)GenVars.worldSurfaceLow;

            var isLeftWorldHalf = x < Main.maxTilesX / 2;
            var maxAttempts = 3;
            var attempt = 0;
            var xShiftOnFailSmall = 10;
            var xShiftOnFailBig = 100;

            // TODO to fix this, a building class should be created and getlayout should be called before this code where the position is determined
            var labWidth = lab.Lab.totalWidth;

            while (y < Main.worldSurface)
            {
                var wallType = Main.tile[x, y].WallType;
                var isCorruptionShaft = wallType == WallID.EbonstoneUnsafe;

                // Shift the position to check at if it is checking inside a corruption shaft
                if (isCorruptionShaft)
                {
                    attempt++;
                    // Reset the height to check from
                    y = (int)GenVars.worldSurfaceLow;
                    if (isLeftWorldHalf)
                    {
                        x += xShiftOnFailSmall;
                    }
                    else
                    {
                        x = x - xShiftOnFailSmall - labWidth;
                    }
                }

                // We struck a solid block
                else if (WorldGen.SolidTile(x, y))
                {
                    var tileType = Main.tile[x, y].TileType;

                    // Add the position if a lab can generate on this block type
                    // The position will also be added if the check for a valid block failed too many times
                    if (!IgnoreBlockTypes.Contains((int)tileType) || attempt >= maxAttempts)
                    {
                        lab.Position = new Vector2(x, y);
                        return;
                    }

                    // We struck a block that a lab shouldn't generate on
                    else if (IgnoreBlockTypes.Contains((int)tileType))
                    {
                        attempt++;
                        // Reset the height to check from
                        y = (int)GenVars.worldSurfaceLow;
                        if (isLeftWorldHalf)
                        {
                            x += xShiftOnFailBig;
                        }
                        else
                        {
                            x = x - xShiftOnFailBig - labWidth;
                        }
                    }
                }
                y++;
            }
            // if we get here it failed
        }
    }
}
