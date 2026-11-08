using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Backgrounds;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Walls;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Biomes
{
    public class LabRoomGas : ModBiome
	{
        // I don't want a custom water style, so I made one as close as I could to vanilla
        // This override must happen, or else the game crashes completely if you take a screenshot in snapshot mode
        public override ModWaterStyle WaterStyle => ModContent.GetInstance<CityRuinsWaterStyle>();
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<BlackLatexSurfaceBackgroundStyle>();
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;
		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => base.BackgroundColor;
		public override string MapBackground => BackgroundPath;

        public Player LastEnteredPlayer;

		public override bool IsBiomeActive(Player player)
        {
            var playerPos = player.Center.ToTileCoordinates();
            if (WorldGen.InWorld(playerPos.X, playerPos.Y))
            {
                var tile = Main.tile[playerPos.X, playerPos.Y];
                if (tile.WallType == ModContent.WallType<LabGasRoomWall>())
                    return true;
            }
            return false;
        }

		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music
        {
            get 
            { 
                return MusicLoader.GetMusicSlot(Mod, Sounds.MusicGas); 
            }
        }

        public override void OnEnter(Player player)
        {
            LastEnteredPlayer = player;
            AudioSystem.RandomizeMusic(player, NPCs.GooType.None);
            TileAnimationSystem.OnEnterRoom(4);
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
