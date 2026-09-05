using ChangedSpecialMod.Backgrounds;
using ChangedSpecialMod.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Biomes
{
    // This was in the original ChangedMod, but it is no longer used
	public class CityRuinsUndergroundBiome : ModBiome
	{
        // I don't want a custom water style, so I made one as close as I could to vanilla
        // This override must happen, or else the game crashes completely if you take a screenshot in snapshot mode
        public override ModWaterStyle WaterStyle => ModContent.GetInstance<CityRuinsWaterStyle>();
        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.GetInstance<CityRuinsUndergroundBackgroundStyle>();
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => base.BackgroundColor;
        public Player LastEnteredPlayer;

        public override bool IsBiomeActive(Player player) 
		{
			return false;
		}

        public override int Music
        {
            get
            {
                return MusicLoader.GetMusicSlot(Mod, AudioSystem.GetMusic(LastEnteredPlayer, NPCs.GooType.None, true));
            }
        }

        public override void OnEnter(Player player)
        {
            LastEnteredPlayer = player;
            AudioSystem.RandomizeMusic(player, NPCs.GooType.None);
        }
    }
}
