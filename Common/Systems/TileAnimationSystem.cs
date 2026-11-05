using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using ChangedSpecialMod.Content.Tiles.Furniture;
using ChangedSpecialMod.Content.Walls;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI;
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
        private bool lightOn = true;

        public override void Load()
        {
            sweeperPuroCageAnimation = new CageAnimation();
            sweeperPuroCageAnimation.Setup();
        }

        private void ToggleLights()
        {
            lightOn = !lightOn;
            var tileType = (ushort)ModContent.TileType<FaultyLightTile>();
            var player = Main.LocalPlayer;

            var playerPos = player.Center.ToTileCoordinates();
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
                for (int y = yMin; y < yMax; y++)
                {
                    if (!WorldGen.InWorld(x, y))
                        continue;

                    var tile = Main.tile[x, y];
                    if (!tile.HasTile)
                        continue;

                    if (tile.TileType == tileType)
                    {
                        anyLight = true;
                        if (!lightOn)
                            tile.TileFrameX = 18;
                        else
                            tile.TileFrameX = 0;

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

        public override void PostUpdateWorld()
        {
            sweeperPuroCageAnimation.Update();
            ChangedUtils.Snowing();

            if (counter % (60 * 3) == 0)
            {
                ToggleLights();
            }

            counter++;
        }
    }
}
