using ChangedSpecialMod.Content.Projectiles;
using ChangedSpecialMod.Content.Tiles.Furniture;
using ChangedSpecialMod.Content.Walls;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Common.Systems
{
    // Animation object for large cages
    public class CageAnimation
    {
        public static float[] animationFrameCounters;
        public static int[] animationFrameIndecis;
        public static float[] animationSpeeds;
        public static bool[] isAnimatingValues;

        int nFrames = 24;
        public int nVariations = 24;
        int updateChance = 300;

        public void Setup()
        {
            animationFrameCounters = new float[nVariations];
            animationFrameIndecis = new int[nVariations];
            animationSpeeds = new float[nVariations];
            isAnimatingValues = new bool[nVariations];
            Update(true);
        }

        // Should only be called once with forceUpdate set to true.
        // It randomizes the values so they don't all start out in the same position
        public void Update(bool forceUpdate = false)
        {
            for (int i = 0; i < nVariations; i++)
            {
                var isAnimating = isAnimatingValues[i];
                var animSpeed = animationSpeeds[i];
                if (Main.rand.NextBool(updateChance) || forceUpdate)
                {
                    isAnimatingValues[i] = forceUpdate ? Main.rand.NextBool(2) : !isAnimating;
                    animSpeed = ChangedUtils.Choose(1, 2);
                    if (Main.rand.NextBool(20))
                        animSpeed = 4;
                    animationSpeeds[i] = animSpeed;
                }

                if (isAnimating)
                {
                    var frameCounter = animationFrameCounters[i] + animSpeed;
                    animationFrameCounters[i] = frameCounter;
                    animationFrameIndecis[i] = forceUpdate ? Main.rand.Next(nFrames) : (int)(frameCounter / 8) % nFrames;
                }
            }
        }

        public int GetFrame(int variation)
        {
            return animationFrameIndecis[variation];
        }
    }

    public class TileAnimationSystem : ModSystem
    {
        public CageAnimation sweeperPuroCageAnimation;
        private int counter = 0;
        public static bool lightOn = true;

        public override void Load()
        {
            sweeperPuroCageAnimation = new CageAnimation();
            sweeperPuroCageAnimation.Setup();
        }

        private void ToggleLights()
        {
            var player = Main.LocalPlayer;
            var playerPos = player.Center.ToTileCoordinates();
            var playerTile = Framing.GetTileSafely(playerPos);
            var shouldFlicker = true;

            if (playerTile.WallType != ModContent.WallType<LabWall>() && !lightOn)
                shouldFlicker = false;

            if (!shouldFlicker)
                return;

            lightOn = !lightOn;
            var tileType = (ushort)ModContent.TileType<FaultyLightTile>();

            var padding = 100;
            var xMin = playerPos.X - padding;
            var xMax = playerPos.X + padding;
            var yMin = playerPos.Y - padding;
            var yMax = playerPos.Y + padding;

            var minDist = float.MaxValue;
            var closestPos = new Point(0, 0);
            var anyLight = false;

            for (int x = xMin; x < xMax; x++)
            {
                // Because lamps are 2 tiles tall, we can check only every two rows
                for (int y = yMin; y < yMax; y += 2)
                {
                    if (!WorldGen.InWorld(x, y))
                        continue;

                    var tile = Main.tile[x, y];
                    if (!tile.HasTile)
                        continue;

                    if (tile.TileType == tileType)
                    {
                        anyLight = true;

                        var tileUp = Framing.GetTileSafely(x, y - 1);
                        var tileDown = Framing.GetTileSafely(x, y + 1);
                        var tileFrameX = !lightOn ? (short)18 : (short)0;

                        tile.TileFrameX = tileFrameX;
                        if (tileUp.TileType == tileType)
                            tileUp.TileFrameX = tileFrameX;
                        if (tileDown.TileType == tileType)
                            tileDown.TileFrameX = tileFrameX;
                        
                        var tilePos = new Point(x, y);
                        var x1 = playerPos.X - tilePos.X;
                        var y1 = playerPos.Y - tilePos.Y;
                        var tmpDist = x1 * x1 + y1 * y1;
                        if (tmpDist < minDist)
                        {
                            minDist = tmpDist;
                            closestPos = tilePos;
                        }
                    }
                }
            }

            if (anyLight)
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                    SoundEngine.PlaySound(Assets.Sounds.SoundLights, new Vector2(closestPos.X * 16, closestPos.Y * 16));
            }
        }

        public static void OnEnterRoom(int roomIndex)
        {
            // If the top left wall of the first room is missing, then this won't work
            var wallTypeBlackGooZone = ModContent.WallType<LabWall>();
            Point16 blackGooRoomPos = new Point16(-1, -1);

            var regionSize = 50;
            for (int x = 0; x < Main.maxTilesX; x += regionSize)
            {
                for (int y = 0; y < Main.maxTilesY; y += regionSize)
                {
                    var tile = Main.tile[x, y];
                    if (tile.WallType == wallTypeBlackGooZone)
                        blackGooRoomPos = new Point16(x, y);
                }
            }

            if (blackGooRoomPos != Point16.NegativeOne)
            {
                var crystalRoomPos = blackGooRoomPos + new Point16(regionSize, 0);
                var libraryRoomPos = blackGooRoomPos + new Point16(2 * regionSize, 0);

                var waterRoomPos = blackGooRoomPos + new Point16(0, regionSize);
                var gasRoomPos = blackGooRoomPos + new Point16(regionSize, regionSize);
                var ventRoomPos = blackGooRoomPos + new Point16(2 * regionSize, regionSize);

                var roomPositions = new Point16[]
                {
                    blackGooRoomPos,
                    crystalRoomPos,
                    libraryRoomPos,
                    waterRoomPos,
                    gasRoomPos,
                    ventRoomPos
                };

                // Get rooms with players inside
                var listActiveRooms = new List<Point16>();
                for (int i = 0; i < roomPositions.Length; i++)
                {
                    var roomPosition = roomPositions[i];
                    for (int j = 0; j < Main.maxPlayers; j++)
                    {
                        var player = Main.player[j];
                        if (!player.active)
                            continue;

                        var playerPos = player.Center.ToTileCoordinates();
                        if (playerPos.X >= roomPosition.X && playerPos.X < roomPosition.X + regionSize &&
                            playerPos.Y >= roomPosition.Y && playerPos.Y < roomPosition.Y + regionSize)
                        {
                            listActiveRooms.Add(roomPosition);
                            break;
                        }
                    }
                }

                //Main.NewText($"{blackGooRoomPos.X}, {blackGooRoomPos.Y}");
                //var localPlayerPos = Main.LocalPlayer.Center.ToTileCoordinates();
                //Main.NewText($"{localPlayerPos.X}, {localPlayerPos.Y}");

                var lightTileType1 = ModContent.TileType<LabLightTile>();
                var lightTileType2 = ModContent.TileType<FaultyLightTile>();
                var generatorTileType = ModContent.TileType<Generator>();

                // Turn off the light for all rooms
                for (int i = 0; i < roomPositions.Length; i++)
                {
                    var roomPosition = roomPositions[i];
                    for (int x = roomPosition.X; x < roomPosition.X + regionSize; x++)
                    {
                        for (int y = roomPosition.Y; y < roomPosition.Y + regionSize; y += 2)
                        {
                            if (x >= Main.maxTilesX || y >= Main.maxTilesY) 
                                continue;

                            var tile = Framing.GetTileSafely(x, y);
                            var tileUp = Framing.GetTileSafely(x, y - 1);
                            var tileDown = Framing.GetTileSafely(x, y + 1);
                            if (tile.HasTile && (tile.TileType == lightTileType1 || tile.TileType == lightTileType2))
                                tile.TileFrameX = 18;
                            if (tileUp.HasTile && (tileUp.TileType == lightTileType1 || tileUp.TileType == lightTileType2))
                                tileUp.TileFrameX = 18;
                            if (tileDown.HasTile && (tileDown.TileType == lightTileType1 || tileDown.TileType == lightTileType2))
                                tileDown.TileFrameX = 18;

                            // Turn off the generator if it is on
                            var isGeneratorTile = tile.TileType == generatorTileType;
                            short xLeft = 4 * 18;
                            var generatorOn = isGeneratorTile && tile.TileFrameX >= xLeft;
                            if (generatorOn)
                            {
                                var topLeftPos = TileObjectData.TopLeft(x, y);

                                for (var j = 0; j < 4; j++)
                                {
                                    for (var k = 0; k < 4; k++)
                                    {
                                        var xx = topLeftPos.X + j;
                                        var yy = topLeftPos.Y + k;
                                        var tile2 = Framing.GetTileSafely(xx, yy);
                                        tile2.TileFrameX = (short)(j * 18);
                                    }
                                }
                            }
                        }
                    }
                }

                // Turn the non-faulty lights for active rooms back on
                for (int i = 0; i < listActiveRooms.Count; i++)
                {
                    var roomPosition = listActiveRooms[i];
                    for (int x = roomPosition.X; x < roomPosition.X + regionSize; x++)
                    {
                        for (int y = roomPosition.Y; y < roomPosition.Y + regionSize; y += 2)
                        {
                            if (x >= Main.maxTilesX || y >= Main.maxTilesY)
                                continue;

                            // Ignore the water room, because you have to turn on the power yourself
                            if (roomPosition == waterRoomPos)
                                continue;

                            var tile = Framing.GetTileSafely(x, y);
                            var tileUp = Framing.GetTileSafely(x, y - 1);
                            var tileDown = Framing.GetTileSafely(x, y + 1);
                            if (tile.HasTile && tile.TileType == lightTileType1)
                                tile.TileFrameX = 0;
                            if (tileUp.HasTile && tileUp.TileType == lightTileType1)
                                tileUp.TileFrameX = 0;
                            if (tileDown.HasTile && tileDown.TileType == lightTileType1)
                                tileDown.TileFrameX = 0;
                        }
                    }
                }
            }
        }

        public override void PostUpdateWorld()
        {
            sweeperPuroCageAnimation.Update();
            ChangedUtils.Snowing();

            if (Main.gamePaused || Main.gameInactive || Main.gameMenu)
                return;

            if (counter % (60 * 3) == 0)
            {
                ToggleLights();
            }

            counter++;
        }
    }
}
