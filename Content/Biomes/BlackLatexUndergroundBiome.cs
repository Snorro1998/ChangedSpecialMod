using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Backgrounds;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Achievements;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Biomes
{
	// Shows setting up two basic biomes. For a more complicated example, please request.

    public class BlackLatexUndergroundBiome : ModBiome
	{
        // I don't want a custom water style, so I made one as close as I could to vanilla
        // This override must happen, or else the game crashes completely if you take a screenshot in snapshot mode
        public override ModWaterStyle WaterStyle => ModContent.GetInstance<CityRuinsWaterStyle>();
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<BlackLatexSurfaceBackgroundStyle>();
        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.GetInstance<BlackLatexUndergroundBackgroundStyle>();
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

		// Populate the Bestiary Filter
		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => base.BackgroundColor;
		public override string MapBackground => BackgroundPath; // Re-uses Bestiary Background for Map Background

        public Player LastEnteredPlayer;

		// Calculate when the biome is active.
		public override bool IsBiomeActive(Player player)
        {
            return CityRuinsBiomeTileCount.BiomeActive(player, NPCs.GooType.Black, false);
		}

		public override SceneEffectPriority Priority => SceneEffectPriority.Environment; // Biomehigh

        // Select music
        public override int Music
        {
            get 
            { 
                return MusicLoader.GetMusicSlot(Mod, AudioSystem.GetMusic(LastEnteredPlayer, NPCs.GooType.Black, true)); 
            }
        }

        public override void OnEnter(Player player)
        {
            LastEnteredPlayer = player;
            AudioSystem.RandomizeMusic(player, NPCs.GooType.Black);
            ModContent.GetInstance<VisitAllBiomesAchievement>().ConditionBlackCave.Complete();
        }

        public bool IsInBiome()
        {
            for (int k = 0; k < Main.maxPlayers; k++)
            {
                Player player = Main.player[k];
                if (!player.active)
                    continue;

                if (player.InModBiome<BlackLatexUndergroundBiome>())
                    return true;
            }
            return false;
        }
    }
}
