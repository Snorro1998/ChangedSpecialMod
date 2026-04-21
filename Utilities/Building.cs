using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Furniture;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace ChangedSpecialMod.Utilities
{
    public enum RoomType
    {
        None = 0,
        Normal = 1,
        Elevator = 2,
        BossRoomLeft = 3,
        BossRoomRight = 4
    }

    public class Building
    {
        public Room[] rooms;
        int nHor = 1;
        int nVer = 1;

        int newRoomIndex = 0;

        int[] roomWidths;
        int[] roomHeights;

        public int totalWidth = 0;
        public int totalHeight = 0;

        public int[] layout = null;

        public Building()
        {
            layout = GetLayout();
        }

        private int[] GetLayout()
        {
            int RoomWidthMin = 14;
            int RoomWidthMax = 19;
            int RoomHeightMin = 9;
            int RoomHeightMax = 13;

            int xMinRooms = 4;
            int xMaxRooms = 5;
            int yMinRooms = 3;
            int yMaxRooms = 4;

            nHor = ChangedUtils.WorldGenRandNext(xMinRooms, xMaxRooms + 1);
            nVer = ChangedUtils.WorldGenRandNext(yMinRooms, yMaxRooms + 1);
            int[] roomNums = new int[nHor * nVer];

            // -    empty
            // =    not part of the building, just to illustrate what is below the ground
            // 1    random room
            // 2    staircase
            // 3    left side goo room
            // 4    right side goo room

            //- - - -
            //- - - -
            //- - - -
            //=======
            //- - - -

            int stairPos = 0;

            if (nHor == 3)
                stairPos = Utils.SelectRandom(WorldGen.genRand, 0, 2);
            else
                stairPos = ChangedUtils.WorldGenRandNext(0, nHor);

            var iStart = stairPos * nVer;
            for (int i = iStart; i < iStart + nVer; i++)
                roomNums[i] = (int)RoomType.Elevator;

            //- - 2 -
            //- - 2 -
            //- - 2 -
            //=======
            //- - 2 -

            int towerStairPos = -1;
            var leftStairPos = stairPos - 1;
            var rightStairPos = stairPos + 1;

            if (stairPos == 0)
                towerStairPos = rightStairPos;
            else if (stairPos == nHor - 1)
                towerStairPos = leftStairPos;
            else
                towerStairPos = ChangedUtils.Choose(leftStairPos, rightStairPos);

            iStart = towerStairPos * nVer;
            for (int i = iStart; i < iStart + nVer; i++)
                roomNums[i] = (int)RoomType.Normal;

            //- - 2 1
            //- - 2 1
            //- - 2 1
            //=======
            //- - 2 1

            for (int x = 0; x < nHor; x++)
            {
                if (x == stairPos || x == towerStairPos)
                    continue;

                var nFloors = ChangedUtils.WorldGenRandNext(2, nVer + 1);
                for (int y = 0; y < nVer; y++)
                {
                    if (y + nFloors >= nVer)
                    {
                        roomNums[x * nVer + y] = (int)RoomType.Normal;
                    }
                }
            }

            //- - 2 1
            //1 1 2 1
            //1 1 2 1
            //=======
            //1 1 2 1

            // I really don't know, so just try 1000 times. Should do something if it fails
            for (var i = 0; i < 1000; i++)
            {
                var xBlack1 = ChangedUtils.WorldGenRandNext(0, nHor - 1);
                var yBlack1 = ChangedUtils.WorldGenRandNext(0, nVer - 1);

                var tBlack1 = roomNums[xBlack1 * nVer + yBlack1];
                var tBlack2 = roomNums[xBlack1 * nVer + yBlack1 + nVer];

                var tBlack1Below = roomNums[xBlack1 * nVer + yBlack1 + 1];
                var tBlack2Below = roomNums[xBlack1 * nVer + yBlack1 + nVer + 1];

                if ((tBlack1 == 0 || tBlack1 == 1) && (tBlack2 == 0 || tBlack2 == 1) && (tBlack1Below == 1 && tBlack2Below == 1))
                {
                    roomNums[xBlack1 * nVer + yBlack1] = (int)RoomType.BossRoomLeft;
                    roomNums[xBlack1 * nVer + yBlack1 + nVer] = (int)RoomType.BossRoomRight;
                    break;
                }
            }

            //- - 2 1
            //3 4 2 1
            //1 1 2 1
            //=======
            //1 1 2 1

            roomWidths = ChangedUtils.GetRandomIntegerArray(nHor, RoomWidthMin, RoomWidthMax);
            roomHeights = ChangedUtils.GetRandomIntegerArray(nVer, RoomHeightMin, RoomHeightMax);

            roomWidths[stairPos] = 11;

            return roomNums;
        }

        public void DigOutSpaceForBuilding(int totalWidth, int totalHeight, int xCurrent, int yCurrent)
        {
            for (int x = 0; x < totalWidth; x++)
            {
                for (int y = 0; y < totalHeight; y++)
                {
                    var xx = xCurrent + x;
                    var yy = yCurrent + y;
                    Main.tile[xx, yy].ClearEverything();
                }
            }
        }

        public void Generate(int xPos, int yPos, LabType labType = LabType.CityRuins, bool dontGenerate = false)
        {
            int xCurrent = xPos;
            int yCurrent = yPos;

            totalWidth = roomWidths.Sum();
            if (roomWidths.Length > 1) 
                totalWidth -= (nHor - 1);
            totalHeight = roomHeights.Sum();
            if (roomHeights.Length > 1) 
                totalHeight -= (nVer - 1);

            // Adjust yPos so the building sits on the ground and the basement is below it
            yPos -= totalHeight;
            yPos += roomHeights.Last();
            yCurrent = yPos;

            if (dontGenerate) return;

            var tTypes = new List<int>()
            {
                TileID.Dirt,
                TileID.Stone,
                TileID.Mud,
                TileID.Sand,
                TileID.SnowBlock,
                TileID.IceBlock,

                ModContent.TileType<DryDirt>(),
                ModContent.TileType<WhiteLatexTile>(),
            };

            int maxDown = 50;
            Dictionary<int, int> blockCounts = new Dictionary<int, int>();
            foreach (var tType in tTypes)
            {
                blockCounts.Add(tType, 0);
            }

            var yBottom = yCurrent + totalHeight;
            for (int x = xCurrent; x < xCurrent + totalWidth; x++)
            {
                for (int y = yBottom; y < yBottom + maxDown; y++)
                {
                    var tile = Main.tile[x, y];
                    if (tTypes.Contains(tile.TileType))
                    {
                        blockCounts[tile.TileType]++;
                        break;
                    }
                }
            }

            var mostOccuringTileType = blockCounts.OrderByDescending(kvp => kvp.Value).First().Key;
            for (int x = xCurrent; x < xCurrent + totalWidth; x++)
            {
                for (int y = yBottom; y < yBottom + maxDown; y++)
                {
                    var breakout = false;
                    var tile = Main.tile[x, y];
                    var isCorruptionShaft = tile.WallType == WallID.EbonstoneUnsafe;
                    if (isCorruptionShaft)
                        break;

                    if (tile.HasTile)
                    {
                        // Remove and replace the tile instead of trying to adjust the shape
                        WorldGen.KillTile(x, y, false, false, true);
                        breakout = true;
                    }

                    WorldGen.PlaceTile(x, y, mostOccuringTileType, true, true);
                    if (breakout)
                        break;
                }
            }

            // Dig out space for the building. TODO do it per room so you don't have empty space around the building if inside a mountain or if some rooms don't generate
            DigOutSpaceForBuilding(totalWidth, totalHeight, xCurrent, yCurrent);
            Microsoft.Xna.Framework.Rectangle rect = new Microsoft.Xna.Framework.Rectangle(xCurrent, yCurrent, totalWidth, totalHeight);
            GenVars.structures.AddProtectedStructure(rect, 1);

            // Vertical strips
            for (int x = 0; x < nHor; x++)
            {
                var width = roomWidths[x];

                // Each room in the vertical strip, starting from the top
                for (int y = 0; y < nVer; y++)
                {
                    var height = roomHeights[y];
                    var rr = layout[newRoomIndex % layout.Length];
                    if (rr != (int)RoomType.None)
                    {
                        var floorHeight = nVer - y;
                        var tmpRooms = WorldGenerator.Rooms;
                        var index = WorldGenerator.RoomIndex;

                        var isLowFloor = floorHeight <= 1;

                        Room room = null;
                        var leftSideOpen = false;

                        if (rr == (int)RoomType.Normal)
                        {
                            // Get one of the basement rooms from the global array and increase the index
                            if (isLowFloor)
                            {
                                room = WorldGenerator.RoomsLow[WorldGenerator.RoomIndexLow % WorldGenerator.RoomsLow.Length];
                                WorldGenerator.RoomIndexLow++;
                            }
                            // Get one of the normal rooms from the global array and increase the index
                            else
                            {
                                room = WorldGenerator.Rooms[WorldGenerator.RoomIndex % WorldGenerator.Rooms.Length];
                                WorldGenerator.RoomIndex++;
                            }
                        }
                        else if (rr == (int)RoomType.Elevator)
                            room = new RoomStairs();
                        else if (rr == (int)RoomType.BossRoomLeft || rr == (int)RoomType.BossRoomRight)
                        {
                            room = labType == LabType.White ? new RoomWhiteGoo() : new RoomBlackGoo();
                            if (rr == 4) 
                                leftSideOpen = true;
                        }

                        var flooded = false;

                        // If the room isn't an elevator, there is a chance it will be flooded
                        if (isLowFloor && room is not RoomStairs)
                            flooded = Utils.NextBool(WorldGen.genRand, 2);

                        WorldGenerator.MakeBox(xCurrent, yCurrent, width, height, room.FloorType, room.GetWallType(), (byte)room.FloorPaint, (byte)room.WallPaint, leftSideOpen);

                        if (flooded)
                        {
                            WorldGenerator.FloodRoom(xCurrent, yCurrent, width, height);
                        }

                        WorldGenerator.AddWire(xCurrent, yCurrent, width, height, room.HasLightSwitch);

                        if (room is RoomStairs)
                        {
                            WorldGen.PlaceTile(xCurrent + width / 2, yCurrent + height - 3, ModContent.TileType<Elevator>(), true, true);
                        }

                        if (room.LanternStyle != -1)
                        {
                            WorldGen.PlaceTile(xCurrent + width / 2, yCurrent + 1, TileID.HangingLanterns, true, true, -1, room.LanternStyle);
                            // Turn the light off
                            Main.tile[xCurrent + width / 2, yCurrent + 1].TileFrameX = 18;
                            Main.tile[xCurrent + width / 2, yCurrent + 2].TileFrameX = 18;
                        }
                        else if (room.TorchStyle != -1)
                        {
                            WorldGen.PlaceTile(xCurrent + width / 2, yCurrent + 3, TileID.Torches, true, true, -1, room.TorchStyle);
                            Main.tile[xCurrent + width / 2, yCurrent + 3].TileFrameX = 66;
                        }
                        else if (room.ChandelierStyle != -1)
                        {
                            // All this code just to place a balloon chandelier
                            // Placetile with style 47 just doesn't work
                            // So currenlty it will always place a balloon chandelier if chandelierstyle is not -1
                            var xxPos = xCurrent + width / 2;
                            var yyPos = yCurrent + 1;
                            WorldGen.PlaceTile(xCurrent + width / 2, yCurrent + 1, TileID.Chandeliers, true, true, -1, 0);

                            for (int b = yyPos; b < yyPos + 3; b++)
                            {
                                for (int a = xxPos - 1; a < xxPos + 2; a++)
                                {
                                    Main.tile[a, b].TileFrameX += 162;
                                    Main.tile[a, b].TileFrameY += 540;
                                }
                            }
                        }

                        // Low rooms always have their doors closed, or else the water would spill out of flooded rooms
                        WorldGenerator.AddDoors(xCurrent, yCurrent, width, height, isLowFloor);

                        if (room.HasChest)
                            WorldGenerator.AddChest(xCurrent, width, yCurrent, height);

                        // Try to place furniture 3 times
                        room.PlacedSingleDecor = false;
                        for (int i = 0; i < 3; i++)
                        {
                            var placedAnything = WorldGenerator.AddDecor(room, xCurrent, width, yCurrent, height);
                            if (placedAnything) break;
                        }

                        if (room.HasFans)
                            WorldGenerator.AddFans(xCurrent, width, yCurrent, height);

                        // Flooded rooms don't have paintings on the wall
                        if (room.HasPainting && !flooded)
                            WorldGenerator.AddPainting(xCurrent, width, yCurrent, height);
                    }

                    yCurrent = yCurrent + height - 1;
                    newRoomIndex++;
                }

                yCurrent = yPos;
                xCurrent = xCurrent + width - 1;
            }
        }
    }
}
