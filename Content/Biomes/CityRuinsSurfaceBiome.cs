using ChangedSpecialMod.Backgrounds;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Biomes
{

    public class CityRuinsSurfaceBiome : ModBiome
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
            var tileCounter = ModContent.GetInstance<CityRuinsBiomeTileCount>();
            var nDryDirt = tileCounter.DryDirtBlockCount;
            var nWhiteLatex = tileCounter.WhiteLatexBlockCount;
            bool enoughBlocks = nDryDirt >= tileCounter.NBlocksNeeded && nDryDirt > nWhiteLatex;
            bool surfaceZone = player.ZoneSkyHeight || player.ZoneOverworldHeight;
            return enoughBlocks && surfaceZone;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.Environment; //biomehigh

        // Select music
        public override int Music
        {
            get 
            { 
                return MusicLoader.GetMusicSlot(Mod, AudioSystem.GetMusic(LastEnteredPlayer, GooType.None)); 
            }
        }

        // Randomize music when entering the biome
        public override void OnEnter(Player player)
        {
            LastEnteredPlayer = player;
            AudioSystem.RandomizeMusic(player, NPCs.GooType.None);
        }

        public bool IsInBiome()
        {
            for (int k = 0; k < Main.maxPlayers; k++)
            {
                Player player = Main.player[k];
                if (!player.active)
                    continue;

                if (player.InModBiome<CityRuinsSurfaceBiome>())
                    return true;
            }
            return false;
        }
    }
}
