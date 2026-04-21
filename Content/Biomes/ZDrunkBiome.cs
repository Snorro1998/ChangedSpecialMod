using ChangedSpecialMod.Backgrounds;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Biomes
{
    // Stupid problems require stupid solutions
    // This biome cannot exist and is only for grouping drunk latecis in the bestiary
    // Also the class is deliberately called ZDrunkBiome to make it appear as the last category 
    // The translations fixes the name anyways
    public class ZDrunkBiome : ModBiome
	{
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<CityRuinsSurfaceBackgroundStyle>();
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;
		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => base.BackgroundColor;
        // Re-uses Bestiary Background for Map Background
        public override string MapBackground => BackgroundPath;
        public Player LastEnteredPlayer;

        public override bool IsBiomeActive(Player player)
        {
            return false;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.None;

        public bool IsInBiome()
        {
            return false;
        }
    }
}
