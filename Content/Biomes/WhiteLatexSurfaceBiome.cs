using ChangedSpecialMod.Backgrounds;
using ChangedSpecialMod.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Biomes
{
    public class WhiteLatexSurfaceBiome : ModBiome
    {
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<WhiteLatexSurfaceBackgroundStyle>();
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        // Populate the Bestiary Filter
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
            var nBlackLatex = tileCounter.BlackLatexBlockCount;
            bool enoughBlocks = nWhiteLatex >= tileCounter.NBlocksNeeded && nWhiteLatex > nDryDirt && nWhiteLatex > nBlackLatex;
            bool surfaceZone = player.ZoneSkyHeight || player.ZoneOverworldHeight;
            return enoughBlocks && surfaceZone;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment; // Biomehigh

        public override int Music
        {
            get { return MusicLoader.GetMusicSlot(Mod, AudioSystem.GetMusic(LastEnteredPlayer, NPCs.GooType.White)); }
        }

        public override void OnEnter(Player player)
        {
            LastEnteredPlayer = player;
            AudioSystem.RandomizeMusic(player, NPCs.GooType.White);
        }

        public bool IsInBiome()
        {
            for (int k = 0; k < Main.maxPlayers; k++)
            {
                Player player = Main.player[k];
                if (!player.active)
                    continue;

                if (player.InModBiome<WhiteLatexSurfaceBiome>())
                    return true;
            }
            return false;
        }
    }
}
