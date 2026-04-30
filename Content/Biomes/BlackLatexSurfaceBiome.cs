using ChangedSpecialMod.Backgrounds;
using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;
using ChangedSpecialMod.Utilities;

namespace ChangedSpecialMod.Content.Biomes
{
	// Shows setting up two basic biomes. For a more complicated example, please request.

    public class BlackLatexSurfaceBiome : ModBiome
	{
        public string CurrentMusic = Sounds.MusicBlackLatexZone;

        // Select all the scenery
        //public override ModWaterStyle WaterStyle => ModContent.GetInstance<CityRuinsWaterStyle>(); // Sets a water style for when inside this biome
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<BlackLatexSurfaceBackgroundStyle>();
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
            var tileCounter = ModContent.GetInstance<CityRuinsBiomeTileCount>();
            var nDryDirt = tileCounter.DryDirtBlockCount;
            var nWhiteLatex = tileCounter.WhiteLatexBlockCount;
            var nBlackLatex = tileCounter.BlackLatexBlockCount;
            bool enoughBlocks = nBlackLatex >= tileCounter.NBlocksNeeded && nBlackLatex > nDryDirt && nBlackLatex > nWhiteLatex;
			bool surfaceZone = player.ZoneSkyHeight || player.ZoneOverworldHeight;
			return enoughBlocks && surfaceZone;
		}

		public override SceneEffectPriority Priority => SceneEffectPriority.Environment; // Biomehigh

        // Select music
        public override int Music
        {
            get 
            { 
                return MusicLoader.GetMusicSlot(Mod, AudioSystem.GetMusic(LastEnteredPlayer, NPCs.GooType.Black)); 
            }
        }

        // Randomize music when entering the biome
        public override void OnEnter(Player player)
        {
            LastEnteredPlayer = player;
            AudioSystem.RandomizeMusic(player, NPCs.GooType.Black);
        }

        public bool IsInBiome()
        {
            for (int k = 0; k < Main.maxPlayers; k++)
            {
                Player player = Main.player[k];
                if (!player.active)
                {
                    continue;
                }

                if (player.InModBiome<BlackLatexSurfaceBiome>())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
