using ChangedSpecialMod.Content.Biomes;
using Terraria;

namespace ChangedSpecialMod.Common
{
    public static class Conditions
    {
        public static Condition InBlackLatexBiome = new Condition(
            "Mods.ChangedSpecialMod.Conditions.InBlackLatexBiome", 
            () => Main.LocalPlayer.InModBiome<BlackLatexSurfaceBiome>() ||
            Main.LocalPlayer.InModBiome<BlackLatexUndergroundBiome>() ||
            Main.LocalPlayer.InModBiome<BlackLatexSurfaceDesertBiome>() ||
            Main.LocalPlayer.InModBiome<BlackLatexSurfaceJungleBiome>() ||
            Main.LocalPlayer.InModBiome<BlackLatexSurfaceSnowBiome>());
        public static Condition InWhiteLatexBiome = new Condition(
            "Mods.ChangedSpecialMod.Conditions.InWhiteLatexBiome",
            () => Main.LocalPlayer.InModBiome<WhiteLatexSurfaceBiome>() ||
            Main.LocalPlayer.InModBiome<WhiteLatexUndergroundBiome>() ||
            Main.LocalPlayer.InModBiome<WhiteLatexSurfaceDesertBiome>() ||
            Main.LocalPlayer.InModBiome<WhiteLatexSurfaceJungleBiome>() ||
            Main.LocalPlayer.InModBiome<WhiteLatexSurfaceSnowBiome>());
    }
}
