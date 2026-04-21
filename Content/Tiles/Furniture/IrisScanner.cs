using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class IrisScanner : ModTile
	{
        int nTimesClicked = 0;

        public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            TileID.Sets.IsValidSpawnPoint[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.AnchorWall = true;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(191, 142, 111), Language.GetText("ItemName.IrisScanner"));
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            int spawnX = (i - (tile.TileFrameX / 18)) + (tile.TileFrameX >= 72 ? 5 : 2);
            int spawnY = j + 2;
            player.FindSpawn();

            if (nTimesClicked == 0)
            {
                Main.NewText("Spawn point set!", 255, 240, 20);
                //Main.NewText("Spawn point set!", 255, 240, 20);
                SoundEngine.PlaySound(Assets.Sounds.SoundSave, new Vector2(i * 16, j * 16));
                player.ChangeSpawn(spawnX, spawnY);
            }
            else
            {
                Main.NewText("Spawn point removed!", 255, 240, 20);
                //Main.NewText("Spawn point removed!", 255, 240, 20);
                SoundEngine.PlaySound(Assets.Sounds.SoundLoad, new Vector2(i * 16, j * 16));
                player.RemoveSpawn();
            }

            nTimesClicked = (nTimesClicked + 1) % 2;
            return true;
        }
	}
}
