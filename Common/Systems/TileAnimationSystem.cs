using ChangedSpecialMod.Utilities;
using Terraria;
using Terraria.ModLoader;

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

        public override void Load()
        {
            sweeperPuroCageAnimation = new CageAnimation();
            sweeperPuroCageAnimation.Setup();
        }

        public override void PostUpdateWorld()
        {
            sweeperPuroCageAnimation.Update();
        }
    }
}
