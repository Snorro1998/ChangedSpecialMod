using ChangedSpecialMod.Content.Biomes;
using Terraria;

namespace ChangedSpecialMod.Common
{
    public static class Conditions
    {
        public static Condition InBlackLatexSurfaceBiome = new Condition("Mods.ChangedSpecialMod.Conditions.InBlackLatexSurfaceBiome", () => Main.LocalPlayer.InModBiome<BlackLatexSurfaceBiome>());
        public static Condition InWhiteLatexSurfaceBiome = new Condition("Mods.ChangedSpecialMod.Conditions.InWhiteLatexSurfaceBiome", () => Main.LocalPlayer.InModBiome<WhiteLatexSurfaceBiome>());
    }
}
