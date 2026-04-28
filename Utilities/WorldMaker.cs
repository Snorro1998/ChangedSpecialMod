using ChangedSpecialMod.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
            int islandIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Smooth World")); //Floating Islands Smooth World
            if (islandIndex != -1)
            {
                tasks.Insert(islandIndex + 1, new PassLegacy("ChangedStructures", (progress, config) =>
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
            }
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
