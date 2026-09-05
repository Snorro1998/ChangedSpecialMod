using ChangedSpecialMod.Backgrounds;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Achievements;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Biomes
{
    public class BlackLatexSurfaceSnowBiome : ModBiome
	{
        // I don't want a custom water style, so I made one as close as I could to vanilla
        // This override must happen, or else the game crashes completely if you take a screenshot in snapshot mode
        public override ModWaterStyle WaterStyle => ModContent.GetInstance<CityRuinsWaterStyle>();
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<BlackLatexSurfaceSnowBackgroundStyle>();
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;
		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => base.BackgroundColor;
		public override string MapBackground => BackgroundPath;

        public Player LastEnteredPlayer;

		public override bool IsBiomeActive(Player player)
        {
            var isActive = TileCountSystem.BiomeActive(player, NPCs.GooType.Black) && TileCountSystem.ActiveBiomeType == BiomeType.Snow;
            if (isActive)
                player.ZoneSnow = true;
            return isActive;
        }

		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

        public override int Music
        {
            get 
            { 
                return MusicLoader.GetMusicSlot(Mod, AudioSystem.GetMusic(LastEnteredPlayer, NPCs.GooType.Black)); 
            }
        }

        public override void OnEnter(Player player)
        {
            LastEnteredPlayer = player;
            AudioSystem.RandomizeMusic(player, NPCs.GooType.Black);
            ModContent.GetInstance<VisitAllBiomesAchievement>().ConditionBlackSurface.Complete();
        }
    }
}
